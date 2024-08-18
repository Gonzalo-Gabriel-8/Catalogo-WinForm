using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion as Descripcion_Categoria, M.Descripcion as Descripcion_Marca, A.Id From ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria AND M.Id = A.IdMarca");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl")));
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Convert.ToString(datos.Lector["Precio"]);
                    aux.Descripcion = Convert.ToString(datos.Lector["Descripcion"]);

                    /*---Traer Categoria---*/
                    aux.Categoria = new Categoria();                    
                    aux.Categoria.Descripcion = (string)datos.Lector["Descripcion_Categoria"];

                    /*---Traer Marca---*/
                    aux.Marca = new Marca();                    
                    aux.Marca.Descripcion = (string)datos.Lector["Descripcion_Marca"];

                    lista.Add(aux);
                }
                datos.CerrarConexion();
                return lista;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos= new AccesoDatos();
            try
            {
                datos.SetearConsulta("insert into ARTICULOS(codigo, nombre, descripcion, IdMarca, IdCategoria, ImagenUrl,Precio) values (@Codigo, @Nombre, @desc, @IdMarca,@IdCategoria,@UrlImagen,@Precio)");
                datos.SetearParametros("@Codigo", nuevo.Codigo);
                datos.SetearParametros("@Nombre", nuevo.Nombre);
                datos.SetearParametros("@desc", nuevo.Descripcion);
                datos.SetearParametros("@IdMarca", nuevo.Marca.Id);
                datos.SetearParametros("@IdCategoria", nuevo.Categoria.Id);
                datos.SetearParametros("@UrlImagen", nuevo.ImagenUrl);
                datos.SetearParametros("@Precio", nuevo.Precio);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Articulo mod)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(" update ARTICULOS set Codigo= @cod, Nombre= @nombre, Descripcion= @descripcion, IdMarca= @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @imagenUrl, Precio = @precio where Id = @Id");
                datos.SetearParametros("@cod", mod.Codigo);
                datos.SetearParametros("@nombre", mod.Nombre);
                datos.SetearParametros("@descripcion", mod.Descripcion);
                datos.SetearParametros("@IdMarca", mod.Marca.Id);
                datos.SetearParametros("IdCategoria", mod.Categoria.Id);
                datos.SetearParametros("@imagenUrl", mod.ImagenUrl);
                datos.SetearParametros("@precio", mod.Precio);
                datos.SetearParametros("@Id", mod.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Eliminar( int id)
        {
            try
            {
                AccesoDatos eDatos = new AccesoDatos();
                eDatos.SetearConsulta("delete from ARTICULOS where id = @id");
                eDatos.SetearParametros("@id", id);
                eDatos.EjecutarAccion();
            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }

        public List<Articulo> filtar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();

            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion as Descripcion_Categoria, M.Descripcion as Descripcion_Marca, A.Id From ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria AND M.Id = A.IdMarca AND";

               
                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con: ":
                            consulta += " Nombre like '" + filtro + "%'";
                            break;

                        case "Termina con: ":
                            consulta += " Nombre like '%" + filtro + "'";
                            break;

                        default:
                            consulta += " Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if(campo== "A.Descripcion")
                {
                    switch (criterio)
                    {
                        case "Comienza con: ":
                            consulta += " A.Descripcion like '" + filtro + "%'";
                            break;

                        case "Termina con: ":
                            consulta += " A.Descripcion like '%" + filtro + "'"; 
                            break;

                        default:
                            consulta += " A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con: ":
                            consulta += " Descripcion like '" + filtro + "%'";
                            break;

                        case "Termina con: ":
                            consulta += " A.Descripcion like '%" + filtro + "'";
                            break;

                        default:
                            consulta += " A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.SetearConsulta( consulta );
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))) ;
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Convert.ToString(datos.Lector["Precio"]);
                    aux.Descripcion = Convert.ToString(datos.Lector["Descripcion"]);

                    /*---Traer Categoria---*/
                    aux.Categoria = new Categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Descripcion_Categoria"];

                    /*---Traer Marca---*/
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Descripcion_Marca"];

                    lista.Add(aux);
                }
                return lista;

            }
                
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
