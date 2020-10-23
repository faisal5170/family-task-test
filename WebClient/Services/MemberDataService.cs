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
using WebClient.Exentions;

namespace WebClient.Services
{
    public class MemberDataService : IMemberDataService
    {
        private readonly HttpClient _httpClient;
        public MemberDataService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            members = new List<MemberVm>();
            LoadMembers();
        }
        private IEnumerable<MemberVm> members;

        public IEnumerable<MemberVm> Members => members;

        public MemberVm SelectedMember { get; private set; }

        public event EventHandler MembersChanged;
        public event EventHandler<string> UpdateMemberFailed;
        public event EventHandler<string> CreateMemberFailed;
        public event EventHandler SelectedMemberChanged;

        private async void LoadMembers()
        {
            members = (await GetAllMembers()).Payload;
            MembersChanged?.Invoke(this, null);
        }

        public async Task<CreateMemberCommandResult> Create(CreateMemberCommand command)
        {
            return await _httpClient.PostJsonAsync<CreateMemberCommandResult>("members", command);
        }

        public async Task<GetAllMembersQueryResult> GetAllMembers()
        {
            return await _httpClient.GetJsonAsync<GetAllMembersQueryResult>("members");
        }

        public async Task<UpdateMemberCommandResult> Update(UpdateMemberCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateMemberCommandResult>($"members/{command.Id}", command);
        }

        public void SelectMember(Guid id)
        {
            if (members.All(memberVm => memberVm.Id != id)) return;
            {
                SelectedMember = members.SingleOrDefault(memberVm => memberVm.Id == id);
                SelectedMemberChanged?.Invoke(this, null);
            }
        }

        public void SelectNullMember()
        {
            SelectedMember = null;
            SelectedMemberChanged?.Invoke(this, null);
        }

        public async Task<FamilyMember[]> GetAllMembersList()
        {
            var members = await _httpClient.GetJsonAsync<GetAllMembersQueryResult>("members");
            return members.Payload.Select(u => u.Map()).ToArray();
        }        
    }
}
