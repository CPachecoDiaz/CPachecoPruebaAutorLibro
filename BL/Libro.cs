using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Libro
    {
        public static ML.Result GetAll(ML.Libro libroConsulta , int? idAutor)
        {
            ML.Result result = new ML.Result();
            try
            {
                int? añoPublicacion = null;
                if (DateTime.TryParse(libroConsulta.FechaPublicacion, out DateTime fecha))
                {
                    añoPublicacion = fecha.Year;
                }

               // ML.Autor autor = new ML.Autor();
                //autor.IdAutor = idAutor;

                using (DL.CPahcecoPruebaAutorLibroEntities context = new DL.CPahcecoPruebaAutorLibroEntities())
                {
                    var registros = context.LibroGetAll(libroConsulta.Titulo, añoPublicacion, libroConsulta.Editorial.IdEditorial, idAutor).ToList();

                    if(registros.Count > 0 )
                    {
                        result.Objects = new List<object>();

                        foreach ( var registro in registros )
                        {
                            ML.Libro libro = new ML.Libro();
                            libro.Editorial = new ML.Editorial();

                            libro.IdLibro = registro.IdLibro;
                            libro.Titulo = registro.Titulo;
                            libro.FechaPublicacion = registro.FechaPublicacion.ToString("yyyy-MM-dd");
                            libro.Editorial.Nombre = registro.EditorialNombre;

                            result.Objects.Add( libro );

                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay libros registrados";
                    }

                    
                }

                result.Correct = true;
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
