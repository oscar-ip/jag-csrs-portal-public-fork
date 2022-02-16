using Csrs.Interfaces.Dynamics.Models;

namespace Csrs.Api.Services
{
    public interface ITaskService
    {

        Task<bool> CreateTask(string fileId, string subject, string description, CancellationToken cancellationToken);

    }
}
