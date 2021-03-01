using System.Globalization;
using System.IO;
using App.Metrics;
using App.Metrics.Apdex;

namespace AppMetricsPrometheus.Formatters
{
	public interface IApdexFormatter
	{
		void Write(StreamWriter streamWriter, MetricsContextValueSource context, ApdexValueSource metric);
	}

	public class ApdexFormatter : BaseFormatter, IApdexFormatter
	{
		public void Write(StreamWriter streamWriter, MetricsContextValueSource metricContext, ApdexValueSource metric)
		{
			WriteMetricName(streamWriter, metricContext, metric.MultidimensionalName);

			var value = metric.ValueProvider.GetValue(metric.ResetOnReporting);

			var fullName = GetFullMetricName(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Frustrating));
			streamWriter.WriteLine($"{fullName} {value.Frustrating.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetFullMetricName(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.SampleSize));
			streamWriter.WriteLine($"{fullName} {value.SampleSize.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetFullMetricName(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Satisfied));
			streamWriter.WriteLine($"{fullName} {value.Satisfied.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetFullMetricName(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Score));
			streamWriter.WriteLine($"{fullName} {value.Score.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetFullMetricName(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Tolerating));
			streamWriter.WriteLine($"{fullName} {value.Tolerating.ToString(CultureInfo.InvariantCulture)}");
		}
	}
}
