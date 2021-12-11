using System;
using System.Diagnostics;

namespace LZ.Compressions.UI.Services
{
    public interface ITimerService
    {
        void Start();
        TimeSpan Stop();
    }

    public class TimerService : ITimerService
    {
        private readonly Stopwatch _timer = new Stopwatch();

        public void Start()
        {
            _timer.Reset();
            _timer.Start();
        }

        public TimeSpan Stop()
        {
            _timer.Stop();
            return _timer.Elapsed;
        }
    }
}
