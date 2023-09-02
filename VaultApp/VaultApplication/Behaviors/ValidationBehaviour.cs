using FluentValidation;
using MediatR;
using VaultDomain.Commands;
using VaultDomain.ValueObjects;

namespace VaultApplication.Behaviors
{
    internal class ValidationBehaviour<TRequest> : IPipelineBehavior<TRequest, Result>
        where TRequest : IDomainCommand<Result>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
        {
            try
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

                    if (failures.Any())
                    {
                        var result = new Result();
                        foreach (var item in failures)
                        {
                            result.WithError(item.ErrorMessage);
                        }
                        return result;
                    }
                }
                return await next();
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}
