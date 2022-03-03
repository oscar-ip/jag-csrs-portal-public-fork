using System;

namespace Csrs.Interfaces
{
    internal class RequestSecurityTokenResponseLifetime
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}