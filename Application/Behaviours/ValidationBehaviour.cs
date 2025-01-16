using FluentValidation.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Wrapper;

namespace Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse> where TResponse : ResponseWrapper
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                return failures.Any() ? await ThrowErrors(failures) : await next();
            }
            return await next();
        }

        private static Task<TResponse> ThrowErrors(IEnumerable<ValidationFailure> failures)
        {
            IDictionary<string, string[]> Errors = new Dictionary<string, string[]>();
            var response = new ResponseWrapper();
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Errors.Add(propertyName, propertyFailures);
            }

            //foreach (var failure in failures)
            //{
            //    var propertyName = failure.PropertyName;
            //    var propertyFailures = failure.ErrorMessage;
            //    //Errors.Add(propertyName, propertyFailures); 
            //    Errors.Add(propertyName, propertyFailures);
            //}
            response.Errors = Errors;
            response.IsSuccess = false;
            response.Message = "Validation-error";
            return Task.FromResult(response as TResponse);
        }

    }
}
