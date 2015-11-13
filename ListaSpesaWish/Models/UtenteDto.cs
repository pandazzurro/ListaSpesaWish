using ListaSpesaWish.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListaSpesaWish.Models
{
    public class UtenteDto
    {
        public long IdUtente { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public UtenteDto(Utente u)
        {
            this.Email = u.Email;
            this.IdUtente = u.IdUtente;
            this.Username = u.Username;
        }

        public UtenteDto() { }
    }
}