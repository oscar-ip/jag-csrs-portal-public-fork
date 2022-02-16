using Microsoft.Rest;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;

namespace Csrs.Api.Infrastructure
{
    /// <summary>
    /// Destructures <see cref="ValidationException"/> response properties.
    /// </summary>
    public class ValidationExceptionDestructurer : ExceptionDestructurer
    {
        public override Type[] TargetTypes => new[] { typeof(ValidationException) };

        public override void Destructure(
            Exception exception,
            IExceptionPropertiesBag propertiesBag,
            Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
        {
            base.Destructure(exception, propertiesBag, destructureException);

#pragma warning disable CA1062 // Validate arguments of public methods
            var targetException = exception as ValidationException;
            if (targetException is not null)
            {
                propertiesBag.AddProperty(nameof(ValidationException.Rule), targetException.Rule);
                propertiesBag.AddProperty(nameof(ValidationException.Target), targetException.Target);
            }
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }
}
