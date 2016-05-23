using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace KeesTalksTech.Utilities.Locking
{
    [TestClass]
    public class NamedMonitorTest
    {
        [TestMethod]
        public void NamedMonitor_ExecuteWithinMonitor_Threaded()
        {
            var monitor = new NamedMonitor();

            int i = 20;

            var thread = new Thread(new ThreadStart(() =>
            {
                monitor.ExecuteWithinMonitor("action", () =>
                {
                    i = 10;
                });
            }));

            monitor.ExecuteWithinMonitor("action", () =>
            {
                thread.Start();
                Thread.Sleep(100);
                Assert.AreEqual(i, 20);
            });

            Thread.Sleep(100);
            Assert.AreEqual(i, 10);
        }

        [TestMethod]
        public void NamedMonitor_EnterExit_Threaded()
        {
            var monitor = new NamedMonitor();

            int i = 20;

            var thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    monitor.Enter("action");
                    i = 10;
                }
                finally
                {
                    monitor.Exit("action");
                }
            }));

            try
            {
                monitor.Enter("action");
                thread.Start();
                Thread.Sleep(100);
                Assert.AreEqual(i, 20);
            }
            finally
            {
                monitor.Exit("action");
            }

            Thread.Sleep(100);
            Assert.AreEqual(i, 10);
        }
    }
}