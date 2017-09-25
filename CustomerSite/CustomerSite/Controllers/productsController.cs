using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CustomerSite;
using System.Net.Http.Headers;
using CustomerSite.Models;
using Newtonsoft.Json;

namespace CustomerSite.Controllers
{
    public class productsController : ApiController
    {
        private CSModel db = new CSModel();

        // GET: api/products
        public HttpResponseMessage Getproducts()
        {
            var query = from pro in db.products
                        select new { product_name = pro.product_name, product_price = pro.product_price };
            if (db.products == null)
            {
               
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, query);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // GET: api/products/5
        [ResponseType(typeof(product))]
        public HttpResponseMessage Getproduct(int id)
        {
            var query = from pro in db.products
                        where pro.product_pk.Equals(id)
                        select new { product_name = pro.product_name, product_price = pro.product_price };
            if (query == null)
            {

                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }

        var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, query);
       response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

       // PUT: api/products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putproduct(int id, product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.product_pk)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/products
        [ResponseType(typeof(product))]
        public IHttpActionResult Postproduct(product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.products.Add(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (productExists(product.product_pk))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = product.product_pk }, product);
        }

        // DELETE: api/products/5
        [ResponseType(typeof(product))]
        public IHttpActionResult Deleteproduct(int id)
        {
            product product = db.products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productExists(int id)
        {
            return db.products.Count(e => e.product_pk == id) > 0;
        }
        
    }
}