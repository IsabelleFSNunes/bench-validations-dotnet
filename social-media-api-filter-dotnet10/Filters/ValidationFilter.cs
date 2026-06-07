using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace social_media_api_filter_dotnet10.Filters;

// Intercepta toda action antes da execução e rejeita requisições com ModelState inválido.
// Substitui o comportamento automático do [ApiController], tornando a validação explícita e reutilizável.
public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(
                new ValidationProblemDetails(context.ModelState)
            );
            return;
        }

        await next();
    }
}
