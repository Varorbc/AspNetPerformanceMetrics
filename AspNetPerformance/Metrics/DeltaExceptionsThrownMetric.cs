using Metrics;

namespace AspNetPerformance.Metrics
{
    /// <summary>
    /// Performance Metric to track the count of exceptions thrown by a controller action in the last
    /// time period
    /// </summary>
    public class DeltaExceptionsThrownMetric : PerformanceMetricBase
    {
        public DeltaExceptionsThrownMetric(ActionInfo info)
            : base(info)
        {
            deltaExceptionsThrownCounter = Metric.Context(actionInfo.ActionType).Meter(COUNTER_NAME, Unit.Errors, TimeUnit.Seconds);
        }

        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const string COUNTER_NAME = "Errors";

        /// <summary>
        /// Reference to the performance counter 
        /// </summary>
        private Meter deltaExceptionsThrownCounter;

        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// If exceptionThrown is true, then the Total Exceptions Thrown counter will be 
        /// incremented by 1
        /// </remarks>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            if (exceptionThrown)
            {
                deltaExceptionsThrownCounter.Mark();
            }
        }

        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
