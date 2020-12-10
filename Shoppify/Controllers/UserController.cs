using Shoppify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Shoppify.Controllers
{
    public class UserController : ApiController
    {
        projectEntities db = new projectEntities();
        [Route("do-login")]
        [HttpGet]
        public HttpResponseMessage checkLogin(string useremail, string userpassword)
        {
            try
            {
                var result = db.tblUsers.Where(x => x.useremail == useremail).FirstOrDefault();
                var pass = db.tblUsers.Where(x => x.userpassword == userpassword).FirstOrDefault();
                if (result != null && pass != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Invalid");
                }
            }catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invalid");
            }
        }

        public bool CheckEmail(string email)
        {
            var isValidEmail = db.tblUsers.Where(w => w.useremail == email).FirstOrDefault();
            if (isValidEmail == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckEmail1(string email, string userpassword)
        {   
            try
            {
                var isValidEmail = db.tblUsers.Where(w => w.useremail == email).FirstOrDefault();
                var pass = db.tblUsers.Where(w => w.userpassword == userpassword).FirstOrDefault();
                if (isValidEmail != null && pass != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
            
        }


        [Route("RegisterUser")]
        [HttpPost]
        public HttpResponseMessage UserRegister(string useremail, string username, string userphone,
               string userpassword, string userapartment, string userstreet, string usertown, string userstate,
               string userpincode, string usercountry)
        {
            if (CheckEmail(useremail))
            {
                tblUser user = new tblUser()
                {
                    useremail = useremail,
                    username = username,
                    userphone = userphone,
                    userpassword = userpassword,
                    userapartment = userapartment,
                    userstreet = userstreet,
                    usertown = usertown,
                    userstate = userstate,
                    userpincode = userpincode,
                    usercountry = usercountry
                };
                db.tblUsers.Add(user);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            return Request.CreateResponse(HttpStatusCode.OK, "invalid");

        }
        [Route("userchangepassword")]
        [HttpPut]
        public HttpResponseMessage changeUserPassword(string useremail, string userpassword, string newpassword)
        {
            if (CheckEmail1(useremail, userpassword))
            {
                var query = db.tblUsers.Find(useremail);
                query.userpassword = newpassword;
                db.Entry(query).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "valid");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "invalid");
            }
        }

    }

}