#如何使用

配置：

    Metric.Config.WithHttpEndpoint("http://localhost:1234/").WithAllCounters().WithInternalMetrics();
    config.Filters.Add(new WebApiPerformanceAttribute());

在 Global.cs中配置

    protected void Application_End()
    {
      PerformanceMetricFactory.CleanupPerformanceMetrics();
    }
