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

        public virtual ListaSpesa ListaSpesa { get; set; }
        public virtual Voce Voce { get; set; }        
    }
}
