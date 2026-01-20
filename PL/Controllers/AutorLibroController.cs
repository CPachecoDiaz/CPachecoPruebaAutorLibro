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

        [HttpGet]
        public JsonResult GetById(int idLibro)
        {
            ML.Result result = GetByIdRest(idLibro);

            if (result.Correct)
            {
                return Json(result.Object, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

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

                    var getTask = client.PostAsJsonAsync("GetAll", libro);
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

        public ML.Result GetByIdRest(int idLibro)
        {
            ML.Result result = new ML.Result();

            try
            {
                string endpoint = "http://localhost:62135/api/Libro/";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(endpoint);

                    var getTask = client.GetAsync("GetById/" + idLibro);
                    getTask.Wait();

                    var resultServicio = getTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        ML.Libro libroInformacion = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Libro>(readTask.Result.Object.ToString());

                        result.Object = libroInformacion;


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

        [NonAction]
        public ML.Result DeleteByEditorial(int idEditorial)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var client = new HttpClient())
                {
                    string endpoint = "http://localhost:62135/api/Libro/";

                    client.BaseAddress = new Uri(endpoint);

                    var postTask = client.DeleteAsync($"DeleteByEditorial/{idEditorial}");
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

        [NonAction]
        public ML.Result DeleteByAutor(int idAutor)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var client = new HttpClient())
                {
                    string endpoint = "http://localhost:62135/api/Libro/";

                    client.BaseAddress = new Uri(endpoint);

                    var postTask = client.DeleteAsync($"DeleteByAutor/{idAutor}");
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
    }
}