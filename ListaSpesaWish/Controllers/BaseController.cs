using ListaSpesaWish.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ListaSpesaWish.Controllers
{
    public class BaseController : ApiController
    {
        private ListaSpesaContext _db;

        public ListaSpesaContext db
        {
            get {
                if (_db == default(ListaSpesaContext))
                    _db = new ListaSpesaContext();
                return _db;
            }
            set { _db = value; }
        }
    }
}