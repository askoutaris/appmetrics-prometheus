using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace AppMetricsPrometheus.NetCore
{
	class AppMetricsPrometheusMiddleware
	{
		private readonly IMetrics _metrics;
		private readonly RequestDelegate _next;
		private readonly string _metricsPath;
		private readonly IPrometheusFormatter _formatter;

		public AppMetricsPrometheusMiddleware(IMetrics metrics, RequestDelegate next, AppMetricsPrometheusSettings settings, IPrometheusFormatter formatter)
		{
			_metrics = metrics;
			_next = next;
			_metricsPath = settings.MetricsPath ?? "/metrics";
			_formatter = formatter;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == _metricsPath)
			{
				context.Response.StatusCode = 200;
				context.Response.Headers[HeaderNames.ContentType] = "text/plain; version=0.0.4; charset=utf-8";

				var output = _formatter.GetOutput(_metrics.Snapshot.Get());
				await context.Response.WriteAsync(output);
				return;
			}
			else
			{
				await _next(context);
			}
		}
	}
}
