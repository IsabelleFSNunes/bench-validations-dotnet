using System.ComponentModel.DataAnnotations;

namespace minimal_api_net8_dataannotations.Features.Posts;

// .NET 8: reusable IEndpointFilter that validates any T using DataAnnotations
public class ValidationEndpointFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument is null)
            return Results.BadRequest("Invalid request.");

        var validationContext = new ValidationContext(argument);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(argument, validationContext, validationResults, validateAllProperties: true))
        {
            var errors = validationResults
                .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(r => r.ErrorMessage ?? "Invalid.").ToArray()
                );
            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
