using FluentValidation;
using FluentValidation.Validators;
using social_media_api_fluentvalidation_dotnet10.DTOs;

namespace social_media_api_fluentvalidation_dotnet10.Validators
{
    public class CreatePostValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostValidator()
        {
            int maxTitleLength = 199;
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The title is required.")
                .MaximumLength(maxTitleLength).WithMessage($"The title is too long. It must not exceed {maxTitleLength} characters.");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Please provide content for the body.");

            RuleFor(x => x.UserId)
                .InclusiveBetween(1, 100).WithMessage("UserId must be between 1 and 100.");

            RuleFor(x => x.Views)
                .InclusiveBetween(0, 10000).WithMessage("Views must be between 0 and 10,000.");

            RuleFor(x => x.Tags)
                .Must(t => t.Count <= 5).WithMessage("Maximum of 5 tags allowed.");


        }
    }
}
