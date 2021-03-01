using System.Globalization;
using System.IO;
using App.Metrics;
using App.Metrics.Gauge;

namespace AppMetricsPrometheus.Formatters
{
	public interface IGaugeFormatter
	{
		void Write(StreamWriter streamWriter, MetricsContextValueSource context, GaugeValueSource metric);
	}

	public class GaugeFormatter : BaseFormatter, IGaugeFormatter
	{
		public void Write(StreamWriter streamWriter, MetricsContextValueSource metricContext, GaugeValueSource metric)
		{
			WriteMetricName(streamWriter, metricContext, metric.MultidimensionalName);

			var value = metric.ValueProvider.GetValue(metric.ResetOnReporting);
			var fullName = GetFullMetricName(metricContext.Context, metric.MultidimensionalName, metric.Tags);
			streamWriter.WriteLine($"{fullName} {value.ToString(CultureInfo.InvariantCulture)}");
		}
	}
}
