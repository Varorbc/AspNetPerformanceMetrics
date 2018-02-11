using Metrics;

namespace AspNetPerformance.Metrics
{
    public class PostAndPutRequestSizeMetric : PerformanceMetricBase
    {
        public PostAndPutRequestSizeMetric(ActionInfo info)
            : base(info)
        {
            histogram = Metric.Context(actionInfo.ActionType).Histogram(COUNTER_NAME, Unit.Bytes, SamplingType.ExponentiallyDecaying);
        }

        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const string COUNTER_NAME = "Post & Put Request Size";

        /// <summary>
        /// Reference to the performance counter 
        /// </summary>
        private Histogram histogram;

        public override void OnActionStart()
        {
            var method = actionInfo.HttpMethod.ToUpper();
            if (method == "POST" || method == "PUT")
            {
                histogram.Update(actionInfo.ContentLength);
            }
        }

        public override void Dispose()
        {
        }
    }
}
