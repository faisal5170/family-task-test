using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;

namespace WebClient.Abstractions
{
    public interface ITaskDataService
    {
        IEnumerable<TaskVm> Tasks { get; }
        TaskVm SelectedTask { get; }

        Task CreateTask(TaskVm model);

        public Task<GetAllTasksQueryResult> GetAllTasks();
        public Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command);
    }
}