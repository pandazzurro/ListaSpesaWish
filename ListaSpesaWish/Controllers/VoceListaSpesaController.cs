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
    public class VoceListaSpesaController : ApiController
    {
        private ListaSpesaContext db = new ListaSpesaContext();

        // GET: api/VoceListaSpesa
        public IQueryable<VoceListaSpesa> GetVoceListaSpesa()
        {
            return db.VoceListaSpesa;
        }

        // GET: api/VoceListaSpesa/5
        [ResponseType(typeof(VoceListaSpesa))]
        public async Task<IHttpActionResult> GetVoceListaSpesa(long id)
        {
            VoceListaSpesa voceListaSpesa = await db.VoceListaSpesa.FindAsync(id);
            if (voceListaSpesa == null)
            {
                return NotFound();
            }

            return Ok(voceListaSpesa);
        }

        // PUT: api/VoceListaSpesa/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVoceListaSpesa(long id, VoceListaSpesa voceListaSpesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != voceListaSpesa.IdVoceListaSpesa)
            {
                return BadRequest();
            }

            db.Entry(voceListaSpesa).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoceListaSpesaExists(id))
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

        // POST: api/VoceListaSpesa
        [ResponseType(typeof(VoceListaSpesa))]
        public async Task<IHttpActionResult> PostVoceListaSpesa(VoceListaSpesa voceListaSpesa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VoceListaSpesa.Add(voceListaSpesa);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = voceListaSpesa.IdVoceListaSpesa }, voceListaSpesa);
        }

        // DELETE: api/VoceListaSpesa/5
        [ResponseType(typeof(VoceListaSpesa))]
        public async Task<IHttpActionResult> DeleteVoceListaSpesa(long id)
        {
            VoceListaSpesa voceListaSpesa = await db.VoceListaSpesa.FindAsync(id);
            if (voceListaSpesa == null)
            {
                return NotFound();
            }

            db.VoceListaSpesa.Remove(voceListaSpesa);
            await db.SaveChangesAsync();

            return Ok(voceListaSpesa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VoceListaSpesaExists(long id)
        {
            return db.VoceListaSpesa.Count(e => e.IdVoceListaSpesa == id) > 0;
        }
    }
}