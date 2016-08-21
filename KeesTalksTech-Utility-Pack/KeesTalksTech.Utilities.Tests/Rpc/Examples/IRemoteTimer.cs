namespace KeesTalksTech.Utilities.UnitTests.Rpc.Examples
{
    /// <summary>
    /// Indicates the object implements a timer.
    /// </summary>
    public interface IRemoteTimer
    {
        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the ticks total.
        /// </summary>
        /// <returns>The ticks.</returns>
        int GetTicksTotal();

        /// <summary>
        /// Sets the interval.
        /// </summary>
        /// <param name="newInterval">The new interval.</param>
        void SetInterval(double newInterval);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}