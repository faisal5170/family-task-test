using Domain.ClientSideModels;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class TasksBase : ComponentBase
    {       
        protected List<TaskVm> tasks = new List<TaskVm>();
        protected List<TaskModel> leftMenuItem = new List<TaskModel>();

        [Inject]
        public ITaskDataService TaskDataService { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            //UpdateTasks();
            //TaskDataService.TasksChanged += TaskDataService_TasksChanged;
        }

        private void TaskDataService_TasksChanged(object sender, EventArgs e)
        {
            UpdateTasks();

            StateHasChanged();
        }

        void UpdateTasks()
        {
            var result = TaskDataService.Tasks;

            if (result.Any())
            {
                tasks = result.ToList();
            }
        }
    }
}
