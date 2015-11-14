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
    public class UtentiListaSpesaController : ApiController
    {
        private ListaSpesaContext db = new ListaSpesaContext();

        // GET: api/UtentiListaSpesa
        public IQueryable<UtentiListaSpesa> GetUtentiListaSpesa()
        {
            return db.UtentiListaSpesa
                     .Include(x => x.ListaSpesa)
                     .Include(x => x.Utente);
        }

        // GET: api/UtentiListaSpesa/5
        [ResponseType(typeof(UtentiListaSpesa))]
        public async Task<IHttpActionResult> GetUtentiListaSpesa(long id)
        {
            UtentiListaSpesa utentiListaSpesa = await db.UtentiListaSpesa
                                                        .Include(x => x.ListaSpesa)
                                                        .Include(x => x.Utente)
                                                        .SingleOrDefaultAsync(x => x.IdUtentiListaSpesa == id);
            if (utentiListaSpesa == null)
            {
                return NotFound();
            }

            return Ok(utentiListaSpesa);
        }

        // PUT: api/UtentiListaSpesa/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUtentiListaSpesa(long id, UtentiListaSpesa utentiListaSpesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != utentiListaSpesa.IdUtentiListaSpesa)
            {
                return BadRequest();
            }

            db.Entry(utentiListaSpesa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtentiListaSpesaExists(id))
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

        // POST: api/UtentiListaSpesa
        [ResponseType(typeof(UtentiListaSpesa))]
        public async Task<IHttpActionResult> PostUtentiListaSpesa(UtentiListaSpesa utentiListaSpesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            utentiListaSpesa.Utente = await db.Utente.FindAsync(utentiListaSpesa.Utente.IdUtente);
            utentiListaSpesa.ListaSpesa = await db.ListaSpesa.FindAsync(utentiListaSpesa.ListaSpesa.IdListaSpesa);

            db.UtentiListaSpesa.Add(utentiListaSpesa);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = utentiListaSpesa.IdUtentiListaSpesa }, utentiListaSpesa);
        }

        // DELETE: api/UtentiListaSpesa/5
        [ResponseType(typeof(UtentiListaSpesa))]
        public async Task<IHttpActionResult> DeleteUtentiListaSpesa(long id)
        {
            UtentiListaSpesa utentiListaSpesa = await db.UtentiListaSpesa.FindAsync(id);
            if (utentiListaSpesa == null)
            {
                return NotFound();
            }

            db.UtentiListaSpesa.Remove(utentiListaSpesa);
            await db.SaveChangesAsync();

            return Ok(utentiListaSpesa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UtentiListaSpesaExists(long id)
        {
            return db.UtentiListaSpesa.Count(e => e.IdUtentiListaSpesa == id) > 0;
        }
    }
}