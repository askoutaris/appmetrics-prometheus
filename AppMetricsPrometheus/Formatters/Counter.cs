using System.Globalization;
using System.IO;
using App.Metrics;
using App.Metrics.Counter;

namespace AppMetricsPrometheus.Formatters
{
	public interface ICounterFormatter
	{
		void Write(StreamWriter streamWriter, MetricsContextValueSource context, CounterValueSource metric);
	}

	public class CounterFormatter : BaseFormatter, ICounterFormatter
	{
		public void Write(StreamWriter streamWriter, MetricsContextValueSource metricContext, CounterValueSource metric)
		{
			WriteMetricName(streamWriter, metric.MultidimensionalName);

			var value = metric.ValueProvider.GetValue(metric.ResetOnReporting);
			var fullName = GetMetricRow(metricContext.Context, metric.MultidimensionalName, metric.Tags);
			streamWriter.WriteLine($"{fullName} {value.Count.ToString(CultureInfo.InvariantCulture)}");
		}

		protected override void WriteMetricName(StreamWriter streamWriter, string metricName)
		{
			var name = GetMetricName(metricName);
			streamWriter.WriteLine($"# HELP {name} values");
			streamWriter.WriteLine($"# TYPE {name} counter");
		}
	}
}
