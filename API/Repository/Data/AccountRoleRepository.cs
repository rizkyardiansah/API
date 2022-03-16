using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, int>
    {
        private readonly MyContext myContext;

        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public IEnumerable GetAccountRoleName(string email)
        {
            var result = (from e in myContext.Employees
                          join a in myContext.Accounts on e.NIK equals a.NIK
                          join ar in myContext.AccountRoles on a.NIK equals ar.NIK
                          join r in myContext.Roles on ar.RoleId equals r.Id
                          where e.Email == email select r.Name).ToList();

            return result;
        }

        public int SignManager(string email)
        {
            Employee candidate = myContext.Employees.Where(e => e.Email == email).SingleOrDefault();
            if (candidate == null)
            {
                return -1;
            }

            Role manager = myContext.Roles.Where(r => r.Name == "Manager").SingleOrDefault();

            myContext.AccountRoles.Add(new AccountRole { NIK = candidate.NIK, RoleId = manager.Id });
            myContext.SaveChanges();

            return 1;
        }
    }
}
