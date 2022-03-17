using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using AppMetricsPrometheus.Collectors;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace AppMetricsPrometheus.NetCore
{
	class AppMetricsPrometheusMiddleware
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IMetrics _metrics;
		private readonly RequestDelegate _next;
		private readonly string _metricsPath;
		private IReadOnlyCollection<Type>? _metricCollectorTypes;
		private readonly IPrometheusFormatter _formatter;
		private readonly ICollector? _systeUsageCollector;

		public AppMetricsPrometheusMiddleware(IServiceProvider serviceProvider, IMetrics metrics, RequestDelegate next, AppMetricsPrometheusSettings settings, IPrometheusFormatter formatter)
		{
			if (settings.MetricCollectorTypes is not null)
				ValidateMetricCollectorTypes(settings.MetricCollectorTypes);

			_serviceProvider = serviceProvider;
			_metrics = metrics;
			_next = next;
			_metricsPath = settings.MetricsPath ?? "/metrics";
			_metricCollectorTypes = settings.MetricCollectorTypes?.ToArray();
			_formatter = formatter;
			_systeUsageCollector = settings.UseSystemUsageCollector ? new SystemUsageCollector(metrics) : null;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == _metricsPath)
			{
				TryCollectMetrics();
				await WriteMetricsResponse(context);
			}
			else
				await _next(context);
		}

		private async Task WriteMetricsResponse(HttpContext context)
		{
			context.Response.StatusCode = 200;
			context.Response.Headers[HeaderNames.ContentType] = "text/plain; version=0.0.4; charset=utf-8";

			var output = _formatter.GetOutput(_metrics.Snapshot.Get());
			await context.Response.WriteAsync(output);
		}

		private void TryCollectMetrics()
		{
			_systeUsageCollector?.Collect();

			if (_metricCollectorTypes is null)
				return;

			foreach (var type in _metricCollectorTypes)
			{
				var collector = (ICollector)(_serviceProvider.GetService(type) ?? throw new Exception($"Metric collector of type {type.FullName} has not been registered in DI"));
				collector.Collect();
			}
		}

		private void ValidateMetricCollectorTypes(IReadOnlyCollection<Type> metricCollectorTypes)
		{
			foreach (var type in metricCollectorTypes)
				if (!typeof(ICollector).IsAssignableFrom(type))
					throw new Exception($"Type {type.FullName} cannot be used as MetricCollectorType in AppMetricsPrometheusMiddleware since it does not implement the ICollector interface");
		}
	}
}
