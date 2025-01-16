using Application.Dto.Student;
using Application.Interfaces.Repositories.StudentRepository;
using Application.Interfaces.Repositories.TeacherRepository;
using FluentValidation;

namespace Application.Validators
{
    public class StudentValidators : AbstractValidator<AddStudentDto>
    {
        private readonly IStudentRepository _studentRepository;

        public StudentValidators(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(x => x.FatherName)
                .NotEmpty().WithMessage("Father's name is required.")
                .Length(2, 100).WithMessage("Father's name must be between 2 and 100 characters.");

            RuleFor(x => x.MotherName)
                .NotEmpty().WithMessage("Mother's name is required.")
                .Length(2, 100).WithMessage("Mother's name must be between 2 and 100 characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g != null && (g.ToLower() == "male" || g.ToLower() == "female"))
                .WithMessage("Gender must be either 'Male' or 'Female'.");

            RuleFor(x => x.DoB)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required")
               .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
               .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
               .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
               .Matches("[0-9]").WithMessage("Password must contain at least one digit")
               .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.Faculty)
                .NotEmpty().WithMessage("Faculty is required.")
                .Must(f => f != null && (f.ToLower() == "bim" || f.ToLower() == "bca" || f.ToLower() == "bbs"))
                .WithMessage("Faculty must be either 'BIM' or 'BCA' or 'BBS'.");

            RuleFor(x => x.Semester)
                .NotEmpty().WithMessage("Semester is required.")
                .Length(1, 50).WithMessage("Semester must be between 1 and 50 characters.");

            RuleFor(x => x.PhoneNo)
               .Matches(@"^9\d{9}$")
               .WithMessage("Phone number must start with 9 and be 10 digits long.");

          
        }
    }
}

