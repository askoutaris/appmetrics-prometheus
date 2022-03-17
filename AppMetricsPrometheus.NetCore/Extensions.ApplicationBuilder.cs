using AppMetricsPrometheus.Formatters;
using Microsoft.AspNetCore.Builder;

namespace AppMetricsPrometheus.NetCore
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseAppMetricsPrometheus(this IApplicationBuilder app)
			=> app.UseAppMetricsPrometheus(new AppMetricsPrometheusSettings());

		public static IApplicationBuilder UseAppMetricsPrometheus(this IApplicationBuilder app, AppMetricsPrometheusSettings settings)
		{
			var formatter = settings.Formatter ?? GetDefaultFormatter();

			return app.UseMiddleware<AppMetricsPrometheusMiddleware>(settings, formatter);
		}

		private static IPrometheusFormatter GetDefaultFormatter()
		{
			return new PrometheusFormatter(
				encoding: Constants.UTF8Encoding,
				newLineFormat: NewLineFormat.Default,
				apdexFormatter: new ApdexFormatter(),
				counterFormatter: new CounterFormatter(),
				gaugeFormatter: new GaugeFormatter(),
				histogramFormatter: new HistogramFormatter(),
				meterFormatter: new MeterFormatter(),
				timerFormatter: new TimerFormatter());
		}
	}
}
