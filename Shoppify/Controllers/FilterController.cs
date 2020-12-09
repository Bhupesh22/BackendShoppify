using Shoppify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shoppify.Controllers
{
    public class FilterController : ApiController
    {
        projectEntities db = new projectEntities();

        [Route("Filterbyprice")]
        public HttpResponseMessage GetPrice(string price)
        {

            switch (price)
            {
                case "0-1999":
                    return Request.CreateResponse(HttpStatusCode.OK, db.tblProducts.Where(s => s.productprice < 2000).ToList());

                case "2000-9999":
                    return Request.CreateResponse(HttpStatusCode.OK, db.tblProducts.Where(s => s.productprice >= 2000 && s.productprice <= 9999).ToList());

                case "10000-80000":
                    return Request.CreateResponse(HttpStatusCode.OK, db.tblProducts.Where(s => s.productprice >= 10000).ToList());

                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product price out of range");
            }
        }

        [Route("Filterbycategory")]
        public HttpResponseMessage GetCategory(string category)
        {
            // IEnumerable<> lstUser = new List<UserDetail>();

            switch (category)
            {
                case "Mobile and Accessories":
                    var mob = (from p in db.tblProducts
                               join c in db.tblCategories
                               on p.categoryid equals c.categoryid
                               select new
                               {
                                   p.productname,
                                   p.productprice,
                                   p.productdescription,
                                   p.productbrand,
                                   p.productimage1,
                                   c.categoryid,
                                   c.categoryname
                               }).Where(c => c.categoryname == "Mobile and Accessories").ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, mob);

                case "TV & Home Entertainment":
                    var ent = (from p in db.tblProducts
                               join c in db.tblCategories
                               on p.categoryid equals c.categoryid
                               select new
                               {
                                   p.productname,
                                   p.productprice,
                                   p.productdescription,
                                   p.productbrand,
                                   p.productimage1,
                                   c.categoryid,
                                   c.categoryname
                               }).Where(c => c.categoryname == "TV & Home Entertainment").ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ent);

                case "Watches":
                    var watch = (from p in db.tblProducts
                                 join c in db.tblCategories
                                 on p.categoryid equals c.categoryid
                                 select new
                                 {
                                     p.productname,
                                     p.productprice,
                                     p.productdescription,
                                     p.productbrand,
                                     p.productimage1,
                                     c.categoryid,
                                     c.categoryname
                                 }).Where(c => c.categoryname == "Watches").ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, watch);

                case "Shoes":
                    var shoes = (from p in db.tblProducts
                                 join c in db.tblCategories
                                 on p.categoryid equals c.categoryid
                                 select new
                                 {
                                     p.productname,
                                     p.productprice,
                                     p.productdescription,
                                     p.productbrand,
                                     p.productimage1,
                                     c.categoryid,
                                     c.categoryname
                                 }).Where(c => c.categoryname == "Shoes").ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, shoes);

                case "Clothing":
                    var clothes = (from p in db.tblProducts
                                   join c in db.tblCategories
                                   on p.categoryid equals c.categoryid
                                   select new
                                   {
                                       p.productname,
                                       p.productprice,
                                       p.productdescription,
                                       p.productbrand,
                                       p.productimage1,
                                       c.categoryid,
                                       c.categoryname
                                   }).Where(c => c.categoryname == "Clothing").ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, clothes);

                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No such category");
            }
        }

        [Route("SearchProduct")]
        public HttpResponseMessage GetSearchProduct(string search)
        {
            var result = db.tblProducts.Where(x => x.productname.StartsWith(search) || search == null).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
            //return Ok(result);
        }


    }
}