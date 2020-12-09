using Shoppify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shoppify.Controllers
{
    public class RetailerController : ApiController
    {
        projectEntities db = new projectEntities();

        [Route("retailer-login")]
        [HttpGet]
        public HttpResponseMessage getRetailer(string retaileremail, string retailerpassword)
        {
            var retailer = db.tblRetailers.Where(x => x.retaileremail == retaileremail && x.retailerpassword == retailerpassword && x.approved == 1).ToList();
            if (retailer.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "valid");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invalid");
            }
        }

        [Route("get-retailerid")]
        [HttpGet]
        public HttpResponseMessage getRetailerId(string retaileremail)
        {
            var retailer = db.tblRetailers.Where(x => x.retaileremail == retaileremail).Select(x => x.retailerid);
            if (retailer != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retailer);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invalid");
            }
        }

        [Route("retailer-register")]
        [HttpPost]

        public HttpResponseMessage register(string retailername, string retaileremail, string retailerpassword)
        {

            tblRetailer retailer = new tblRetailer()
            {
                retailername = retailername,
                retaileremail = retaileremail,
                retailerpassword = retailerpassword,
                approved = 0

            };

            db.tblRetailers.Add(retailer);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "valid");
        }

        public bool CheckRetailer(string retaileremail, string oldpassword)
        {
            var result = db.tblRetailers.Where(x => x.retaileremail == retaileremail && x.retailerpassword == oldpassword);
            if(result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getid(string retaileremail)
        {
            tblRetailer retailer = new tblRetailer();
            retailer.retailerid = db.tblRetailers.First(x=>x.retaileremail==retaileremail).retailerid;
            return retailer.retailerid;
        }

        [Route("ChangePassword")]
        [HttpPut]
        public HttpResponseMessage ChangePass(string retaileremail, string retailerpassword, string newpassword)
        {
            
            if(CheckRetailer(retaileremail, retailerpassword))
            {
                int retailerid = getid(retaileremail);
                var query = db.tblRetailers.Find(retailerid);
                query.retailerpassword = newpassword;
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
