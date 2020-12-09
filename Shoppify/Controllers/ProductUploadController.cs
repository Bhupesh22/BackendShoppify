using Shoppify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Shoppify.Controllers
{
    public class ProductUploadController : ApiController
    {
        projectEntities db = new projectEntities();
        [HttpPost]
        [Route("UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            string imageName = null;

            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Files["Image"];

            //Create custom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
            postedFile.SaveAs(filePath);


            //Save to DB
            using (projectEntities db = new projectEntities())
            {

                tblProduct tblProduct = new tblProduct()
                {
                    retailerid = Convert.ToInt32(httpRequest["RetailerId"]),
                    productname = httpRequest["ProductName"],
                    productdescription = httpRequest["ProductDescription"],
                    productbrand = httpRequest["ProductBrand"],
                    productquantity = Convert.ToInt32(httpRequest["ProductQuantity"]),
                    productprice = Convert.ToInt32(httpRequest["ProductPrice"]),
                    categoryid = Convert.ToInt32(httpRequest["CategoryId"]),


                    productimage1 = imageName,

                };
                db.tblProducts.Add(tblProduct);
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Route("GetRetailersId")]
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

        [Route("Getproducts")]

        public HttpResponseMessage Get()
        {
            var product = db.tblProducts.ToList();
            if (product.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No data found");
            }
        }


        [Route("InsertIntoCart")]
        [HttpPost]
        public HttpResponseMessage InsertCart(string useremail, int productid, int cartquantity)
        {
            tblCart cart = new tblCart()
            {
                useremail = useremail,
                productid = productid,
                cartquantity = cartquantity
            };
            db.tblCarts.Add(cart);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }



        [Route("GetUserCart")]
        [HttpGet]
        public HttpResponseMessage getUserCart(string useremail)
        {

            var result = from c in db.tblCarts
                         join p in db.tblProducts
      on c.productid equals p.productid
                         join r in db.tblRetailers on p.retailerid equals r.retailerid
                         join ct in db.tblCategories on p.categoryid equals ct.categoryid
                         where c.useremail == useremail
                         select new
                         {
                             p.productid,
                             p.productname,
                             p.productimage1,
                             p.productdescription,
                             p.productprice,
                             ct.categoryname,
                             c.useremail,
                             c.cartquantity,
                             r.retailerid,
                             r.retailername,
                             r.retaileremail,
                             c.cartid,
                             total = c.cartquantity * p.productprice
                         };
            //var result = db.tblCarts.Where(x => x.useremail == useremail);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("RemoveFromCart")]
        [HttpDelete]
        public HttpResponseMessage removeFromCart(int cartid, int productid)
        {
            tblCart result = db.tblCarts.Find(cartid, productid);
            db.tblCarts.Remove(result);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        [Route("GetSubtotal")]
        [HttpGet]
        public HttpResponseMessage getSubtotal(string useremail)
        {
            var result = (from p in db.tblProducts
                          join c in db.tblCarts
                          on p.productid equals c.productid
                          where c.useremail == useremail
                          select new
                          {
                              p.productprice
                          }).ToList();

            double sum = (double)result.Sum(x => x.productprice);
            double count = result.Count();
            List<double> resultnew = new List<double>() { count, sum };
            return Request.CreateResponse(HttpStatusCode.OK, resultnew);
        }

        [Route("PurchaseProduct")]
        [HttpPost]
        public HttpResponseMessage purchaseProduct(string useremail, int productid, int retailerid, int orderquantity)
        {
            string date = DateTime.Now.ToString();
            
            tblOrder order = new tblOrder()
            {
                orderdate = date,
                useremail = useremail,
                productid = productid,
                retailerid = retailerid,
                orderquantity = orderquantity

            };
            db.tblOrders.Add(order);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }
    }
}