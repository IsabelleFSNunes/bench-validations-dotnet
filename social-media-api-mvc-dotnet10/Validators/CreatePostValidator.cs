using FluentValidation;
using FluentValidation.Validators;
using social_media_api_fluentvalidation_dotnet10.DTOs;

namespace social_media_api_fluentvalidation_dotnet10.Validators
{
    public class CreatePostValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostValidator()
        {
            int maxCaracteresTitle = 199;
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Titulo é obrigatório")
                .MaximumLength(maxCaracteresTitle).WithMessage($"Título não pode exceder {maxCaracteresTitle} caracteres");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Conteúdo é obrigatório");

            RuleFor(x => x.UserId)
                .InclusiveBetween(1, 100).WithMessage("UserId deve estar entre 1 e 100");

            RuleFor(x => x.Views)
                .InclusiveBetween(0, 10000).WithMessage("Views deve estar entre 0 e 10000");

            RuleFor(x => x.Tags)
                .Must(t => t.Count <= 5).WithMessage("Máximo de 5 tags permitidas");


        }
    }
}
