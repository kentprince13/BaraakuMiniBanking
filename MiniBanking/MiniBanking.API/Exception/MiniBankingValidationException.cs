using FluentValidation.Results;
using MiniBanking.Domain.Exception;

namespace MiniBanking.API.Exception;

public class MiniBankingValidationException:MiniBankingDomainException
{
    public MiniBankingValidationException()
            : base("One or more validation failures have occurred.", "Invalid Request")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public MiniBankingValidationException(List<ValidationFailure> failures)
            : this()
        {
            //var message = string.Empty;
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();
                Failures.Add(propertyName, propertyFailures);
            }

        }
        public IDictionary<string, string[]> Failures { get; set; }
}