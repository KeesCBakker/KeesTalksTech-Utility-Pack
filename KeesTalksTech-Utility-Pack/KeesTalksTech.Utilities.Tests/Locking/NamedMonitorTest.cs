using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace KeesTalksTech.Utilities.Locking
{
    [TestClass]
    public class NamedMonitorTest
    {
        [TestMethod]
		[TestCategory("UnitTest")]
        public void NamedMonitor_Exclusion()
        {
            var monitor = new NamedMonitor();

            try
            {
                monitor.Enter("A");
                monitor.Enter("B");
            }
            finally
            {
                monitor.Exit("B");
                monitor.Exit("A");
            }
        }

        [TestMethod]
		[TestCategory("UnitTest")]
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
		[TestCategory("UnitTest")]
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