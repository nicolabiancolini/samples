using Labs.MvcHater.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.MvcHater.Endpoints
{
    public class WooEndpoint
    {
        public String Get()
        {
            return JsonConvert.SerializeObject(new Woo());
        }
    }
}
