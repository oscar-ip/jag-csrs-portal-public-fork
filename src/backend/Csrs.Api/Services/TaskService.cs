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

        public async Task<string> CreateTask(string fileId, string subject, string description, CancellationToken cancellationToken)
        {

            MicrosoftDynamicsCRMtask task = new MicrosoftDynamicsCRMtask();
            task.Subject = subject;
            task.Description = description;

            try
            {

                MicrosoftDynamicsCRMssgCsrsfile file = await _dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(fileId, null, null, cancellationToken);

                if (file._owninguserValue is not null)
                {
                    task.OwninguserTaskODataBind = _dynamicsClient.GetEntityURI("systemusers", file._owninguserValue);
                }
                else if (file._owningteamValue is not null)
                {
                    task.OwningteamTaskODataBind = _dynamicsClient.GetEntityURI("teams", file._owningteamValue);
                }

                task.RegardingobjectidSsgCsrsfileODataBind = _dynamicsClient.GetEntityURI("ssg_csrsfiles", file.SsgCsrsfileid);

            }
            catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                
                _logger.LogError("Provided fileId not found");
                throw;

            }

            task.Prioritycode = 1;
            task.Statuscode = 2;
            task.Isregularactivity = true;
            task.Activitytypecode = "task";
            //Set due date. If there is an issue set the timestamp to now?
            task.Scheduledend = DateTimeOffset.UtcNow;

            await _dynamicsClient.Tasks.CreateAsync(task);
           
            return "Success";
        }
    }
}
