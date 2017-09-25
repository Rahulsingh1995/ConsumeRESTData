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

namespace CustomerSite.Controllers
{
    public class ordersController : ApiController
    {
        private CSModel db = new CSModel();

        // GET: api/orders
        public HttpResponseMessage Getorders()
        {

            var query = from cust in db.customers
                        join order in db.orders on cust.customer_pk equals order.customer_pk
                        join product in db.products on order.product_pk equals product.product_pk
                        select new { name= cust.cust_name, product_name=product.product_name, quantity=order.order_quantity,amount= order.order_quantity*product.product_price };
                

            if (db.orders == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, query);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // GET: api/orders/5
        [ResponseType(typeof(order))]
        public HttpResponseMessage Getorder(int id)
        {
            var query = from cust in db.customers
                        join order in db.orders on cust.customer_pk equals order.customer_pk
                        join product in db.products on order.product_pk equals product.product_pk
                        where order.order_pk.Equals(id)
                        select new { name = cust.cust_name, product_name = product.product_name, quantity = order.order_quantity, amount = order.order_quantity * product.product_price };
            if (query == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK,query);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

       // PUT: api/orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putorder(int id, order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.order_pk)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!orderExists(id))
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

        // POST: api/orders
        [ResponseType(typeof(order))]
        public IHttpActionResult Postorder(order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.orders.Add(order);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (orderExists(order.order_pk))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = order.order_pk }, order);
        }

        // DELETE: api/orders/5
        [ResponseType(typeof(order))]
        public IHttpActionResult Deleteorder(int id)
        {
            order order = db.orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool orderExists(int id)
        {
            return db.orders.Count(e => e.order_pk == id) > 0;
        }
        
    }
}