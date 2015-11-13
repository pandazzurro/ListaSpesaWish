using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ListaSpesaWish.EF;

namespace ListaSpesaWish.Controllers
{
    public class ListaSpesaController : ApiController
    {
        private ListaSpesaContext db = new ListaSpesaContext();

        // GET: api/ListaSpesa
        public IQueryable<ListaSpesa> GetListaSpesa()
        {
            return db.ListaSpesa
                     .Include(x => x.UtentiListaSpesa)
                     .Include(x => x.VociListaSpesa);
        }

        // GET: api/ListaSpesa/5
        [ResponseType(typeof(ListaSpesa))]
        public async Task<IHttpActionResult> GetListaSpesa(long id)
        {
            ListaSpesa listaSpesa = await db.ListaSpesa                                            
                                            .FindAsync(id);
            if (listaSpesa == null)
            {
                return NotFound();
            }

            return Ok(listaSpesa);
        }

        // PUT: api/ListaSpesa/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutListaSpesa(long id, ListaSpesa listaSpesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != listaSpesa.IdListaSpesa)
            {
                return BadRequest();
            }

            db.Entry(listaSpesa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListaSpesaExists(id))
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

        // POST: api/ListaSpesa
        [ResponseType(typeof(ListaSpesa))]
        public async Task<IHttpActionResult> PostListaSpesa(ListaSpesa listaSpesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ListaSpesa.Add(listaSpesa);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = listaSpesa.IdListaSpesa }, listaSpesa);
        }

        // DELETE: api/ListaSpesa/5
        [ResponseType(typeof(ListaSpesa))]
        public async Task<IHttpActionResult> DeleteListaSpesa(long id)
        {
            ListaSpesa listaSpesa = await db.ListaSpesa.FindAsync(id);
            if (listaSpesa == null)
            {
                return NotFound();
            }

            db.ListaSpesa.Remove(listaSpesa);
            await db.SaveChangesAsync();

            return Ok(listaSpesa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ListaSpesaExists(long id)
        {
            return db.ListaSpesa.Count(e => e.IdListaSpesa == id) > 0;
        }
    }
}