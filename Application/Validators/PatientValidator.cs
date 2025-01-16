using Application.Dto.Patient;
using Application.Interfaces.Repositories.PatientRepository;
using FluentValidation;

namespace Application.Validators
{
    public class PatientValidator : AbstractValidator<PatientDto>
    {
        private readonly IPatientRepository _patientRepository;

        public PatientValidator(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.EmailAddress)
               .NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Invalid email address")
               .MustAsync(async (model, email, cancellation) => await IsEmailUniqueAsync(model.Id, email))
               .WithMessage("Email already exists");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");

            RuleFor(x => x.PhoneNumber)
               .NotEmpty().WithMessage("Phone Number is required")
               .Length(10).WithMessage("Phone Number should be at least 10 digits")
               .Matches(@"^9[78]\d{8}$").WithMessage("Invalid phone format")
               .MustAsync(async (model, phone, cancellation) => await IsPhoneUniqueAsync(model.Id, phone))
               .WithMessage("Phone number already exists");
        }
        private async Task<bool> IsEmailUniqueAsync(int id, string email)
        {
            return !await _patientRepository.AnyAsync(u => u.Id != id && u.EmailAddress == email);
        }

        private async Task<bool> IsPhoneUniqueAsync(int id, string phone)
        {
            return !await _patientRepository.AnyAsync(u => u.Id != id && u.PhoneNumber == phone);
        }
    }
}
