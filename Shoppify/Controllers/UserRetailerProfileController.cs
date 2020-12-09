using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Shoppify.Models;

namespace Project.Controllers
{

    public class UserRetailerProfileController : ApiController
    {
        projectEntities db = new projectEntities();
        [Route("GetUserProfile")]
        public HttpResponseMessage GetUserProfile(string uemail)
        {
            DateTime newdate;

            var uprof = (from u in db.tblUsers
                         join o in db.tblOrders
                         on u.useremail equals o.useremail
                         join p in db.tblProducts
                         on o.productid equals p.productid
                         select new
                         {
                             u.useremail,
                             u.username,
                             u.userphone,
                             u.userapartment,
                             u.userstreet,
                             u.usertown,
                             u.userstate,
                             u.userpincode,
                             u.usercountry,
                             o.orderid,
                             o.orderdate,
                             p.productname,
                             p.productprice,
                             p.productbrand,
                             p.productdescription,
                             o.orderquantity,

                         }).Where(u => u.useremail == uemail).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, uprof);
        }
        [Route("GetRetailerProfile")]
        public HttpResponseMessage GetRetailerProfile(string retaileremail)
        {
            var rprof = (from r in db.tblRetailers
                         join p in db.tblProducts
                         on r.retailerid equals p.retailerid
                         join o in db.tblOrders
                         on p.productid equals o.productid
                         let RetailerRevenue = p.productprice * o.orderquantity
                         select new
                         {
                             r.retailerid,
                             r.retailername,
                             r.retaileremail,
                             p.productname,
                             p.productprice,
                             p.productbrand,
                             o.useremail,
                             o.orderquantity,
                             RetailerRevenue

                         }).Where(r => r.retaileremail == retaileremail).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, rprof);
        }

        [Route("GetProfile")]
        [HttpGet]
        public HttpResponseMessage GetProfile(string uemail)
        {
            var result = db.tblUsers.Where(x => x.useremail == uemail).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}