﻿using System;
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

        public VoceListaSpesa VoceListaSpesa { get; set; }
    }
}