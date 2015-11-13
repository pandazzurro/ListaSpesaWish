using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.EF
{
    public class ListaSpesa
    {
        public long IdListaSpesa { get; set; }
        public string Nome { get; set; }
        
        public ICollection<UtentiListaSpesa> UtentiListaSpesa { get; set; }
        public ICollection<VoceListaSpesa> VociListaSpesa { get; set; }

        public ListaSpesa()
        {
            UtentiListaSpesa = new HashSet<UtentiListaSpesa>();
            VociListaSpesa = new HashSet<VoceListaSpesa>();
        }
    }
}
