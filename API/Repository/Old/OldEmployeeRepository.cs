using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class OldEmployeeRepository: IEmployeeRepository
    {

        private readonly MyContext context;

        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string NIK)
        {
            Employee entity = context.Employees
                .Where(e => e.NIK == NIK)
                .FirstOrDefault<Employee>();
            
            if (entity == null)
            {
                return 0;
            }

            context.Remove(entity);
            int result = context.SaveChanges();
            return result;
        }

        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            return context.Employees.Find(NIK);
        }

        public int Insert(Employee employee)
        {
            if (context.Employees.Where(e => e.Phone == employee.Phone).SingleOrDefault<Employee>() != null)
            {
                return -101;
            }

            if (context.Employees.Where(e => e.Email == employee.Email).SingleOrDefault<Employee>() != null)
            {
                return -102;
            }

            string formattedNIK = "";
            string year = DateTime.Now.Year.ToString();
            if (context.Employees.Count<Employee>() == 0)
            {
                formattedNIK = year + "001";
            } else
            {
                string prevNIK = context.Employees.ToList<Employee>().Last<Employee>().NIK;
                int increment = int.Parse(prevNIK.Split("00")[1]);
                increment++;
                formattedNIK = year + "00" + increment;
            }

            employee.NIK = formattedNIK;
            context.Employees.Add(employee);
            int result = context.SaveChanges();
            return result;
        }

        public int Update(Employee employee)
        {
            Employee searchedEmployee = context.Employees.Find(employee.NIK);

            if (searchedEmployee == null)
            {
                return -201;
            }

            if (employee.FirstName != null)
            {
                searchedEmployee.FirstName = employee.FirstName;
            } 
            
            if (employee.LastName != null)
            {
                searchedEmployee.LastName = employee.LastName;
            } 
            
            if (employee.Phone != null)
            {
                if (context.Employees.Where(e => e.Phone == employee.Phone).SingleOrDefault<Employee>() != null)
                {
                    return -101;
                }

                searchedEmployee.Phone = employee.Phone;
            } 
            
            if (employee.BirthDate != null)
            {
                searchedEmployee.BirthDate = employee.BirthDate;
            } 
            
            if (employee.Salary != 0)
            {
                searchedEmployee.Salary = employee.Salary;
            } 
            
            if (employee.Email != null)
            {
                if (context.Employees.Where(e => e.Email == employee.Email).SingleOrDefault<Employee>() != null)
                {
                    return -102;
                }
                searchedEmployee.Email = employee.Email;
            }

            //context.Entry(employee).State = EntityState.Modified;
            int result = context.SaveChanges();
            return result;
        }
    }
}
