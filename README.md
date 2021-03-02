# appmetrics-prometheus
Prometheus exporter for AppMetrics

### ASP.NET Core

In order to use TemplateBinder with ASP.Net Core you have to install <a href="https://www.nuget.org/packages/TemplateBinder.Extensions.DependencyInjection/" target="_blank">TemplateBinder.Extensions.DependencyInjection</a> nuget package

```csharp
public void Configure(IApplicationBuilder app)
{
  // add this as the first middleware of your project
  app.UseAppMetricsPrometheus();
}
```
