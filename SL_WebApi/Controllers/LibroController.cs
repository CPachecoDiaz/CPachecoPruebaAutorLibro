using System;
using System.Collections.Generic;
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

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            ML.Libro libro = new ML.Libro();
            libro.Editorial = new ML.Editorial();
            ML.Autor autor = new ML.Autor();
            libro.Titulo = "";
            libro.FechaPublicacion = null;
            libro.Editorial.IdEditorial = (libro.Editorial.IdEditorial == 0) ? null : libro.Editorial.IdEditorial;
            int? idAutor = null;

            ML.Result result = BL.Libro.GetAll(libro, idAutor);

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
