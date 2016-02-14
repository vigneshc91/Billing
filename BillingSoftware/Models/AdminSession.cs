using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingSoftware.Models
{
    public class AdminSession<T> where T : class
    {
        public string id { get; set; }

        public string ipaddress { get; set; }

        public DateTime created_at { get; set; }

        public T user { get; set; }

        public int user_type { get; set; }
    }
}