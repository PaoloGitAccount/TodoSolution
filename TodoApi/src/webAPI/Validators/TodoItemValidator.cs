using FluentValidation;
using Core.Entities;

namespace WebAPI.Validators
{
    public class TodoItemValidator : AbstractValidator<TodoItem>
    {
        public TodoItemValidator()
        {
            RuleFor(todo => todo.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(todo => todo.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(50).WithMessage("Category must not exceed 50 characters.");

            RuleFor(todo => todo.Priority)
                .InclusiveBetween(1, 5).WithMessage("Priority must be between 1 and 5.");

            RuleFor(todo => todo.DueDate)
                .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future.")
                .When(todo => todo.DueDate.HasValue).WithMessage("Due date must be provided.");

            RuleFor(todo => todo.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
                .When(todo => todo.Latitude.HasValue);

            RuleFor(todo => todo.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
                .When(todo => todo.Longitude.HasValue);
        }
    }
}
