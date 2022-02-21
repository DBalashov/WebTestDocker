using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Timer;

namespace Handler
{
    public class MetricHandler
    {
        public static CounterOptions LocalRequestCount   = new() { Name = "local_request_count", MeasurementUnit   = Unit.Requests };
        public static GaugeOptions   LocalItemsCount     = new() { Name = "local_items_count", MeasurementUnit     = Unit.Items };
        public static CounterOptions LocalRequestSuccess = new() { Name = "local_request_success", MeasurementUnit = Unit.Requests };
        public static CounterOptions LocalRequestFailed  = new() { Name = "local_request_failed", MeasurementUnit  = Unit.Requests };

        public static CounterOptions RemoteRequestCount   = new() { Name = "remote_request_count", MeasurementUnit   = Unit.Requests };
        public static GaugeOptions   RemoteItemsCount     = new() { Name = "remote_items_count", MeasurementUnit     = Unit.Items };
        public static CounterOptions RemoteRequestSuccess = new() { Name = "remote_request_success", MeasurementUnit = Unit.Requests };
        public static CounterOptions RemoteRequestFailed  = new() { Name = "remote_request_failed", MeasurementUnit  = Unit.Requests };

        public static TimerOptions RemoteRequestDuration = new() { Name = "remote_request_duration", MeasurementUnit = Unit.Custom("ms") };
        public static TimerOptions LocalRequestDuration  = new() { Name = "local_request_duration", MeasurementUnit  = Unit.Custom("ms") };


        readonly IMetricsRoot metrics;

        public MetricHandler(IMetricsRoot metrics)
        {
            this.metrics = metrics;
        }

        public void Increment(CounterOptions counter)
        {
            metrics.Measure.Counter.Increment(counter);
        }

        public void Set(GaugeOptions counter, int value)
        {
            metrics.Measure.Gauge.SetValue(counter, value);
        }

        public void Set(TimerOptions counter, long value)
        {
            metrics.Measure.Timer.Time(counter, value);
        }
    }
}