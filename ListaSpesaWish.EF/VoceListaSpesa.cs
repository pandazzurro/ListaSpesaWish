using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.EF
{
    public class VoceListaSpesa
    {
        public long IdVoceListaSpesa { get; set; }
        public bool Comprata { get; set; }

        public ListaSpesa ListaSpesa { get; set; }
        public ICollection<Voce> Voci { get; set; }

        public VoceListaSpesa()
        {
            Voci = new HashSet<Voce>();
        }
    }
}
