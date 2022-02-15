using Csrs.Interfaces.Dynamics.Models;

namespace Csrs.Api.Services
{
    public interface ITaskService
    {

        Task<string> CreateTask(string fileId, string subject, string description, CancellationToken cancellationToken);

    }
}
