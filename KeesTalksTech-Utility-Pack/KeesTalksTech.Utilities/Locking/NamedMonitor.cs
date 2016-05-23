using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeesTalksTech.Utilities.Locking
{
    /// <summary>
    /// A NamedMonitor creates a locking mechanism that will lock on keys. 
    /// </summary>
    public class NamedMonitor
	{
		private Dictionary<string, object> _monitors = new Dictionary<string, object>();
		private object _mutex = new object();
		private string _prefix = Guid.NewGuid().ToString();

        /// <summary>
        /// Enters the monitor.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Enter(string key)
		{
			var l = GetLockingObject(key);
			Monitor.Enter(l);
		}

        /// <summary>
        /// Exits the monitor.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Exit(string key)
		{
			var l = GetLockingObject(key);
			Monitor.Exit(l);
		}

        /// <summary>
        /// Gets the locking object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The object that can be used for locking.</returns>
        private object GetLockingObject(string key)
		{
			lock (_mutex)
			{
				key = _prefix + "." + key;

				if (!_monitors.ContainsKey(key))
				{
					var l = new object();
					_monitors.Add(key, l);
					return l;
				}

				return _monitors[key];
			}
		}

        /// <summary>
        /// Executes the function within a monitor.
        /// </summary>
        /// <typeparam name="T">The resulting type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="func">The function.</param>
        /// <returns>An async task.</returns>
        public async Task<T> ExecuteWithinMonitor<T>(string key, Func<Task<T>> func)
		{
			try
			{
				Enter(key);
				return await func();
			}
			finally
			{
				Exit(key);
			}
		}

        /// <summary>
        /// Executes the function within monitor.
        /// </summary>
        /// <typeparam name="T">The resulting type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="func">The function.</param>
        /// <returns>The result.</returns>
        public T ExecuteWithinMonitor<T>(string key, Func<T> func)
		{
			try
			{
				Enter(key);
				return func();
			}
			finally
			{
				Exit(key);
			}
		}

        /// <summary>
        /// Executes the action within the monitor.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="action">The action.</param>
        public void ExecuteWithinMonitor(string key, Action action)
		{
			try
			{
				Enter(key);
				action();
			}
			finally
			{
				Exit(key);
			}
		}
    }
}