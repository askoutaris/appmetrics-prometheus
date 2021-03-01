using App.Metrics;

namespace AppMetricsPrometheus
{
	public interface IPrometheusFormatter
	{
		string GetOutput(MetricsDataValueSource snapshot);
	}
}
