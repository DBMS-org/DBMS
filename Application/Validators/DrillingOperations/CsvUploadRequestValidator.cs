using FluentValidation;
using Application.DTOs.DrillingOperations;

namespace Application.Validators.DrillingOperations
{
    public class CsvUploadRequestValidator : BaseValidator<CsvUploadRequest>
    {
        public CsvUploadRequestValidator()
        {
            RuleFor(x => x.FileStream)
                .NotNull()
                .WithMessage("File stream is required")
                .Must(stream => stream != Stream.Null && stream.Length > 0)
                .WithMessage("File cannot be empty");

            RuleFor(x => x.FileName)
                .RequiredString("File name", 1, 255)
                .Must(fileName => fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                .WithMessage("File must be a CSV file");

            RuleFor(x => x.FileSize)
                .GreaterThan(0)
                .WithMessage("File size must be greater than 0")
                .LessThanOrEqualTo(50 * 1024 * 1024) // 50MB limit
                .WithMessage("File size cannot exceed 50MB");
        }
    }
} 