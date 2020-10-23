using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Exentions
{
    public static class FamilyModelExtentions
    {
        public static FamilyMember Map(this MemberVm model)
        {
            if (model == null) return null;
            return new FamilyMember
            {
                id = model.Id,
                avtar = model.Avatar,
                email = model.Email,
                firstname = model.FirstName,
                lastname = model.LastName,
                role = model.Roles
            };
        }
        public static FamilyMember Map(this CreateMemberCommandResult result)
        {
            if (result == null || result.Payload == null) return null;
            var payload = result.Payload;
            return new FamilyMember
            {
                id = payload.Id,
                avtar = payload.Avatar,
                email = payload.Email,
                firstname = payload.FirstName,
                lastname = payload.LastName,
                role = payload.Roles
            };
        }

        public static CreateMemberCommand Map(this FamilyMember member)
        {
            if (member == null) return null;
            return new CreateMemberCommand
            {
                Avatar = member.avtar,
                Email = member.email,
                FirstName = member.firstname,
                LastName = member.lastname,
                Roles = member.role,
            };
        }

    }
}
