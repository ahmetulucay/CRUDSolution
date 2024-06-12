using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters;
public class PersonsListActionFilter : IActionFilter
{
    private readonly ILogger<PersonsListActionFilter> _logger;

    public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
    {
        _logger = logger;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //To do: add after logic here
        _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");

        PersonsController personsController = (PersonsController)context.Controller;

        IDictionary<string, object?>? parameters = (IDictionary<string, object?>) 
            context.HttpContext.Items["arguments"]; 

        if (parameters != null)
        {
            if (parameters.ContainsKey("searchBy"))
            {
                personsController.ViewData["searchBy"] = Convert.ToString(parameters["searchBy"]);
            }
            if (parameters.ContainsKey("searchString"))
            {
                personsController.ViewData["searchString"] = Convert.ToString(parameters["searchString"]);
            }
            if (parameters.ContainsKey("sortBy"))
            {
                personsController.ViewData["sortBy"] = Convert.ToString(parameters["sortBy"]);
            }
            if (parameters.ContainsKey("sortOrder"))
            {
                personsController.ViewData["sortOrder"] = Convert.ToString(parameters["sortOrder"]);
            }
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["arguments"] = context.ActionArguments;
        //To do: add before logic here
        _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");

        if (context.ActionArguments.ContainsKey("searchBy"))
        {
            string? searchBy = Convert.ToString
                (context.ActionArguments["searchBy"]);

            //validate the {searchBy} parameter value
            if (!string.IsNullOrEmpty (searchBy))
            {
                var searchByOptions = new List<string>()
                {
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryID),
                    nameof(PersonResponse.Address)
                };

                //reset the {searchBy} parameter value
                if (searchByOptions.Any(temp => temp == searchBy) == false)
                {
                    _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                    
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);

                    _logger.LogInformation("searchBy updated value {searchBy}",
                    context.ActionArguments["searchBy"]);
                }
            }

        }
    }
}
