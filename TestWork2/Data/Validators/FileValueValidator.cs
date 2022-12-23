using FluentValidation;
using TestWork2.Data.Models;
using File = TestWork2.Data.Models.File;

namespace TestWork2.Data.Validators;

public class FileValueValidator : AbstractValidator<FileValue>
{
    private readonly IValidator<File> _fileValidator;

    public FileValueValidator(IValidator<File> fileValidator)
    {
        _fileValidator = fileValidator;
    }
    
    public FileValueValidator()
    {
        RuleFor(property => property.DateTime)
            .GreaterThanOrEqualTo(new DateTime(year: 2000, month: 1, day: 1))
            .WithMessage(@"'Date time' cannot be less then '01.01.2000'")
            .LessThanOrEqualTo(DateTime.Now).WithMessage(@"'Date time' cannot be great then current time");
        RuleFor(property => property.Seconds)
            .GreaterThanOrEqualTo(0).WithMessage("'Seconds' cannot be less then 0");
        RuleFor(property => property.Indicator)
            .GreaterThanOrEqualTo(0).WithMessage("'Indicator' cannot be less then 0");
        
        RuleFor(property => property.File).SetValidator(_fileValidator);
    }
}