using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class LibroController : Controller
    {
        // GET: Libro
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Libro libro = new ML.Libro();
            libro.Editorial = new ML.Editorial();
            ML.Autor autor = new ML.Autor();
            libro.Titulo = "";
            libro.FechaPublicacion = null;
            libro.Editorial.IdEditorial = (libro.Editorial.IdEditorial == 0) ? null : libro.Editorial.IdEditorial; 
            int? idAutor = null;

            ML.Result result = BL.Libro.GetAll(libro, idAutor.Value);

            if(result.Correct)
            {
                libro.Libros = result.Objects.ToList();

                return View(libro);
            }
            else
            {
                libro.Libros = new List<object>();
                return View(libro);
            }
           
        }
    }
}