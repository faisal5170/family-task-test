using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.DataModels;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
            try
            {
                var task = _mapper.Map<Domain.DataModels.Task>(command);
                var persistedMember = await _taskRepository.CreateRecordAsync(task);

                var vm = _mapper.Map<TaskVm>(persistedMember);
                return new CreateTaskCommandResult()
                {
                    Payload = vm
                };
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            IEnumerable<TaskVm> vm = new List<TaskVm>();

            var tasks = await _taskRepository.GetAllTasks();

            if (tasks != null && tasks.Any())
            {
                tasks.Where(task => task.Member != null).ToList().ForEach(memberTask => memberTask.Member.Tasks = null);
                vm = _mapper.Map<IEnumerable<TaskVm>>(tasks);
            }

            return new GetAllTasksQueryResult()
            {
                Payload = vm
            };
        }

        public async Task<UpdateTaskCommandResult> UpdateTaskCommandHandler(UpdateTaskCommand command)
        {
            var isSucceed = true;
            var task = await _taskRepository.NoTrack().GetTask(command.Id);
            _mapper.Map(command, task);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);
            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new UpdateTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

    }
}
