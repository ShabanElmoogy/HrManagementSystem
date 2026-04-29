namespace HrManagementSystem.Contracts.BasicContracts.Authentication.ForgotPassword
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
    {
        private readonly IStringLocalizer<ForgetPasswordRequest> _localizer;
        public ForgetPasswordRequestValidator(IStringLocalizer<ForgetPasswordRequest> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Email)
                .Trimmed()
                .NotEmpty()
                .WithMessage(_localizer[Strings.Required])
                .EmailAddress();
        }
    }
}