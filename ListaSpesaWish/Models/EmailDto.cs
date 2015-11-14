using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.Models
{
    public class EmailDto
    {
        public string Body {get;set;}
        public string Destination { get; set; }
        public string Subject { get; set; }
    }
}
