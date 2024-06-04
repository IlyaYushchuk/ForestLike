using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ForestLike;

using Timer = System.Timers.Timer;

public class ActivityObserver
{
    [DllImport("user32.dll")]
    static extern int GetForegroundWindow(); 
    [DllImport("user32")]
    private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);
    [DllImport("user32.dll")]
    private static extern int EnumWindows(EnumWindowsProc enumProc, int lParam);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowTextLength(IntPtr hWnd);


    delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);



    private static ActivityObserver instance;
    private Timer activityCheckTimer;

    public event Action NotAllowedAppUsed;
    public List<string> allowedApps = new List<string>();


    private ActivityObserver()
    {
        activityCheckTimer = new Timer(new TimeSpan(0,0,5));
        activityCheckTimer.AutoReset = true;
        activityCheckTimer.Elapsed += (s,e)=>
        {
            Observe();
        };
        var currWind = GetActiveWindowTitle();
        allowedApps.Add(currWind);
    }
    public string GetActiveWindowTitle()
    {
        IntPtr handle = GetForegroundWindow();
        if (handle == IntPtr.Zero)
        {
            return null;
        }

        StringBuilder sb = new StringBuilder(256);
        int length = GetWindowText(handle, sb, sb.Capacity);
        if (length > 0)
        {
            return sb.ToString();
        }

        return null;
    }
    public static ActivityObserver GetInstance()
    {
        if (instance == null)
        {
            instance = new ActivityObserver();
        }
        return instance;
    }

    public void StartObserveTimer()
    {
        activityCheckTimer.Start();
    }
    public void StopObserveTimer()
    {
        activityCheckTimer.Stop();
    }

    public IEnumerable<string> GetAllOpenedWindows()
    {
        List<string> windowTitles = new List<string>();
        EnumWindows(delegate (IntPtr hWnd, int lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                int length = GetWindowTextLength(hWnd);
                if (length > 0)
                {
                    var builder = new System.Text.StringBuilder(length + 1);
                    GetWindowText(hWnd, builder, builder.Capacity);
                    string title = builder.ToString();
                    if (!string.IsNullOrEmpty(title))
                    {
                        windowTitles.Add(title);
                    }
                }
            }
            return true;
        }, 0);
        return windowTitles;
    }

    
    private void Observe()
    {
        var currWind = GetActiveWindowTitle();
        if (!allowedApps.Contains(currWind))
        { 
            NotAllowedAppUsed?.Invoke();
        }
    }
    
    public void AddAllowedApp(string windName)
    {
        allowedApps.Add(windName);
    }
    public void RemoveAllowedApp(string windName)
    {
        allowedApps.Remove(windName);
    }
}
