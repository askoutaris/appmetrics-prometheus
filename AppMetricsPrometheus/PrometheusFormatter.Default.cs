using System;
using System.IO;
using System.Text;
using App.Metrics;
using AppMetricsPrometheus.Formatters;

namespace AppMetricsPrometheus
{
	public class PrometheusFormatter : IPrometheusFormatter
	{
		private readonly Encoding _encoding;
		private readonly NewLineFormat _newLineFormat;
		private readonly IApdexFormatter _apdexFormatter;
		private readonly ICounterFormatter _counterFormatter;
		private readonly IGaugeFormatter _gaugeFormatter;
		private readonly IHistogramFormatter _histogramFormatter;
		private readonly IMeterFormatter _meterFormatter;
		private readonly ITimerFormatter _timerFormatter;

		public PrometheusFormatter(
			Encoding encoding,
			NewLineFormat newLineFormat,
			IApdexFormatter apdexFormatter,
			ICounterFormatter counterFormatter,
			IGaugeFormatter gaugeFormatter,
			IHistogramFormatter histogramFormatter,
			IMeterFormatter meterFormatter,
			ITimerFormatter timerFormatter)
		{
			_encoding = encoding;
			_newLineFormat = newLineFormat;
			_apdexFormatter = apdexFormatter;
			_counterFormatter = counterFormatter;
			_gaugeFormatter = gaugeFormatter;
			_histogramFormatter = histogramFormatter;
			_meterFormatter = meterFormatter;
			_timerFormatter = timerFormatter;
		}

		public string GetOutput(MetricsDataValueSource snapshot)
		{
			using (var stream = new MemoryStream())
			{
				using (var streamWriter = new StreamWriter(stream, _encoding, bufferSize: 1024, leaveOpen: true) { NewLine = GetNewLineChar(_newLineFormat) })
				{
					foreach (var metricContext in snapshot.Contexts)
					{
						foreach (var metric in metricContext.ApdexScores)
							_apdexFormatter.Write(streamWriter, metricContext, metric);

						foreach (var metric in metricContext.Counters)
							_counterFormatter.Write(streamWriter, metricContext, metric);

						foreach (var metric in metricContext.Gauges)
							_gaugeFormatter.Write(streamWriter, metricContext, metric);

						foreach (var metric in metricContext.Histograms)
							_histogramFormatter.Write(streamWriter, metricContext, metric);

						foreach (var metric in metricContext.Meters)
							_meterFormatter.Write(streamWriter, metricContext, metric);

						foreach (var metric in metricContext.Timers)
							_timerFormatter.Write(streamWriter, metricContext, metric);
					}

					streamWriter.Flush();
				}

				return _encoding.GetString(stream.ToArray());
			}
		}

		private string GetNewLineChar(NewLineFormat newLine)
		{
			return newLine switch
			{
				NewLineFormat.Auto => Environment.NewLine,
				NewLineFormat.Windows => "\r\n",
				NewLineFormat.Unix or NewLineFormat.Default => "\n",
				_ => throw new ArgumentOutOfRangeException(nameof(newLine), newLine, null),
			};
		}
	}
}
