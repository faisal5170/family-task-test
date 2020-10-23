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
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public TaskService(IMapper mapper, ITaskRepository taskRepository, IMemberRepository memberRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _memberRepository = memberRepository;
        }

        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
            try
            {
                var task = _mapper.Map<Domain.DataModels.Task>(command);
                var persistedTask = await _taskRepository.CreateRecordAsync(task);

                var vm = _mapper.Map<TaskVm>(persistedTask);
                if (persistedTask.AssignedToId != null && persistedTask.AssignedToId != Guid.Empty)
                {
                    vm.Member = await _memberRepository.ByIdAsync(persistedTask.AssignedToId.Value);
                    vm.Member.Tasks = null;
                }
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

        public async Task<AssignTaskCommandResult> AssignTaskToMember(Guid taskId, Guid memberId)
        {
            var isSucceed = true;
            var task = await _taskRepository.ByIdAsync(taskId);
            task.AssignedToId = memberId;

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new AssignTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

    }
}
