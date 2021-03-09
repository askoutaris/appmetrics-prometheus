using System.Globalization;
using System.IO;
using App.Metrics;
using App.Metrics.Meter;

namespace AppMetricsPrometheus.Formatters
{
	public interface IMeterFormatter
	{
		void Write(StreamWriter streamWriter, MetricsContextValueSource context, MeterValueSource metric);
	}

	public class MeterFormatter : BaseFormatter, IMeterFormatter
	{
		public void Write(StreamWriter streamWriter, MetricsContextValueSource metricContext, MeterValueSource metric)
		{
			WriteMetricName(streamWriter, metric.MultidimensionalName);

			var value = metric.ValueProvider.GetValue(metric.ResetOnReporting);
			var fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags);
			streamWriter.WriteLine($"{fullName} {value.Count.ToString(CultureInfo.InvariantCulture)}");
		}
	}
}
