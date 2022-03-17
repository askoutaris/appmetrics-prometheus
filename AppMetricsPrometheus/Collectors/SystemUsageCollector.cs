using System;
using System.Diagnostics;
using App.Metrics;
using App.Metrics.Gauge;

namespace AppMetricsPrometheus.Collectors
{
	public class SystemUsageCollector : ICollector
	{
		private readonly IMetrics _metrics;
		private DateTime _lastTimeStamp;
		private TimeSpan _lastTotalProcessorTime = TimeSpan.Zero;
		private TimeSpan _lastUserProcessorTime = TimeSpan.Zero;
		private TimeSpan _lastPrivilegedProcessorTime = TimeSpan.Zero;

		private readonly GaugeOptions _totalCpuUsed;
		private readonly GaugeOptions _privilegedCpuUsed;
		private readonly GaugeOptions _userCpuUsed;
		private readonly GaugeOptions _memoryWorkingSet;
		private readonly GaugeOptions _nonPagedSystemMemory;
		private readonly GaugeOptions _pagedMemory;
		private readonly GaugeOptions _pagedSystemMemory;
		private readonly GaugeOptions _privateMemory;
		private readonly GaugeOptions _virtualMemory;

		public SystemUsageCollector(IMetrics metrics, string context = "System")
		{
			_metrics = metrics;
			_lastTimeStamp = Process.GetCurrentProcess().StartTime;

			_totalCpuUsed = new GaugeOptions
			{
				Context = context,
				Name = "Total CPU Percentage Used",
				MeasurementUnit = Unit.Percent
			};
			_privilegedCpuUsed = new GaugeOptions
			{
				Context = context,
				Name = "Privileged CPU Percentage Used",
				MeasurementUnit = Unit.Percent
			};
			_userCpuUsed = new GaugeOptions
			{
				Context = context,
				Name = "User CPU Percentage Used",
				MeasurementUnit = Unit.Percent
			};
			_memoryWorkingSet = new GaugeOptions
			{
				Context = context,
				Name = "Memory Working Set",
				MeasurementUnit = Unit.Bytes
			};
			_nonPagedSystemMemory = new GaugeOptions
			{
				Context = context,
				Name = "Non Paged System Memory",
				MeasurementUnit = Unit.Bytes
			};
			_pagedMemory = new GaugeOptions
			{
				Context = context,
				Name = "Paged Memory",
				MeasurementUnit = Unit.Bytes
			};
			_pagedSystemMemory = new GaugeOptions
			{
				Context = context,
				Name = "Paged System Memory",
				MeasurementUnit = Unit.Bytes
			};
			_privateMemory = new GaugeOptions
			{
				Context = context,
				Name = "Private Memory",
				MeasurementUnit = Unit.Bytes
			};
			_virtualMemory = new GaugeOptions
			{
				Context = context,
				Name = "Virtual Memory",
				MeasurementUnit = Unit.Bytes
			};
		}

		public void Collect()
		{
			var process = Process.GetCurrentProcess();

			double num2 = process.PrivilegedProcessorTime.TotalMilliseconds - _lastPrivilegedProcessorTime.TotalMilliseconds;
			double num3 = process.UserProcessorTime.TotalMilliseconds - _lastUserProcessorTime.TotalMilliseconds;
			double num4 = (DateTime.UtcNow - _lastTimeStamp).TotalMilliseconds * Environment.ProcessorCount;

			var cpuUsedMs = (process.TotalProcessorTime - _lastTotalProcessorTime).TotalMilliseconds;
			var totalMsPassed = (DateTime.UtcNow - _lastTimeStamp).TotalMilliseconds;
			var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

			_lastTotalProcessorTime = process.TotalProcessorTime;
			_lastPrivilegedProcessorTime = process.PrivilegedProcessorTime;
			_lastUserProcessorTime = process.UserProcessorTime;
			_lastTimeStamp = DateTime.UtcNow;

			_metrics.Measure.Gauge.SetValue(_totalCpuUsed, cpuUsageTotal * 100);
			_metrics.Measure.Gauge.SetValue(_privilegedCpuUsed, num2 * 100.0 / num4);
			_metrics.Measure.Gauge.SetValue(_userCpuUsed, num3 * 100.0 / num4);
			_metrics.Measure.Gauge.SetValue(_memoryWorkingSet, process.WorkingSet64);
			_metrics.Measure.Gauge.SetValue(_nonPagedSystemMemory, process.NonpagedSystemMemorySize64);
			_metrics.Measure.Gauge.SetValue(_pagedMemory, process.PagedMemorySize64);
			_metrics.Measure.Gauge.SetValue(_pagedSystemMemory, process.PagedSystemMemorySize64);
			_metrics.Measure.Gauge.SetValue(_privateMemory, process.PrivateMemorySize64);
			_metrics.Measure.Gauge.SetValue(_virtualMemory, process.VirtualMemorySize64);
		}
	}
}
