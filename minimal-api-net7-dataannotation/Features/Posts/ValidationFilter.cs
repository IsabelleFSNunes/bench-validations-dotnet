using System.ComponentModel.DataAnnotations;

namespace minimal_api_net7_dataannotations.Features.Posts;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext ctx,
        EndpointFilterDelegate next)
    {
        var arg = ctx.GetArgument<T>(0);

        if (arg is null)
            return Results.BadRequest("Invalid request.");

        var validationContext = new ValidationContext(arg);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(arg, validationContext, validationResults, validateAllProperties: true))
        {
            var errors = validationResults
                .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(r => r.ErrorMessage ?? "Invalid.").ToArray()
                );
            return Results.ValidationProblem(errors);
        }

        return await next(ctx);
    }
}
