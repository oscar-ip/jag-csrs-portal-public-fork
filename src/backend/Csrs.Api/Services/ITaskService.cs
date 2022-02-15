using Csrs.Interfaces.Dynamics.Models;

namespace Csrs.Api.Services
{
    public interface ITaskService
    {

        Task<string> CreateTask(string fileId, MicrosoftDynamicsCRMtask task, CancellationToken cancellationToken);

    }
}
