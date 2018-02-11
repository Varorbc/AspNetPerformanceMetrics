using Metrics;

namespace AspNetPerformance.Metrics
{
    /// <summary>
    /// Performance Metric that updates the counters that track the average time a method took
    /// </summary>
    public class TimerForEachRequestMetric : PerformanceMetricBase
    {
        public TimerForEachRequestMetric(ActionInfo info)
            : base(info)
        {
            string controllerName = actionInfo.ControllerName;
            string actionName = actionInfo.ActionName;
            string counterName = string.Format("{0} {1}", controllerName, actionName);

            averageTimeCounter = Metric.Context(this.actionInfo.ActionType).Timer(counterName, Unit.Requests, SamplingType.ExponentiallyDecaying,
                TimeUnit.Seconds, TimeUnit.Milliseconds);
        }

        private Timer averageTimeCounter;

        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// This method increments the Average Time per Call counter by the number of ticks
        /// </remarks>
        /// <param name="elapsedTicks">A long of the number of ticks it took to complete the action</param>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown) => averageTimeCounter.Record(elapsedTicks, TimeUnit.Nanoseconds);

        /// <summary>
        /// Disposes of the  PerformanceCounter objects when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
