using System;
using System.Collections.Generic;

namespace AppMetricsPrometheus.NetCore
{
	public class AppMetricsPrometheusSettings
	{
		/// <summary>
		/// The relative url path that metrics will be exposed. Default is /metrics
		/// </summary>
		public string? MetricsPath { get; set; }

		/// <summary>
		/// Type of custom metric collectors in order to collect metrics each time metrics are requested
		/// </summary>
		public IReadOnlyCollection<Type>? MetricCollectorTypes { get; set; }

		/// <summary>
		/// The formatter which will be used to write metrics into the response
		/// </summary>
		public IPrometheusFormatter? Formatter { get; set; }

		/// <summary>
		/// Make use of UseSystemUsageCollector to collect system metrics (like CPU usage, memory, etc)
		/// </summary>
		public bool UseSystemUsageCollector { get; set; }
	}
}
