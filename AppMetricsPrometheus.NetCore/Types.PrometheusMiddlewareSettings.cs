namespace AppMetricsPrometheus.NetCore
{
	public class AppMetricsPrometheusSettings
	{
		public string? MetricsPath { get; set; }
		public IPrometheusFormatter? Formatter { get; set; }
	}
}
