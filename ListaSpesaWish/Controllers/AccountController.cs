﻿using ListaSpesaWish.EF;
using ListaSpesaWish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;

namespace ListaSpesaWish.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {

        [Route("Login")]
        [ResponseType(typeof(UtenteDto))]
        public async Task<IHttpActionResult> PostUtente(LoginDto login)
        {
            Utente utente = await db.Utente.Where(x => string.Compare(x.Username, login.Username, true) == 0 && 
                                                       string.Compare(x.Password, login.Password, true) == 0)
                                           .FirstOrDefaultAsync();
            if (utente == null) return NotFound();
            else return Ok(new UtenteDto(utente));
            
        }
    }
}
