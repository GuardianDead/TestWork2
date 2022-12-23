using FluentValidation;

namespace TestWork2.Data.Validators;

public class FileValidator : AbstractValidator<Models.File>
{
    public FileValidator()
    {
        RuleFor(property => property.Name)
            .NotEmpty().WithMessage("File name cannot be empty")
            .NotNull().WithMessage("File name cannot be null");
        RuleFor(property => property.RowCount)
            .GreaterThanOrEqualTo(1).WithMessage("'Row count' cannot be less then 1")
            .LessThanOrEqualTo(10_000).WithMessage("'Row count' cannot be great then 10 000");
    }
}