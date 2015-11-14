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
using ListaSpesaWish.Service;
using ListaSpesaWish.Models;

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
                     .Include(x => x.VociListaSpesa)
                     .Include(x => x.VociListaSpesa.Select(y => y.Voce))
                     .Include(x => x.UtentiListaSpesa.Select(y => y.Utente));
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
            
            db.UtentiListaSpesa.RemoveRange(db.UtentiListaSpesa.Where(x => x.ListaSpesa.IdListaSpesa == listaSpesa.IdListaSpesa));
            db.VoceListaSpesa.RemoveRange(db.VoceListaSpesa.Where(x => x.ListaSpesa.IdListaSpesa == listaSpesa.IdListaSpesa));

            db.ListaSpesa.Remove(listaSpesa);
            await db.SaveChangesAsync();

            return Ok(listaSpesa);
        }

        [HttpDelete]
        [Route("api/ListaSpesa/Clear/{id}")]
        [ResponseType(typeof(ClearListaDto))]
        public async Task<IHttpActionResult> ClearListaSpesa(long id)
        {
            ListaSpesa listaSpesa = await db.ListaSpesa.FindAsync(id);
            
            if (listaSpesa == null)
            {
                return NotFound();
            }

            if(!db.ListaSpesa
                 .Include(x => x.VociListaSpesa)
                 .Where(x => x.VociListaSpesa.Where(y => y.Comprata == false).Any())
                 .Any())
            {
                db.UtentiListaSpesa.RemoveRange(db.UtentiListaSpesa.Where(x => x.ListaSpesa.IdListaSpesa == listaSpesa.IdListaSpesa));
                db.VoceListaSpesa.RemoveRange(db.VoceListaSpesa.Where(x => x.ListaSpesa.IdListaSpesa == listaSpesa.IdListaSpesa));
                db.ListaSpesa.Remove(listaSpesa);
                await db.SaveChangesAsync();
                return Ok(new ClearListaDto() { Response = true });
            }
            return Ok(new ClearListaDto() { Response = false });            
        }

        [HttpPost]
        [Route("api/ListaSpesa/Mail")]        
        public async Task<IHttpActionResult> SendListaSpesa(ListaSpesa listaSpesa)
        {
            ListaSpesaDto dto = await db.ListaSpesa
                                        .Include(x => x.UtentiListaSpesa)
                                        .Include(x => x.VociListaSpesa)
                                        .Where(x => x.IdListaSpesa == listaSpesa.IdListaSpesa)
                                        .Select(x => new ListaSpesaDto()
                                        {
                                            Nome = x.Nome,
                                            Voci = x.VociListaSpesa.Select(y => new VoceDto()
                                                                    {
                                                                        Name = y.Voce.Name,
                                                                        Comprata = y.Comprata
                                                                    }).ToList(),
                                            Utenti = x.UtentiListaSpesa.Select(y => y.Utente).ToList()
                                        })
                                        .FirstOrDefaultAsync();
   
            if(await EmailService.SendAsync(dto))
                return Ok();
            return InternalServerError();
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