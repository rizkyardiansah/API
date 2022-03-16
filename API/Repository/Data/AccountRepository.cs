using API.Context;
using API.Models;
using API.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;

        public AccountRepository(MyContext myContext) : base(myContext) {
            this.myContext = myContext;
        }

        public int Register(RegisterVM registerVM)
        {
            
            //check is Phone Unique
            if (myContext.Employees.SingleOrDefault(e => e.Phone == registerVM.Phone) != null)
            {
                return -1;
            }

            //check is Email Unique
            if (myContext.Employees.SingleOrDefault(e => e.Email == registerVM.Email) != null)
            {
                return -2;
            }

            //check is gender valid
            if (registerVM.Gender != "Male" && registerVM.Gender != "Female")
            {
                return -3;
            }

            //convert gender string to enum
            int gender = (registerVM.Gender == "Male") ? 0 : 1;

            //generate nik
            int increment = 1;
            string year = DateTime.Now.Year.ToString();
            if (myContext.Employees.Count<Employee>() > 0)
            {
                string prevNIK = myContext.Employees.OrderBy(e => e.NIK).LastOrDefault().NIK.Substring(4);
                increment = int.Parse(prevNIK) + 1;
            }
            string NIK = year + increment.ToString().PadLeft(3, '0');


            Employee emp = new Employee
            {
                NIK = NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Gender)gender,
            };
            
            Account acc = new Account
            {
                NIK = NIK,
                Password = HashPassword(registerVM.Password, GenerateSalt(12)),
            };

            Education educ = new Education
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversityId = registerVM.UniversityId
            };

            myContext.Employees.Add(emp);
            myContext.Accounts.Add(acc);
            myContext.Educations.Add(educ);
            myContext.SaveChanges();

            AccountRole accRole = new AccountRole
            {
                NIK = NIK,
                RoleId = myContext.Roles.Where(r => r.Name == "Employee").SingleOrDefault().Id
            };

            Profiling prof = new Profiling
            {
                NIK = NIK,
                EducationId = educ.Id,
            };

            myContext.AccountRoles.Add(accRole);
            myContext.Profilings.Add(prof);
            myContext.SaveChanges();

            return 1;
        }

        public int Login(LoginVM loginVM)
        {
            //check is email exist
            Employee emp = myContext.Employees.SingleOrDefault(e => e.Email == loginVM.Email);
            if (emp == null)
            {
                return -1; //Account not found
            }
            Account acc = myContext.Accounts.SingleOrDefault(a => a.NIK == emp.NIK);

            if (!ValidatePassword(loginVM.Password, acc.Password))
            {
                return -2; //wrong passsword
            }

            return 1; //Login success
        }

        public int ForgotPassword(EmailVM forgotPasswordVM)
        {
            Employee emp = myContext.Employees.SingleOrDefault(e => e.Email == forgotPasswordVM.Email);
            if (emp == null)
            {
                return -1; //email not found
            }
            Account acc = myContext.Accounts.SingleOrDefault(a => a.NIK == emp.NIK);

            acc.OTP = new Random().Next(100000, 1000000);
            acc.ExpiredToken = DateTime.Now.AddMinutes(5);
            acc.IsUsed = false;
            myContext.SaveChanges();
            SendEmail(forgotPasswordVM.Email, acc.OTP, acc.ExpiredToken);
            return 1;
        }

        public int ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var data = myContext.Employees.Join(
                myContext.Accounts,
                e => e.NIK,
                a => a.NIK,
                (e, a) => new
                {
                    e.NIK,
                    e.Email,
                    a.Password,
                    a.OTP,
                    a.ExpiredToken,
                    a.IsUsed
                }).Where(e => e.Email == changePasswordVM.Email).SingleOrDefault();

            if (data == null)
            {
                return -1; // Account not Found
            }

            if (changePasswordVM.NewPassword != changePasswordVM.ConfirmPassword)
            {
                return -2; // new password and confirm password not match
            }

            if (changePasswordVM.OTP != data.OTP)
            {
                return -3; //otp not match
            }

            if (DateTime.Now > data.ExpiredToken)
            {
                return -4; // otp expired
            }

            if (data.IsUsed == true)
            {
                return -5; // otp already used
            }

            myContext.Accounts.SingleOrDefault(a => a.NIK == data.NIK).IsUsed = true;
            myContext.Accounts.SingleOrDefault(a => a.NIK == data.NIK).Password = HashPassword(changePasswordVM.NewPassword, GenerateSalt(12));
            myContext.SaveChanges();

            return 1;
        }

        private void SendEmail(string email, int otp, DateTime expired)
        {
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Muhammad Rizky Ardiansah", "muhammadrizkyardiansah93@gmail.com"));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = "Forgot Password OTP";
            mimeMessage.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Email: {email}\nOTP: {otp}\nExpired Date: {expired.ToString()}"
            };

            SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp-relay.sendinblue.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("muhammadrizkyardiansah93@gmail.com", "A6vz189fZFqs5tGW");
            smtp.Send(mimeMessage);
            smtp.Disconnect(true);
        }

        private string GenerateSalt(int num)
        {
            return BCrypt.Net.BCrypt.GenerateSalt(num);
        }

        private string HashPassword(string plainText, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText, salt);
        }

        private bool ValidatePassword(string inputPassword, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, correctHash);
        }
    }
}
