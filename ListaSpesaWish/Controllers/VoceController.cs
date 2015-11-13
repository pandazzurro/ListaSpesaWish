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
    public class VoceController : ApiController
    {
        private ListaSpesaContext db = new ListaSpesaContext();

        // GET: api/Voce
        public IQueryable<Voce> GetVoce()
        {            
            return db.Voce;
        }

        // GET: api/Voce/5
        [ResponseType(typeof(Voce))]
        public async Task<IHttpActionResult> GetVoce(long id)
        {
            Voce voce = await db.Voce.FindAsync(id);
            if (voce == null)
            {
                return NotFound();
            }

            return Ok(voce);
        }

        // PUT: api/Voce/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVoce(long id, Voce voce)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != voce.IdVoce)
            {
                return BadRequest();
            }

            db.Entry(voce).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoceExists(id))
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

        // POST: api/Voce
        [ResponseType(typeof(Voce))]
        public async Task<IHttpActionResult> PostVoce(Voce voce)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Voce.Add(voce);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = voce.IdVoce }, voce);
        }

        // DELETE: api/Voce/5
        [ResponseType(typeof(Voce))]
        public async Task<IHttpActionResult> DeleteVoce(long id)
        {
            Voce voce = await db.Voce.FindAsync(id);
            if (voce == null)
            {
                return NotFound();
            }

            db.Voce.Remove(voce);
            await db.SaveChangesAsync();

            return Ok(voce);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoceExists(long id)
        {
            return db.Voce.Count(e => e.IdVoce == id) > 0;
        }
    }
}