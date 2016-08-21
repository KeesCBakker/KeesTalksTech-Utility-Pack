using System;
using System.Timers;

namespace KeesTalksTech.Utilities.UnitTests.Rpc.Examples
{
    public class RemoteTimer : IRemoteTimer, IDisposable
    {
        private Timer timer;
        private int ticks = 0;

        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public double Interval { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteTimer" /> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        public RemoteTimer(double interval)
        {
            SetInterval(interval);
        }

        /// <summary>
        /// Sets the interval.
        /// </summary>
        /// <param name="interval">The interval.</param>
        public void SetInterval(double interval)
        {
            var isRunning = (timer?.Enabled).GetValueOrDefault();

            timer?.Stop();
            timer?.Dispose();

            timer = new Timer(interval);
            timer.Elapsed += (s, e) => ticks++;

            Interval = interval;

            if (isRunning)
            {
                Start();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return timer.Enabled; }
        }

        /// <summary>
        /// Gets the ticks total.
        /// </summary>
        /// <returns>
        /// The ticks.
        /// </returns>
        public int GetTicksTotal()
        {
            return ticks;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
