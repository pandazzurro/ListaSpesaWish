using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.EF
{
    public class UtentiListaSpesa
    {
        public long IdUtentiListaSpesa { get; set; }

        public Utente Utente { get; set; }
        public ListaSpesa ListaSpesa { get; set; }
    }
}
