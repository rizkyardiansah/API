using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;   
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [Authorize(Roles = "Director, Manager")]
        [HttpGet("masteremployees")]
        public ActionResult MasterEmployeeData()
        {
            return Ok(employeeRepository.MasterEmployeeData());
        }

        [HttpGet("TestCORS")]
        public ActionResult TestCors()
        {
            return Ok("Test CORS Berhasil");
        }
    }
}
