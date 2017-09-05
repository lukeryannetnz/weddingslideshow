using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace weddingslideshow.api.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        public IEnumerable<string> Get(string hashTag)
        {
            return new [] {"http://www.rewildthyself.com/wp-content/uploads/2015/03/poop.jpeg"};
        }
    }
}