using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ThreadPriority;

/// <summary>
/// Interaction logic for FrmTrackThread.xaml
/// </summary>
public partial class FrmTrackThread
{
    public FrmTrackThread()
    {
        InitializeComponent();
    }

    private void OnRun(object sender, RoutedEventArgs e)
    {
        Task.Run(() => { Dispatcher.Invoke(() => { ThreadInfo.Text = "- Thread Starts - "; }); })
            .GetAwaiter()
            .OnCompleted(() =>
            {
                try
                {
                    Console.WriteLine(ThreadInfo.Text);

                    var threadA = new Thread(MyThreadClass.Thread1);
                    var threadB = new Thread(MyThreadClass.Thread2);
                    var threadC = new Thread(MyThreadClass.Thread1);
                    var threadD = new Thread(MyThreadClass.Thread2);
                    threadA.Priority = System.Threading.ThreadPriority.Highest;
                    threadB.Priority = System.Threading.ThreadPriority.Normal;
                    threadC.Priority = System.Threading.ThreadPriority.AboveNormal;
                    threadD.Priority = System.Threading.ThreadPriority.BelowNormal;
                    threadA.Name = "Thread A";
                    threadB.Name = "Thread B";
                    threadC.Name = "Thread C";
                    threadD.Name = "Thread D";

                    threadA.Start();
                    threadB.Start();
                    threadC.Start();
                    threadD.Start();

                    threadA.Join();
                    threadB.Join();
                    threadC.Join();
                    threadD.Join();
                }
                catch (ThreadAbortException exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
                finally
                {
                    Dispatcher.Invoke(() => { ThreadInfo.Text = "- Thread Ends - "; });
                    Console.WriteLine(ThreadInfo.Text);
                }
            });
    }
}

internal static class MyThreadClass
{
    public static void Thread1()
    {
        var thread = Thread.CurrentThread;

        for (var loopCount = 0; loopCount <= 2; loopCount++)
        {
            Console.WriteLine($"Name of Thread: {thread.Name} Process = {loopCount}");
            Thread.Sleep(500);
        }

        Console.WriteLine(
            $"The thread {thread.Name} has exited with code {(int)thread.ThreadState} (0x{(int)thread.ThreadState}).");
    }

    public static void Thread2()
    {
        var thread = Thread.CurrentThread;
        for (var loopCount = 0; loopCount < 6; loopCount++)
        {
            Console.WriteLine($"Name of Thread: {thread.Name} Process = {loopCount}");
            Thread.Sleep(1500);
        }

        Console.WriteLine(
            $"The thread {thread.Name} has exited with code {(int)thread.ThreadState} (0x{(int)thread.ThreadState}).");
    }
}