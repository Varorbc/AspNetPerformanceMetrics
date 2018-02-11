using AspNetPerformance.Metrics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspNetPerformance
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PerformanceTracker
    {
        public PerformanceTracker(ActionInfo info) => actionInfo = info;

        /// <summary>
        /// Hold info about the action being tracked
        /// </summary>
        private ActionInfo actionInfo;

        /// <summary>
        /// Stopwatch to time how long the action took
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// Collection of all the performance metrics to be tracked
        /// </summary>
        private List<PerformanceMetricBase> performanceMetrics;

        public void ProcessActionStart()
        {
            try
            {
                // Use the factory class to get all of the performance metrics that are being tracked
                // for MVC Actions
                performanceMetrics = PerformanceMetricFactory.GetPerformanceMetrics(actionInfo);

                // Iterate through each metric and call the OnActionStart() method
                // Start off a task to do this so it can it does not block and minimized impact to the user
                Task t = Task.Factory.StartNew(() =>
                {
                    foreach (PerformanceMetricBase m in performanceMetrics)
                    {
                        m.OnActionStart();
                    }
                });

                stopwatch = Stopwatch.StartNew();
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception {0} occurred PerformanceTracker.ProcessActionStart().  Message {1}\nStackTrace {0}",
                    ex.GetType().FullName, ex.Message, ex.StackTrace);
                Trace.WriteLine(message);
            }
        }

        public void ProcessActionComplete(bool unhandledExceptionFlag)
        {
            try
            {
                // Stop the stopwatch
                stopwatch.Stop();

                // Iterate through each metric and call the OnActionComplete() method
                // Start off a task to do this so it can it does not block and minimized impact to the user
                Task t = Task.Factory.StartNew(() =>
                {
                    foreach (PerformanceMetricBase m in performanceMetrics)
                    {
                        m.OnActionComplete(stopwatch.ElapsedTicks, unhandledExceptionFlag);
                    }
                });
            }
            catch (Exception ex)
            {
                string message = string.Format("Exception {0} occurred PerformanceTracker.ProcessActionComplete().  Message {1}\nStackTrace {0}",
                    ex.GetType().FullName, ex.Message, ex.StackTrace);
                Trace.WriteLine(message);
            }
        }
    }
}
