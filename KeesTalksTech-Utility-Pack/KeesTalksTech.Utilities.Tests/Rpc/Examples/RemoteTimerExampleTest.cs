using KeesTalksTech.Utilities.Rpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace KeesTalksTech.Utilities.UnitTests.Rpc.Examples
{
    [TestClass]
    public class RemoteTimerExampleTest
    {
        [TestMethod]
        public void TestRemoteTimer()
        {
            var json = "";
            var timer = new RemoteTimer(10);
            var interpretor = Interpretation.Create<IRemoteTimer>(timer, typeof(IRemoteTimerExtensions));

            Assert.IsFalse(timer.IsRunning);
            Assert.AreEqual(10, timer.Interval);

            /* Start the timer and assert it is running */
            json = @"{ ""method-name"": ""Start"" }";
            interpretor.Execute(json);

            Assert.IsTrue(timer.IsRunning);

            Thread.Sleep(50);

            /* Pause the timer twice and assert running state */

            json = @"{ ""method-name"": ""Pause"" }";
            interpretor.Execute(json);
            Assert.IsFalse(timer.IsRunning);

            json = @"{ ""method-name"": ""Pause"" }";
            interpretor.Execute(json);
            Assert.IsTrue(timer.IsRunning);

            Thread.Sleep(50);

            /* Stop the timer, set a new interval and restart it: */
            json =   @"[{ ""method-name"": ""Stop""},
                        { ""method-name"": ""SetInterval"", ""newInterval"": 20 },
                        { ""method-name"": ""Start""}]";

            interpretor.Execute(json);
            Assert.AreEqual(20, timer.Interval);

            Thread.Sleep(100);

            /* Get tick information from timer */
            json = @"{ ""method-name"": ""GetTicksTotal""}";
            var ticks = interpretor.Execute(json);

            Assert.IsNotNull(ticks);
            Assert.IsInstanceOfType(ticks, typeof(int));
            Assert.IsTrue(Convert.ToInt32(ticks) > 0);

            /* Try to execute public method on RemoteTimer */
            bool error = false;
            json = @"{ ""method-name"": ""Dispose"" }";

            try
            {
                interpretor.Execute(json);
            }
            catch { error = true; }

            Assert.IsTrue(error);

            //dispose timer because the test is ready
            timer.Dispose();
        }
    }
}
