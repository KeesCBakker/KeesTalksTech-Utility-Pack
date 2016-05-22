using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeesTalksTech.Utiltities.Locking
{
	public class NamedMonitor
	{
		private Dictionary<string, object> _monitors = new Dictionary<string, object>();
		private object _mutex = new object();
		private string _prefix = Guid.NewGuid().ToString();

		public void Enter(string key)
		{
			var l = GetLockingObject(key);
			Monitor.Enter(l);
		}

		public void Exit(string key)
		{
			var l = GetLockingObject(key);
			Monitor.Exit(l);
		}

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