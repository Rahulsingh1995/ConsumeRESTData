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
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CustomerSite.Controllers
{
    public class customersController : ApiController
    {
        private CSModel db = new CSModel();

        // GET: api/customers
        public HttpResponseMessage Getcustomers()
        {
            var query = from cust in db.customers
                     select new { cust_name = cust.cust_name, contact_no=cust.contact_no };
            if (db.customers == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, query);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        // GET: api/customers/5
        [ResponseType(typeof(customer))]
        public HttpResponseMessage Getcustomer(int id)
        {
            var query = from cust in db.customers
                        where cust.customer_pk.Equals(id)
                        select new { cust_name = cust.cust_name, contact_no = cust.contact_no };
            if (query == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, query);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;

        }
       
        // PUT: api/customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcustomer(int id, customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.customer_pk)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!customerExists(id))
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

        // POST: api/customers
        [ResponseType(typeof(customer))]
        public IHttpActionResult Postcustomer(customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.customers.Add(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (customerExists(customer.customer_pk))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = customer.customer_pk }, customer);
        }

        // DELETE: api/customers/5
        [ResponseType(typeof(customer))]
        public IHttpActionResult Deletecustomer(int id)
        {
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool customerExists(int id)
        {
            return db.customers.Count(e => e.customer_pk == id) > 0;
        }
    }
}