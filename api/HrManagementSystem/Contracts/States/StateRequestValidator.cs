namespace HrManagementSystem.Contracts.State;

public class StateRequestValidator : AbstractValidator<StateRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<StateRequest> _localizer;

    public StateRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<StateRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(s => s.NameEn)
            .Trimmed()
            .NotEmpty()
            .WithName(Strings.NameEn)
            .WithMessage(_localizer[Strings.Required])
            .Length(2, 100)
            .WithMessage(_localizer[Strings.MaxLengthError])
            .Matches(@"^[A-Za-z\s]+$")
            .WithMessage(_localizer[Strings.EnglishLetterOnly]);

        RuleFor(s => s.NameAr)
            .Trimmed()
            .NotEmpty()
            .WithName(Strings.NameAr)
            .WithMessage(_localizer[Strings.Required])
            .Length(2, 100)
            .WithMessage(_localizer[Strings.MaxLengthError])
            .Matches(@"^[\p{IsArabic}\s]+$")
            .WithMessage(_localizer[Strings.ArabicLetterOnly]);

        RuleFor(s => s.Code)
            .Trimmed()
            .NotEmpty()
            .WithName(Strings.Code)
            .WithMessage(_localizer[Strings.Required])
            .Length(2, 10)
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(s => s.CountryId)
            .GreaterThan(0)
            .WithName(Strings.Country)
            .WithMessage(_localizer[Strings.Required]);

        RuleFor(s => s)
           .Must(s => !IsStateNameEnDuplicated(s))
           .WithName(Strings.NameEn)
           .WithMessage(_localizer[Strings.DuplicatedValue]);

        RuleFor(s => s)
           .Must(s => !IsStateNameArDuplicated(s))
           .WithName(Strings.NameAr)
           .WithMessage(_localizer[Strings.DuplicatedValue]);

        RuleFor(s => s)
           .Must(s => !IsCodeDuplicated(s))
           .WithName(Strings.Code)
           .WithMessage(_localizer[Strings.DuplicatedValue]);

        RuleFor(s => s)
           .Must(s => ISCountryExists(s.CountryId))
           .WithName(Strings.Country)
           .WithMessage(_localizer[Strings.CountryNotFound]);
    }

    private bool IsStateNameEnDuplicated(StateRequest state)
    {
        return _dbContext.States.Any(s => s.NameEn == state.NameEn && s.CountryId == state.CountryId && s.Id != state.Id);
    }

    private bool IsStateNameArDuplicated(StateRequest state)
    {
        return _dbContext.States.Any(s => s.NameAr == state.NameAr && s.CountryId == state.CountryId && s.Id != state.Id);
    }

    private bool IsCodeDuplicated(StateRequest state)
    {
        return _dbContext.States.Any(s => s.Code == state.Code && s.CountryId == state.CountryId && s.Id != state.Id);
    }

    private bool ISCountryExists(int countryId)
    {
        return _dbContext.Countries.Any(c => c.Id == countryId && !c.IsDeleted);
    }
}