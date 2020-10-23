using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebClient.Abstractions;
using Microsoft.AspNetCore.Components;
using Domain.ViewModel;
using Core.Extensions.ModelConversion;

namespace WebClient.Services
{
    public class TaskDataService : ITaskDataService
    {

        private readonly HttpClient _httpClient;
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            tasks = new List<TaskVm>();
            LoadTasks();
        }
        private IEnumerable<TaskVm> tasks;

        public IEnumerable<TaskVm> Tasks => tasks;

        public TaskVm SelectedTask { get; private set; }

        public event EventHandler TasksChanged;
        public event EventHandler SelectedTaskChanged;
        public event EventHandler<string> CreateTaskFailed;

        private async void LoadTasks()
        {
            tasks = (await GetAllTasks()).Payload;
            TasksChanged?.Invoke(this, null);
        }

        public async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await _httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }

        public async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await _httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }

        public async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateTaskCommandResult>("tasks/" + command.Id, command);
        }

        public async Task<CreateTaskCommandResult> CreateTask(TaskVm model)
        {
            var result = await Create(model.ToCreateTaskCommand());
            return result;
        }

        public async Task AssignTaskToMember(string taskId, string memberId)
        {
            var result = await _httpClient.PostJsonAsync<AssignTaskCommandResult>($"tasks/{taskId}/assign/{memberId}", null);
        }
    }
}