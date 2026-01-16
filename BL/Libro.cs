using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Libro
    {
        public static ML.Result GetAll(ML.Libro libroConsulta)
        {
            ML.Result result = new ML.Result();
            try
            {

                int? anioPublicacion = libroConsulta.FechaPublicacion?.Year;
               

                using (DL.CPahcecoPruebaAutorLibroEntities context = new DL.CPahcecoPruebaAutorLibroEntities())
                {
                    var registros = context.LibroGetAll(libroConsulta.Titulo, anioPublicacion, libroConsulta.Editorial.IdEditorial, libroConsulta.Autor.IdAutor).ToList();

                    if (registros.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var registro in registros)
                        {
                            ML.Libro libro = new ML.Libro
                            {
                                IdLibro = registro.IdLibro,
                                Titulo = registro.Titulo,
                                FechaPublicacion = registro.FechaPublicacion,
                                Editorial = new ML.Editorial
                                {
                                    IdEditorial = registro.IdEditorial,
                                    Nombre = registro.EditorialNombre
                                },
                                 Autor = new ML.Autor
                                 {
                                     IdAutor = registro.IdAutor,
                                     Nombre = registro.AutorNombre,
                                     Apellido = registro.Apellido
                                 }
                            };

                            result.Objects.Add(libro);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay libros registrados";
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

        public static ML.Result Add(ML.Libro libro)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var context = new DL.CPahcecoPruebaAutorLibroEntities())
                {
                   int libroAdd = context.LibroAdd(libro.Titulo,libro.FechaPublicacion,libro.Editorial.IdEditorial,libro.Autor.IdAutor);

                    if (libroAdd == 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se añadio correctamente";
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

        public static ML.Result DeleteByEditorial(int idEditorial)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var context = new DL.CPahcecoPruebaAutorLibroEntities())
                {
                    int libroDelete = context.DeleteByEditorial(idEditorial);

                    if (libroDelete > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se elimino correctamente";
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

        public static ML.Result DeleteByAutor(int idAutor)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (var context = new DL.CPahcecoPruebaAutorLibroEntities())
                {
                    int libroDelete = context.DeleteByAutor(idAutor);

                    if (libroDelete > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se elimino correctamente";
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
