using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Rest;
using System.Net;

namespace Csrs.Api.Services
{
    public class TaskService : ITaskService
    {

        private readonly IDynamicsClient _dynamicsClient;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            IDynamicsClient dynamicsClient,
            ILogger<TaskService> logger)
        {
            _dynamicsClient = dynamicsClient ?? throw new ArgumentNullException(nameof(dynamicsClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> CreateTask(string fileId, MicrosoftDynamicsCRMtask task, CancellationToken cancellationToken)
        {

            try
            {
                List<string> expand = new List<string> { "owninguser" };
                MicrosoftDynamicsCRMssgCsrsfile file = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(fileId, null, expand: expand, cancellationToken);
                task.RegardingobjectidSsgCsrsfile = file;
                task.Owninguser = file.Owninguser;
            }
            catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Provided fileId not found");
                throw;
            }

            await _dynamicsClient.Tasks.CreateAsync(task);

            return "Success";
        }
    }
}
