using Microsoft.Rest;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;

namespace Csrs.Api.Infrastructure
{
    /// <summary>
    /// Destructures <see cref="ValidationException"/> response properties.
    /// Warning this could write out PII from the response content.
    /// </summary>
    public class SerializationExceptionDestructurer : ExceptionDestructurer
    {
        public override Type[] TargetTypes => new[] { typeof(SerializationException) };

        public override void Destructure(
            Exception exception,
            IExceptionPropertiesBag propertiesBag,
            Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
        {
            base.Destructure(exception, propertiesBag, destructureException);

#pragma warning disable CA1062 // Validate arguments of public methods
            var targetException = exception as SerializationException;
            if (targetException is not null)
            {
                // warning: response could have PII
                propertiesBag.AddProperty(nameof(SerializationException.Content), targetException.Content);
            }
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }
}
