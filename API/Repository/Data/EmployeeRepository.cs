using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }

        public IEnumerable MasterEmployeeData()
        {
            var masterEmployee = (
                from e in context.Employees
                join a in context.Accounts on e.NIK equals a.NIK
                join p in context.Profilings on e.NIK equals p.NIK
                join ed in context.Educations on p.EducationId equals ed.Id
                join u in context.Universities on ed.UniversityId equals u.Id
                select new
                {
                    e.NIK,
                    FullName = $"{e.FirstName} {e.LastName}",
                    e.Phone,
                    Gender = e.Gender.ToString(),
                    e.Email,
                    e.BirthDate,
                    e.Salary,
                    Educations_Id = p.EducationId,
                    ed.GPA,
                    ed.Degree,
                    UniversityName = u.Name
                }).ToList();

            return masterEmployee;
        }

        public MasterEmployeeVM MasterEmployeeData(string nik)
        {
            MasterEmployeeVM masterEmployee = (
                from e in context.Employees
                join a in context.Accounts on e.NIK equals a.NIK
                join p in context.Profilings on e.NIK equals p.NIK
                join ed in context.Educations on p.EducationId equals ed.Id
                join u in context.Universities on ed.UniversityId equals u.Id
                select new MasterEmployeeVM
                {
                    NIK= e.NIK,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Phone = e.Phone,
                    Gender = e.Gender.ToString(),
                    Email = e.Email,
                    BirthDate = e.BirthDate,
                    Salary = e.Salary,
                    Educations_Id = p.EducationId,
                    GPA = ed.GPA,
                    Degree = ed.Degree,
                    UniversityName = u.Name
                }).SingleOrDefault<MasterEmployeeVM>(j => j.NIK == nik);

            return masterEmployee;
        }


    }
}
