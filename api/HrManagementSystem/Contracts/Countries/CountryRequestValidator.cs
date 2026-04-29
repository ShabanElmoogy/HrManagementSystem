namespace HrManagementSystem.Contracts.Countries;

public class CountryRequestValidator : AbstractValidator<CountryRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IStringLocalizer<CountryRequest> _localizer;

    public CountryRequestValidator(ApplicationDbContext dbContext, IStringLocalizer<CountryRequest> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;

        RuleFor(c => c.NameEn)
            .Trimmed()
            .NotEmpty()
            .WithName(Strings.NameEn)
            .WithMessage(_localizer[Strings.Required])
            .Length(2, 100)
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(c => c.NameAr)
            .Trimmed()
            .NotEmpty()
            .WithName(Strings.NameAr)
            .WithMessage(_localizer[Strings.Required])
            .Length(2, 100)
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(c => c.Alpha2Code)
            .Length(2, 2)
            .When(c => !string.IsNullOrEmpty(c.Alpha2Code))
            .Trimmed()
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(c => c.Alpha3Code)
            .Length(3, 3)
            .When(c => !string.IsNullOrEmpty(c.Alpha3Code))
            .Trimmed()
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(c => c.PhoneCode)
            .Length(1, 10)
            .When(c => !string.IsNullOrEmpty(c.PhoneCode))
            .Trimmed()
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(c => c.CurrencyCode)
            .Length(3)
            .When(c => !string.IsNullOrEmpty(c.CurrencyCode))
            .Trimmed()
            .WithMessage(_localizer[Strings.MaxLengthError]);

        RuleFor(c => c)
           .Must(c => !IsCountryNameEnDuplicated(c))
           .WithName(Strings.NameEn)
           .WithMessage(_localizer[Strings.DuplicatedValue]);

        RuleFor(c => c)
           .Must(c => !IsCountryNameArDuplicated(c))
           .WithName(Strings.NameAr)
           .WithMessage(_localizer[Strings.DuplicatedValue]);

        RuleFor(c => c)
           .Must(c => !IsAlpha2CodeDuplicated(c))
           .WithName(Strings.Alpha2Code)
           .WithMessage(_localizer[Strings.DuplicatedValue]);

        RuleFor(c => c)
           .Must(c => !IsAlpha3CodeDuplicated(c))
           .WithName(Strings.Alpha3Code)
           .WithMessage(_localizer[Strings.DuplicatedValue]);
    }

    private bool IsCountryNameEnDuplicated(CountryRequest country)
    {
        return _dbContext.Countries.Any(c => c.NameEn == country.NameEn && c.Id != country.Id);
    }

    private bool IsCountryNameArDuplicated(CountryRequest country)
    {
        return _dbContext.Countries.Any(c => c.NameAr == country.NameAr && c.Id != country.Id);
    }

    private bool IsAlpha2CodeDuplicated(CountryRequest country)
    {
        return _dbContext.Countries.Any(c => c.Alpha2Code == country.Alpha2Code && c.Id != country.Id);
    }

    private bool IsAlpha3CodeDuplicated(CountryRequest country)
    {
        return _dbContext.Countries.Any(c => c.Alpha3Code == country.Alpha3Code && c.Id != country.Id);
    }
}
