using System.IO;
using System.Linq;
using App.Metrics;
using App.Metrics.Apdex;

namespace AppMetricsPrometheus.Formatters
{
	public abstract class BaseFormatter
	{
		protected string GetMetricName(string context, string metricName)
		{
			var name = ($"{context}_{metricName}".ToLower());
			return Constants.ValidPrometheusNameCharsRegex.Replace(name, "_");
		}

		protected string GetFullMetricName(string context, string metricName, MetricTags metricTags, string? suffix = null)
		{
			string fullMetricName = GetMetricName(context, metricName);
			if (suffix != null)
				fullMetricName = $"{fullMetricName}_{suffix}";

			if (metricTags.Count == 0)
				return fullMetricName;

			var tagKeyValues = metricTags.Keys
				.Zip(metricTags.Values, (key, value) => $"{key}=\"{value}\"");

			var tagsStr = string.Join(",", tagKeyValues);

			return $"{fullMetricName}{{{tagsStr}}}".ToLower();
		}

		protected void WriteMetricName(StreamWriter streamWriter, MetricsContextValueSource metricContext, string metricName)
		{
			var name = GetMetricName(metricContext.Context, metricName);
			streamWriter.WriteLine($"# HELP {name} values");
			streamWriter.WriteLine($"# TYPE {name} gauge");
		}
	}
}
