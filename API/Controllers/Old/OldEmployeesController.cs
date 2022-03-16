using API.Models;
using API.Repository;
using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OldEmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        public OldEmployeesController(OldEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            if (employeeRepository.Get().Count() == 0)
            {
                return Ok("Employee Table is Empty");
            }
            return Ok(employeeRepository.Get());
        }


        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            Employee employee = employeeRepository.Get(NIK);

            if (employee == null)
            {
                return NotFound("Employee Not Found");
            }

            return Ok(employee);

        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            int result = employeeRepository.Delete(NIK);

            if (result <= 0)
            {
                return BadRequest("Delete Failed");
            }

            return Ok("Succesfully Delete an Employee");
        }


        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            try
            {
                int result = employeeRepository.Insert(employee);

                if (result == -101)
                {
                    return BadRequest("Phone already used");
                }

                if (result == -102)
                {
                    return BadRequest("Email already used");
                }

                if (result <= 0)
                {
                    return BadRequest("Fail to input Employee data");
                }

                return Ok("Employee Data Inserted");
            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        public ActionResult Update(Employee employee)
        {
            try
            {
                int result = employeeRepository.Update(employee);
                if (result == -201)
                {
                    return NotFound("Employee Not Found");
                }

                if (result == -101)
                {
                    return BadRequest("Phone already used");
                }

                if (result == -102)
                {
                    return BadRequest("Email already used");
                }

                if (result <= 0)
                {
                    return BadRequest("Fail to update Employee data");
                }
                return Ok("Update success");
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
    }
}
