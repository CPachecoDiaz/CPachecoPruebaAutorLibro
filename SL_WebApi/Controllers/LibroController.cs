using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL_WebApi.Controllers
{
    [RoutePrefix("api/Libro")]
    public class LibroController : ApiController
    {
        [AcceptVerbs("OPTIONS")]
        [Route("{*any}")]
        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Accept");
            return response;
        }

        [HttpPost]
        [Route("GetAll")]
        public IHttpActionResult GetAll([FromBody] ML.Libro libro)
        {
            if (libro == null)
            {
                libro = new ML.Libro();
            }


            ML.Result result = BL.Libro.GetAll(libro);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }


        [HttpPost]
        [Route("Add")]
        public IHttpActionResult Add([FromBody] ML.Libro libro)
        {
            if (libro == null)
            {
                libro = new ML.Libro();
            }


            ML.Result result = BL.Libro.Add(libro);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result);
            }
        }

    }
}
