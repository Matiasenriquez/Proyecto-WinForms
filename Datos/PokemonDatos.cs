using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //1er paso: añadir libreria para poder acceder a los datos de la db
using Dominio; //luego de añadir la referencia de mi otro proyecto, agrego desde aca

namespace Datos
{
    //Conexiones a bases de datos mediante un metodo para las clases
    public class PokemonDatos
    {
        public List<Pokemon> Listar()
        {
           List<Pokemon> lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection(); //2do paso, crear objeto para crear la conexion a la db.
            SqlCommand comando = new SqlCommand(); //3do paso, objeto que me permite realizar las acciones a la db.
            SqlDataReader lector; //4to paso, objeto para poder leer los datos que se crean o traen de la db, a traves de SqlCommand.

            try
            {
                //5to paso, configurar la cadena de conexion. Usamos el objeto "conexion" para conectar con la db local o remota mediante el nombre del motor de base de dato.
                //server o data base = nombre de la base de datos local o remota
                //database = nombre de la base de datos que quiero utilizar de la coleccion
                //integrated security = si es local, true por defecto, si es autenticacion de sql server, van las credenciales para unirse a una db con usuario y contraseña.
                conexion.ConnectionString = "server=(localdb)\\MSSQLLocalDB; database=POKEDEX_DB; integrated security=true";
                //6to paso: creamos la sentencia sql que quiero ejecutar y de que tipo: texto, procedimiento almacenado o enlace directo con la tabla.
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion as tipo, D.Descripcion as Debilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo and D.Id = P.IdDebilidad";
                //donde se va a ejecutar esa conexion.
                comando.Connection = conexion;

                //7mo paso, abrir la conexion.
                conexion.Open();
                //8vo paso, leo la conexion en la variable lector.
                lector = comando.ExecuteReader();

                //recorremos la tabla de la db con un while. Recorre y agrega el contenido de cada fila y columna en una lista, despues retorna una lista de objetos.    
                while (lector.Read())
                {
                    Pokemon auxiliar = new Pokemon();
                    auxiliar.Numero = lector.GetInt32(0);
                    auxiliar.Nombre = (string)lector["Nombre"];
                    auxiliar.Descripcion = (string)lector["Descripcion"];
                    //por cada columna nueva de la db que quiera, debo agregar otro objeto.
                    auxiliar.UrlImagen = (string)lector["UrlImagen"];
                    auxiliar.Tipo = new Elemento();
                    auxiliar.Tipo.Descripcion = (string)lector["Tipo"];
                    auxiliar.Debilidad = new Elemento();
                    auxiliar.Debilidad.Descripcion = (string)lector["Debilidad"];

                    //9no paso, agrego toda la informacion del recorrido en una lista
                    lista.Add(auxiliar);
                }
                //10mo psao, cierro la conexion y devuelvo la informacion.
                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Pokemon nuevo)
        {
            //logica para agregar los pokemons a la base de datos.
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetConsulta("INSERT INTO POKEMONS (Numero, Nombre, Descripcion, Activo) VALUES (" + nuevo.Numero +  ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1)");
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
        public void Modificar(Pokemon modificar)
        {

        }

    }
}
