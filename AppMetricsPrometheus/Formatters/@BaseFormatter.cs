using System.IO;
using System.Linq;
using App.Metrics;

namespace AppMetricsPrometheus.Formatters
{
	public abstract class BaseFormatter
	{
		protected string GetMetricName(string metricName)
		{
			var name = metricName.ToLower();
			return Constants.ValidPrometheusNameCharsRegex.Replace(name, "_");
		}

		protected string GetMetricRow(string context, string metricName, MetricTags metricTags, string? suffix = null)
		{
			string fullMetricName = GetMetricName(metricName);
			if (suffix != null)
				fullMetricName = $"{fullMetricName}_{suffix}";

			if (metricTags.Count == 0)
				return fullMetricName;

			var tagKeyValues = metricTags.Keys
				.Zip(metricTags.Values, (key, value) => $"{key}=\"{value}\"");

			var tagsStr = string.Join(",", tagKeyValues);
			var contextTag = $"context=\"{context}\"";

			return $"{fullMetricName}{{{tagsStr},{contextTag}}}".ToLower();
		}

		protected void WriteMetricName(StreamWriter streamWriter, string metricName)
		{
			var name = GetMetricName(metricName);
			streamWriter.WriteLine($"# HELP {name} values");
			streamWriter.WriteLine($"# TYPE {name} gauge");
		}
	}
}
