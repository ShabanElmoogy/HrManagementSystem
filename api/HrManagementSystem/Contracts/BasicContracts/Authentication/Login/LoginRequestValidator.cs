namespace HrManagementSystem.Contracts.BasicContracts.Authentication.Login
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        private readonly IStringLocalizer<LoginRequest> _localizer;
        public LoginRequestValidator(IStringLocalizer<LoginRequest> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UserName)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required]);

            RuleFor(x => x.Password)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required]); ;
        }
    }
}
