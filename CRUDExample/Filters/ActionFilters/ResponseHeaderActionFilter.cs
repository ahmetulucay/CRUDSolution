﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;
public class ResponseHeaderActionFilter : IActionFilter
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;

    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
    {
        _logger = logger;
    }
    //before
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuting));
    }

    //after
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuted));
    }
}
