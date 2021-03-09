using System.Globalization;
using System.IO;
using App.Metrics;
using App.Metrics.Histogram;

namespace AppMetricsPrometheus.Formatters
{
	public interface IHistogramFormatter
	{
		void Write(StreamWriter streamWriter, MetricsContextValueSource context, HistogramValueSource metric);
	}

	public class HistogramFormatter : BaseFormatter, IHistogramFormatter
	{
		public void Write(StreamWriter streamWriter, MetricsContextValueSource metricContext, HistogramValueSource metric)
		{
			WriteMetricName(streamWriter, metric.MultidimensionalName);

			var value = metric.ValueProvider.GetValue(metric.ResetOnReporting);

			var fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Count));
			streamWriter.WriteLine($"{fullName} {value.Count.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Max));
			streamWriter.WriteLine($"{fullName} {value.Max.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Mean));
			streamWriter.WriteLine($"{fullName} {value.Mean.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Median));
			streamWriter.WriteLine($"{fullName} {value.Median.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Min));
			streamWriter.WriteLine($"{fullName} {value.Min.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.SampleSize));
			streamWriter.WriteLine($"{fullName} {value.SampleSize.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.StdDev));
			streamWriter.WriteLine($"{fullName} {value.StdDev.ToString(CultureInfo.InvariantCulture)}");
			fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags, nameof(value.Sum));
			streamWriter.WriteLine($"{fullName} {value.Sum.ToString(CultureInfo.InvariantCulture)}");
		}
	}
}
