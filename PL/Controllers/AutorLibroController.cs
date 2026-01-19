using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace PL.Controllers
{
    public class AutorLibroController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Libro libro = new ML.Libro();
            libro.Editorial = new ML.Editorial();
            libro.Autor = new ML.Autor();

            libro.Titulo = "";
            libro.FechaPublicacion = null;
            libro.Editorial.IdEditorial = null;
            libro.Autor.IdAutor = null;

            ML.Result registros = BL.Libro.GetAll(libro);

            if (registros.Correct)
            {
                libro.Libros = registros.Objects.ToList();
            }
            else
            {
                libro.Libros = new List<object>();
            }

            return View(libro);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Libro libroBuscar)
        {

            if (libroBuscar == null) libroBuscar = new ML.Libro();
            if (libroBuscar.Editorial == null) libroBuscar.Editorial = new ML.Editorial();
            if (libroBuscar.Autor == null) libroBuscar.Autor = new ML.Autor();

            ML.Result registroBusqueda = GetAllRest(libroBuscar);

            if (registroBusqueda.Correct)
            {
                libroBuscar.Libros = registroBusqueda.Objects;
            }
            else
            {
                libroBuscar.Libros = new List<object>();
            }

            return View(libroBuscar);
        }

        [HttpPost]
        public ActionResult Forms(ML.Libro libro)
        {

            ML.Result result = Add(libro);

            if (result.Correct)
            {
                TempData["Success"] = "El libro se agregó correctamente";
            }
            else
            {
                TempData["Error"] = "Ocurrió un error al agregar el libro: " + result.ErrorMessage;
            }

            return RedirectToAction("GetAll");
        }

        //[HttpPost]
        //public ActionResult DeleteByIdEditorial(int idEditorial)
        //{
        //    ML.Result result = DeleteByEditorial(idEditorial);

        //    if (result.Correct)
        //    {
        //        TempData["Success"] = "Libros eliminados correctamente";
        //    }
        //    else
        //    {
        //        TempData["Error"] = "Error al eliminar libros: " + result.ErrorMessage;
        //    }

        //    return RedirectToAction("GetAll");
        //}

        //[HttpPost]
        //public ActionResult DeleteByIdAutor(int idAutor)
        //{
        //    ML.Result result = DeleteByAutor(idAutor);

        //    if (result.Correct)
        //    {
        //        TempData["Success"] = "Libros del autor eliminados correctamente";
        //    }
        //    else
        //    {
        //        TempData["Error"] = "Error al eliminar libros: " + result.ErrorMessage;
        //    }

        //    return RedirectToAction("GetAll");
        //}


        [NonAction]
        public ML.Result GetAllRest(ML.Libro libro)
        {
            ML.Result result = new ML.Result();

            try
            {
                string endpoint = "http://localhost:62135/api/Libro/";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(endpoint);

                    var getTask = client.PostAsJsonAsync("GetAll",libro);
                    getTask.Wait();

                    var resultServicio = getTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        result.Objects = new List<object>();

                        foreach (var item in readTask.Result.Objects)
                        {
                            ML.Libro librolist =
                                Newtonsoft.Json.JsonConvert
                                .DeserializeObject<ML.Libro>(item.ToString());

                            result.Objects.Add(librolist);
                        }

                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;

        }

        [NonAction]
        public ML.Result Add(ML.Libro Libro)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var client = new HttpClient())
                {
                    string endpoint = "http://localhost:62135/api/Libro/";

                    client.BaseAddress = new Uri(endpoint);

                    var postTask = client.PostAsJsonAsync<ML.Libro>("Add", Libro);
                    postTask.Wait();

                    var resultServicio = postTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        //[NonAction]
        //public ML.Result DeleteByEditorial(int idEditorial)
        //{
        //    ML.Result result = new ML.Result();

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string endpoint = "http://localhost:62135/api/Libro/";

        //            client.BaseAddress = new Uri(endpoint);

        //            var postTask = client.DeleteAsync($"DeleteByEditorial/{idEditorial}");
        //            postTask.Wait();

        //            var resultServicio = postTask.Result;

        //            if (resultServicio.IsSuccessStatusCode)
        //            {
        //                result.Correct = true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.ErrorMessage = ex.Message;
        //        result.Ex = ex;
        //    }
        //    return result;
        //}

        //[NonAction]
        //public ML.Result DeleteByAutor(int idAutor)
        //{
        //    ML.Result result = new ML.Result();

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            string endpoint = "http://localhost:62135/api/Libro/";

        //            client.BaseAddress = new Uri(endpoint);

        //            var postTask = client.DeleteAsync($"DeleteByAutor/{idAutor}");
        //            postTask.Wait();

        //            var resultServicio = postTask.Result;

        //            if (resultServicio.IsSuccessStatusCode)
        //            {
        //                result.Correct = true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.ErrorMessage = ex.Message;
        //        result.Ex = ex;
        //    }
        //    return result;
        //}
    }
}