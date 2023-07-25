using System;
using System.Diagnostics;
using System.Threading;

namespace Utils
{
    public class MyStopWatch
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public void ProcessStopwath<T>(Action<T> Action, T parm, int printOverTime = 10)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            Action.Invoke(parm);

            _stopwatch.Stop();
            if (_stopwatch.ElapsedMilliseconds > printOverTime)
                Console.WriteLine($"{Action.Method.Name} == {_stopwatch.ElapsedMilliseconds} mils");
        }

        private double _totalavg = 0;
        public int ProcessStopwath(Action Action, bool isPrint = true, int printOverTime = 10)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var start = stopwatch.ElapsedMilliseconds;
            Action.Invoke();
            var gap = stopwatch.ElapsedMilliseconds - start;

            stopwatch.Stop();

            if (gap > printOverTime && isPrint)
            {
                _totalavg = (_totalavg + gap) / 2;
                var tmp = $"{Action.Method.Name} == {Thread.CurrentThread.ManagedThreadId} =={gap} mils===avg:::{_totalavg}";
                Console.WriteLine(tmp);
                //_ILog.Info(tmp);
            }

            return (int)gap;
        }

        public static int ProcessStopwacthByStatic(Action Action, bool isPrint = true, int printOverTime = 10, string tag = "")
        {
            var stopwatch = Stopwatch.StartNew();
            Action.Invoke();

            var gap = stopwatch.ElapsedMilliseconds;
            if (gap > printOverTime && isPrint)
                Debug.WriteLine($"{DateTime.Now}\t {Action.Method.Name} == {tag} == {stopwatch.ElapsedMilliseconds} mils");

            return (int)gap;
        }
    }
}