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

        event EventHandler TasksUpdated;
        event EventHandler TaskSelected;
        event EventHandler TasksChanged;
        event EventHandler SelectedTaskChanged;
        event EventHandler<string> CreateTaskFailed;

        Task CreateTask(TaskVm model);
    }
}