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

    public event Action NotAllowedAppUsed;

    private static ActivityObserver instance;
    
    private ActivityObserver()
    {
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


    public IEnumerable<Process> GetAllOpenedWindows()
    {
        // Process[] processlist = Process.GetProcesses();

        //TODO delete this code
        //foreach (Process process in processlist)
        //{
        //    if (!String.IsNullOrEmpty(process.MainWindowTitle))
        //    {
        //        Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
        //    }
        //}
        return Process.GetProcesses();
    }

    //public async void KostilFunc()
    //{
    //        int j = 0;
    //        while (j < 1000)
    //        {
    //            var currWind = GetForegroundWindow();
    //            //if (!allowedApps.Contains(Process.GetProcessById(GetWindowProcessID(currWind)).Id))
    //            //{
    //            //NotAllowedAppUsed?.Invoke();
    //            Process process = Process.GetProcessById(GetWindowProcessID(currWind));
    //            Console.WriteLine("Process: {0} ID: {1} more info {2} something {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.MainWindowHandle);
    //            //  }
    //            await Task.Delay(1000);
    //            j++;
    //        }
    //}
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
