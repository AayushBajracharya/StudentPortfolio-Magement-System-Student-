using Application.Dto.Teacher;
using Application.Interfaces.Repositories.TeacherRepository;
using FluentValidation;

public class TeacherValidator : AbstractValidator<AddTeacherDto>  // Validate AddTeacherDto
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherValidator(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required");

        RuleFor(x => x.Experience)
            .NotEmpty().WithMessage("Experience is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address")
            .MustAsync(async (model, email, cancellation) => await IsEmailUniqueAsync(model.Id, email))
            .WithMessage("Email already exists");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.DOB)
            .NotEmpty().WithMessage("Date of Birth is required");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required");

        RuleFor(x => x.Phonenumber)
            .NotEmpty().WithMessage("Phone Number is required")
            .Length(10).WithMessage("Phone Number should be exactly 10 digits")
            .Matches(@"^9[78]\d{8}$").WithMessage("Invalid phone format")
            .MustAsync(async (model, phone, cancellation) => await IsPhoneUniqueAsync(model.Id, phone))
            .WithMessage("Phone number already exists");
    }

    private async Task<bool> IsEmailUniqueAsync(int id, string email)
    {
        return !await _teacherRepository.AnyAsync(u => u.Id != id && u.Email == email);
    }

    private async Task<bool> IsPhoneUniqueAsync(int id, string phone)
    {
        return !await _teacherRepository.AnyAsync(u => u.Id != id && u.Phonenumber == phone);
    }
}
