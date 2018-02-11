using AspNetPerformance.Metrics;
using System.Collections.Generic;
using System.Linq;

namespace AspNetPerformance
{
    /// <summary>
    /// Class to hold all of the PerformanceMetricBase objects associated with an ActionInfo
    /// (effectively all the performance metrics associated with an action)
    /// </summary>
    public class PerformanceMetricContainer
    {
        /// <summary>
        /// Creates a new PerformanceMetricContainer object
        /// </summary>
        /// <param name="actionInfo">An ActionInfo object that describes the action the metrics will apply to</param>
        public PerformanceMetricContainer(ActionInfo actionInfo, List<PerformanceMetricBase> metrics)
        {
            ActionInfo = actionInfo;
            performanceMetrics = metrics;
        }

        private List<PerformanceMetricBase> performanceMetrics;

        /// <summary>
        /// An ActionInfo object which describes the controller action the metrics contained in this object apply to
        /// </summary>
        public ActionInfo ActionInfo { get; private set; }

        /// <summary>
        /// Gets a list of all the Performance metrics associated with the ActionInfo in this contianer
        /// </summary>
        /// <returns>A List of PerformanceMetricBase objects</returns>
        public List<PerformanceMetricBase> GetPerformanceMetrics() => performanceMetrics.ToList();

        /// <summary>
        /// Calls the Dispose() method on all the PerformaneMetricBase objects.   This only
        /// gets called when the application exits
        /// </summary>
        internal void DisposePerformanceMetrics()
        {
            foreach (PerformanceMetricBase metric in performanceMetrics)
            {
                metric.Dispose();
            }
        }
    }
}
