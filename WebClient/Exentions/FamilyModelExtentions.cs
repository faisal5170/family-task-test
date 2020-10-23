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
        public static FamilyMember ToFamilyMember(this MemberVm model)
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
    }
}
