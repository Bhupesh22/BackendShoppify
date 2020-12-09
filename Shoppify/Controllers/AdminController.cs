using Shoppify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace Shoppify.Controllers
{
    public class AdminController : ApiController
    {
        projectEntities db = new projectEntities();
        [Route("admin-dashboard")]
        [HttpGet]
        public HttpResponseMessage getRetailers()
        {
            var retailers = db.tblRetailers.Where(x => x.approved == 0).ToList();
            if (retailers.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retailers);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "No retailers");
            }
        }
        [Route("approve-retailer")]
        [HttpPut]
        public dynamic ApproveRetailer(int retailerid, string retaileremail)
        {
            var retailer = db.tblRetailers.Find(retailerid);
            retailer.approved = 1;
            db.Entry(retailer).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Approved");
        }

        [Route("send-email")]
        [HttpGet]
        public HttpResponseMessage SendEmail(string retaileremail)
        {
            string from = "onlineshoppinglti@gmail.com";
            string subject = "Welcome to online shopping";
            string body = "Hello , online shopping welcomes you to be a contributor as a retailer";
            SmtpClient smtp = new SmtpClient();
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(from);
            mm.To.Add(retaileremail);
            mm.Subject = subject;
            mm.Body = body;
            smtp.Send(mm);
            return Request.CreateResponse(HttpStatusCode.OK, "Done");
        }
    }
}