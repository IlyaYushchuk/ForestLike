using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ForestLike;

using Timer = System.Timers.Timer;

public class ActivityObserver
{
    //TODO temporary list but may be it will be replaced
    List<Process> allowedApps = new List<Process>();
    [DllImport("user32.dll")]
    static extern int GetForegroundWindow(); 
    [DllImport("user32")]
    private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);
    private static ActivityObserver instance;
    private Timer activityCheckTimer;

    public event Action NotAllowedAppUsed;

    private ActivityObserver()
    {
        activityCheckTimer = new Timer(new TimeSpan(0,0,1));
        activityCheckTimer.AutoReset = true;
        activityCheckTimer.Elapsed += (s,e)=>{
            Observe();
        };
        var hwnd = GetForegroundWindow();
        allowedApps.Add(Process.GetProcessById(GetWindowProcessID(hwnd)));
        Process process = Process.GetProcessById(GetWindowProcessID(hwnd));
        Console.WriteLine("Process: {0} ID: {1} ", process.ProcessName, process.Id);
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

    public IEnumerable<Process> GetAllOpenedWindows()
    {
        return Process.GetProcesses();
    }
    private void Observe()
    {
        var currWind = GetForegroundWindow();
        Process process = Process.GetProcessById(GetWindowProcessID(currWind));
        if (allowedApps.Find(p => p.Id == process.Id) == null)
        { 
            NotAllowedAppUsed?.Invoke();
            Console.WriteLine("Process: {0} ID: {1} ", process.ProcessName, process.Id);
        }
    }
    private Int32 GetWindowProcessID(Int32 hwnd)
    {
        Int32 pid = 1;
        GetWindowThreadProcessId(hwnd, out pid);
        return pid;
    }
    public void AddAllowedApp(Process process)
    {
        allowedApps.Add(process);
    }
    public void RemoveAllowedApp(Process process)
    {
        allowedApps.Remove(process);
    }
}
