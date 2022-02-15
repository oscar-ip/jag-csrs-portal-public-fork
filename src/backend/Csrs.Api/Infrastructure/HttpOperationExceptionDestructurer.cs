using Microsoft.Rest;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;

namespace Csrs.Api.Infrastructure
{
    /// <summary>
    /// Destructures <see cref="HttpOperationException"/> response properties.
    /// </summary>
    public class HttpOperationExceptionDestructurer : ExceptionDestructurer
    {
        public override Type[] TargetTypes => new[] { typeof(HttpOperationException) };

        public override void Destructure(
            Exception exception,
            IExceptionPropertiesBag propertiesBag,
            Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
        {
            base.Destructure(exception, propertiesBag, destructureException);

#pragma warning disable CA1062 // Validate arguments of public methods
            var targetException = exception as HttpOperationException;
            if (targetException is not null)
            {
                propertiesBag.AddProperty(nameof(HttpOperationException.Response.StatusCode), targetException.Response.StatusCode);
                propertiesBag.AddProperty(nameof(HttpOperationException.Response.ReasonPhrase), targetException.Response.ReasonPhrase);
                propertiesBag.AddProperty(nameof(HttpOperationException.Response.Content), targetException.Response.Content);
            }
#pragma warning restore CA1062 // Validate arguments of public methods
        }
    }
}
