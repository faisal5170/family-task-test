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
    public class MembersBase: ComponentBase
    {
        protected List<FamilyMember> members = new List<FamilyMember>();
        protected List<MenuItem> menuItems = new List<MenuItem>();

        protected bool showCreator;
        protected bool isLoaded;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await MemberDataService.GetAllMembers();

            if (result != null && result.Payload != null && result.Payload.Any())
            {
                foreach (var item in result.Payload)
                {
                    members.Add(new FamilyMember()
                    {
                        id = item.Id,
                        firstname = item.FirstName,
                        lastname = item.LastName,
                        avtar = item.Avatar,
                        email = item.Email,
                        role = item.Roles
                    });
                }
            }

            for (int i = 0; i < members.Count; i++)
            {
                menuItems.Add(new MenuItem
                {
                    iconColor = members[i].avtar,
                    label = members[i].firstname,
                    referenceId = members[i].id
                });
            }
            showCreator = true;
            isLoaded = true;
        }

        protected void OnAddItem()
        {
            showCreator = true;
            StateHasChanged();
        }

        protected async Task OnMemberAdd(FamilyMember familyMember)
        {
            var result = await MemberDataService.Create(new Domain.Commands.CreateMemberCommand()
            {
                Avatar = familyMember.avtar,
                FirstName = familyMember.firstname,
                LastName = familyMember.lastname,
                Email = familyMember.email,
                Roles = familyMember.role
            });

            if (result != null && result.Payload != null && result.Payload.Id != Guid.Empty)
            {
                members.Add(new FamilyMember()
                {
                    avtar = result.Payload.Avatar,
                    email = result.Payload.Email,
                    firstname = result.Payload.FirstName,
                    lastname = result.Payload.LastName,
                    role = result.Payload.Roles,
                    id = result.Payload.Id
                });

                menuItems.Add(new MenuItem
                {
                    iconColor = result.Payload.Avatar,
                    label = result.Payload.FirstName,
                    referenceId = result.Payload.Id
                });


                showCreator = false;
                StateHasChanged();
            }
        }

    }
}
