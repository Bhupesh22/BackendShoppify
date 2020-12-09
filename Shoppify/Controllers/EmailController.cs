using Shoppify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shoppify.Controllers
{
    public class EmailController : ApiController
    {
        projectEntities db = new projectEntities();

        //[System.Web.Http.AcceptVerbs("GET", "POST")]
        //[System.Web.Http.HttpGet]
        public bool CheckEmail(string email)
        {
            var isValidEmail = db.tblUsers.Where(w => w.useremail == email).FirstOrDefault();
            if (isValidEmail != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [Route("SendUserEmail")]
        [HttpGet]
        public async Task<int> SendEmail(string to)
        {
            if (CheckEmail(to) == true)
            {
                string from = "onlineshoppinglti@gmail.com";
                string subject = "Welcome to online shopping";
                Random generator = new Random();
                int r = generator.Next(1000, 10000);
                string body = "Hello , Your otp is " + r;

                SmtpClient smtp = new SmtpClient();

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(from);
                mm.To.Add(to);
                mm.Subject = subject;
                mm.Body = body;
                await Task.Run(() => smtp.SendAsync(mm, null));
                return r;
            }
            else
            {
                return 0;
            }

        }
        public bool CheckRetailerEmail(string email)
        {
            var isValidEmail = db.tblRetailers.Where(w => w.retaileremail == email).FirstOrDefault();
            if (isValidEmail != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [Route("SendRetailerEmail")]
        [HttpGet]
        public async Task<int> SendRetailerEmail(string to)
        {
            if (CheckRetailerEmail(to) == true)
            {
                string from = "onlineshoppinglti@gmail.com";
                string subject = "Welcome to online shopping";
                Random generator = new Random();
                int r = generator.Next(1000, 10000);
                string body = "Hello , Your otp is " + r;

                SmtpClient smtp = new SmtpClient();

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(from);
                mm.To.Add(to);
                mm.Subject = subject;
                mm.Body = body;
                await Task.Run(() => smtp.SendAsync(mm, null));
                return r;
            }
            else
            {
                return 0;
            }

        }

        [Route("UpdateUserPassword")]
        [HttpPut]
        public dynamic UpdatePassword(string email, string password)
        {
            //var query = from user in tblUser where user.email == email select user;
            var query = db.tblUsers.Find(email);
            query.userpassword = password;
            db.Entry(query).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Valid");
        }

        public int getid(string retaileremail)
        {
            tblRetailer retailer = new tblRetailer();
            retailer.retailerid = db.tblRetailers.First(x => x.retaileremail == retaileremail).retailerid;
            return retailer.retailerid;
        }

        [Route("UpdateRetailerPassword")]
        [HttpPut]
        public dynamic UpdateRetailerPassword(string retaileremail, string retailerpassword)
        {
            //var query = from user in tblUser where user.email == email select user;
            int retailerid = getid(retaileremail);
            var query = db.tblRetailers.Find(retailerid);
            query.retailerpassword = retailerpassword;
            db.Entry(query).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Valid");
        }

    }
}