using Metrics;

namespace AspNetPerformance.Metrics
{
    /// <summary>
    /// Performance metric to update the counter that tracks the number of times
    /// an action has been called in the last reporting period
    /// </summary>
    public class DeltaCallsMetric : PerformanceMetricBase
    {
        public DeltaCallsMetric(ActionInfo info)
            : base(info)
        {
            string categoryName = actionInfo.ControllerName;
            string instanceName = actionInfo.ActionName;
            string counterName = string.Format("{0} {1} {2}", categoryName, instanceName, COUNTER_NAME);
            deltaCallsCounter = Metric.Context(actionInfo.ActionType).Counter(counterName, Unit.Requests);
        }

        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const string COUNTER_NAME = "Delta Calls";

        /// <summary>
        /// Reference to the counter to be updated
        /// </summary>
        private Counter deltaCallsCounter;

        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// This method increments the "Delta Calls" counter by 1.  It does not use the
        /// elapsedTicks that is passed in
        /// </remarks>
        /// <param name="elapsedTicks">A long of the ticks it took the action to complete (not used)</param>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown) => deltaCallsCounter.Increment();

        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
