using Core.Abstractions.Repositories;
using Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TaskRepository : BaseRepository<Guid, Domain.DataModels.Task, TaskRepository>, ITaskRepository
    {
        public TaskRepository(FamilyTaskContext context) : base(context)
        { }

        ITaskRepository IBaseRepository<Guid, Domain.DataModels.Task, ITaskRepository>.NoTrack()
        {
            return base.NoTrack();
        }

        ITaskRepository IBaseRepository<Guid, Domain.DataModels.Task, ITaskRepository>.Reset()
        {
            return base.Reset();
        }

        public async Task<List<Domain.DataModels.Task>> GetAllTasks()
        {
            var result = await Query.Include(x => x.Member).ToListAsync();
            return result;
        }

        public async Task<Domain.DataModels.Task> GetTask(Guid id)
        {
            var result = await Query.FirstOrDefaultAsync(x=>x.Id == id);
            return result;
        }
    }
}
