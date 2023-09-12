using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using OpenTelemetry.Trace;

namespace MyWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(IndexModel));
    private static readonly ActivitySource RegisteredActivity = new ActivitySource("Example");


    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void Process() 
    {
        log4net.ThreadContext.Properties["trace_id"] = Tracer.CurrentSpan.Context.TraceId;
        log4net.ThreadContext.Properties["span_id"] = Tracer.CurrentSpan.Context.SpanId;
        log4net.ThreadContext.Properties["parent_span_id"] = Tracer.CurrentSpan.ParentSpanId;
        log.Info("Process");
    }

    public void OnGet()
    {
        log4net.ThreadContext.Properties["trace_id"] = Tracer.CurrentSpan.Context.TraceId;
        log4net.ThreadContext.Properties["span_id"] = Tracer.CurrentSpan.Context.SpanId;
        log4net.ThreadContext.Properties["parent_span_id"] = Tracer.CurrentSpan.ParentSpanId;
        log.Info("OnGet");
        Process();
        using (var activity = RegisteredActivity.StartActivity("Child"))
        {
            log4net.ThreadContext.Properties["trace_id"] = Tracer.CurrentSpan.Context.TraceId;
            log4net.ThreadContext.Properties["span_id"] = Tracer.CurrentSpan.Context.SpanId;
            log4net.ThreadContext.Properties["parent_span_id"] = Tracer.CurrentSpan.ParentSpanId;
            log.Info("Child");
            activity?.SetTag("foo", "bar1");
	    activity?.SetStatus(ActivityStatusCode.Ok);
            using (var childactivity = RegisteredActivity.StartActivity("SayHello"))
            {
              log4net.ThreadContext.Properties["trace_id"] = Tracer.CurrentSpan.Context.TraceId;
              log4net.ThreadContext.Properties["span_id"] = Tracer.CurrentSpan.Context.SpanId;
              log4net.ThreadContext.Properties["parent_span_id"] = Tracer.CurrentSpan.ParentSpanId;
              log.Info("SayHello");
              childactivity?.SetTag("operation.value", 1);
              childactivity?.SetStatus(ActivityStatusCode.Ok);
            }
        }
    }
}
