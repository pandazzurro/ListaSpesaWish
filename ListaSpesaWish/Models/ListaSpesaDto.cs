using ListaSpesaWish.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.Models
{
    public class ListaSpesaDto
    {
        public string Nome { get; set; }
        public ICollection<Utente> Utenti { get; set; }
        public ICollection<VoceDto> Voci { get; set; }
    }
}
