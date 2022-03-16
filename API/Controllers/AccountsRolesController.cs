using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsRolesController : BaseController<AccountRole, AccountRoleRepository, int>
    {
        private readonly AccountRoleRepository accountRoleRepository;

        public AccountsRolesController(AccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
        {
            this.accountRoleRepository = accountRoleRepository;
        }

        [HttpPost("signmanager")]
        public ActionResult SignManager(EmailVM account)
        {
            int result = accountRoleRepository.SignManager(account.Email);

            if (result == -1)
            {
                return NotFound("Account Not Found");
            }

            return Ok("Employee Promoted");
        }
    }
}
