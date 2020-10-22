using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface ITaskRepository : IBaseRepository<Guid, Domain.DataModels.Task, ITaskRepository>
    {
        Task<List<Domain.DataModels.Task>> GetAllTasks();
        Task<Domain.DataModels.Task> GetTask(Guid id);
    }
}
