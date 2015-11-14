using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.EF
{
    public class Voce
    {
        public long IdVoce { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public ICollection<VoceListaSpesa> VociListaSpesa { get; set; }

        public Voce()
        {
            VociListaSpesa = new HashSet<VoceListaSpesa>();
        }
    }
}
