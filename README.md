# appmetrics-prometheus
Prometheus exporter for AppMetrics

### ASP.NET Core - Setup

In order to use AppMetrics Prometheus exporter with ASP.Net Core you have to install <a href="https://www.nuget.org/packages/AppMetricsPrometheus.NetCore/" target="_blank">AppMetricsPrometheus.NetCore</a> nuget package

#### Simple usage
You can navigate to /metrics (default path) to view your application metrics
```csharp
public void Configure(IApplicationBuilder app)
{
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus();
}
```
#### Custom URL path
```csharp
public void Configure(IApplicationBuilder app)
{
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus(new AppMetricsPrometheusSettings { MetricsPath = "/my-metrics-path" });
}
```
#### SystemUsageCollector - cpu/memory metrics
Make use of SystemUsageCollector to collect and report metrics like CPU and memory usage
```csharp
public void Configure(IApplicationBuilder app)
{
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus(new AppMetricsPrometheusSettings { UseSystemUsageCollector = true });
}
```
#### Custom collectors
You may also create and use your own collectors
```csharp
// create your collector
class MyCollector : ICollector
{
  private readonly IMetrics _metrics;

  public MyCollector(IMetrics metrics)
  {
    _metrics = metrics;
  }

  public void Collect()
  {
    // collect your metrics here and record them in _metrics
  }
}

// give your collector's type to UseAppMetricsPrometheus
public void Configure(IApplicationBuilder app)
{
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus(new AppMetricsPrometheusSettings { MetricCollectorTypes = new[] { typeof(MyCollector) } });
}

// register you collector to DI
public void ConfigureServices(IServiceCollection services)
{
  services.AddSingleton<MyCollector>();
}
```

### Custom Formatters
You can create your own formatter and use it in like the following way
```csharp
public string GetMetrics()
{
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
  
  return output;
} 
  
public void Configure(IApplicationBuilder app)
{
  IPrometheusFormatter formatter = new PrometheusFormatter(
    encoding: Constants.UTF8Encoding,
    newLineFormat: NewLineFormat.Default,
    apdexFormatter: new ApdexFormatter(),
    counterFormatter: new CounterFormatter(),
    gaugeFormatter: new GaugeFormatter(),
    histogramFormatter: new HistogramFormatter(),
    meterFormatter: new MeterFormatter(),
    timerFormatter: new TimerFormatter());
    
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus(new AppMetricsPrometheusSettings { Formatter = formatter });
} 
```
