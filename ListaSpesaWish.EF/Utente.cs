using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.EF
{
    public class Utente
    {
        public long IdUtente { get; set; }
        public string Username { get; set;}
        public string Password { get; set; }
        public string Email { get; set; }

        public ICollection<UtentiListaSpesa> UtentiListaSpesa { get; set; }

        public Utente()
        {
            UtentiListaSpesa = new HashSet<UtentiListaSpesa>();
        }
    }
}
