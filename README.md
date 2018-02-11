AspNetPerformanceMetrics
========================

AspNetPerformanceMetrics adds the metrics provided by the [Metrics.NET](https://github.com/etishor/Metrics.NET) to ASP.NET MVC 

In Global

    protected void Application_End()
    {
      PerformanceMetricFactory.CleanupPerformanceMetrics();
    }
