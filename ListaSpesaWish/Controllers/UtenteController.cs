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
    public class UtenteController : ApiController
    {
        private ListaSpesaContext db = new ListaSpesaContext();

        // GET: api/Utente
        public IQueryable<Utente> GetUtente()
        {
            return db.Utente;
        }

        // GET: api/Utente/5
        [ResponseType(typeof(Utente))]
        public async Task<IHttpActionResult> GetUtente(long id)
        {
            Utente utente = await db.Utente.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }

            return Ok(utente);
        }

        // PUT: api/Utente/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUtente(long id, Utente utente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != utente.IdUtente)
            {
                return BadRequest();
            }

            db.Entry(utente).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtenteExists(id))
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

        // POST: api/Utente
        [ResponseType(typeof(Utente))]
        public async Task<IHttpActionResult> PostUtente(Utente utente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Utente.Add(utente);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = utente.IdUtente }, utente);
        }

        // DELETE: api/Utente/5
        [ResponseType(typeof(Utente))]
        public async Task<IHttpActionResult> DeleteUtente(long id)
        {
            Utente utente = await db.Utente.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }

            db.Utente.Remove(utente);
            await db.SaveChangesAsync();

            return Ok(utente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UtenteExists(long id)
        {
            return db.Utente.Count(e => e.IdUtente == id) > 0;
        }
    }
}