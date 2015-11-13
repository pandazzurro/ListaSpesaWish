using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaSpesaWish.EF
{
    public class DbInitializer : CreateDatabaseIfNotExists<ListaSpesaContext>
    {
        protected override void Seed(ListaSpesaContext context)
        {
            context.Utente.Add(new Utente() { Username = "test", Password = "test", Email = "andreatosato@email.it", UtentiListaSpesa = null });
            context.Utente.Add(new Utente() { Username = "test2", Password = "test2", Email = "dossobuono@gmail.com", UtentiListaSpesa = null });
            context.Voce.Add(new Voce() { Name = "mela"});
            base.Seed(context);
        }
    }
}
