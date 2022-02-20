using App.Metrics;
using App.Metrics.Counter;

namespace Handler
{
    public class MetricHandler
    {
        public static CounterOptions RequestCount   = new() { Name = "api_request_count", MeasurementUnit   = Unit.Calls};
        public static CounterOptions RequestSuccess = new() { Name = "api_request_success", MeasurementUnit = Unit.Calls };
        public static CounterOptions RequestFailed  = new() { Name = "api_request_failed", MeasurementUnit  = Unit.Calls };

        readonly IMetricsRoot metrics;
        
        public MetricHandler(IMetricsRoot metrics)
        {
            this.metrics = metrics;
        }

        public void Increment(CounterOptions counter)
        {
            metrics.Measure.Counter.Increment(counter);
        }
    }
}