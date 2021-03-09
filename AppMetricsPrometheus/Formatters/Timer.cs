using System.Globalization;
using System.IO;
using App.Metrics;
using App.Metrics.Timer;

namespace AppMetricsPrometheus.Formatters
{
	public interface ITimerFormatter
	{
		void Write(StreamWriter streamWriter, MetricsContextValueSource context, TimerValueSource metric);
	}

	public class TimerFormatter : BaseFormatter, ITimerFormatter
	{
		public void Write(StreamWriter streamWriter, MetricsContextValueSource metricContext, TimerValueSource metric)
		{
			WriteMetricName(streamWriter, metric.MultidimensionalName);

			var timerValue = metric.ValueProvider.GetValue(metric.ResetOnReporting);
			var value = timerValue.Histogram;

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
