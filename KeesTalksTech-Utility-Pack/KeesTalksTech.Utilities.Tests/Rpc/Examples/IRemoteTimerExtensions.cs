namespace KeesTalksTech.Utilities.UnitTests.Rpc.Examples
{
    public static class IRemoteTimerExtensions
    {
        /// <summary>
        /// Pauses the specified timer.
        /// </summary>
        /// <param name="timer">The timer.</param>
        public static void Pause(this IRemoteTimer timer)
        {
            if (timer.IsRunning)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }
    }
}
