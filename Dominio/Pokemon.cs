using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; //esto me permite modificar el texto de las columnas en mi interface

namespace Dominio
{
    public class Pokemon
    {
        //anotations - permite modificar el texto que figura en las columnas sin romper la db
        [DisplayName("Número")]
        //debe colocarse arriba de cada atributo de mi objeto
        //estos son los datos que voy a traer de mi db.
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; }
        public Elemento Tipo { get; set; }
        public Elemento Debilidad { get; set; }
    }
}