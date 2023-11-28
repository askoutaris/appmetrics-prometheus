# appmetrics-prometheus
Prometheus exporter for AppMetrics

### ASP.NET Core - Setup

In order to use AppMetrics Prometheus exporter with ASP.Net Core you have to install <a href="https://www.nuget.org/packages/AppMetricsPrometheus.NetCore/" target="_blank">AppMetricsPrometheus.NetCore</a> nuget package

```csharp
public void Configure(IApplicationBuilder app)
{
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus();
}
```

You can navigate to /metrics (default path) to view your application metrics

### Custom Formatters

You can create your own formatter and use it in like the following way

```csharp
  IPrometheusFormatter formatter = new PrometheusFormatter(
    encoding: Constants.UTF8Encoding,
    newLineFormat: NewLineFormat.Default,
    apdexFormatter: new ApdexFormatter(),
    counterFormatter: new CounterFormatter(),
    gaugeFormatter: new GaugeFormatter(),
    histogramFormatter: new HistogramFormatter(),
    meterFormatter: new MeterFormatter(),
    timerFormatter: new TimerFormatter());
        
  IMetrics metrics = GetMetrics();
   
  var output = formatter.GetOutput(_metrics.Snapshot.Get());
```
