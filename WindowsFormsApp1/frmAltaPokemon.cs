using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Datos;

namespace WindowsFormsApp1
{
    public partial class frmAltaPokemon : Form
    {

        private Pokemon pokemon = null;
        public frmAltaPokemon()
        {
            InitializeComponent();
        }
        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pokemon";
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            //este bloque de codigo me permite traer los tipos y elementos para el desplegable directamente
            //con los valores de la base de datos
            ElementoDatos elementoDatos = new ElementoDatos();
            try
            {
                cbTipo.DataSource = elementoDatos.listar();
                cbTipo.ValueMember = "Id";
                cbTipo.DisplayMember = "Descripcion";
                cbDebilidad.DataSource = elementoDatos.listar();
                cbDebilidad.ValueMember = "Id";
                cbDebilidad.DisplayMember = "Descripcion";

                if (pokemon != null)
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    CargarImagen(pokemon.UrlImagen);
                    cbTipo.SelectedValue = pokemon.Tipo.Id;
                    cbDebilidad.SelectedValue = pokemon.Debilidad.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PokemonDatos datos = new PokemonDatos();
            try
            {
                //verifico si hay o no pokemons
                if (pokemon == null) 
                    pokemon = new Pokemon();
                
                //creo mis objetos para que capturen el contenido de los textbox del formulario
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;
                pokemon.Tipo = (Elemento)cbTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cbDebilidad.SelectedItem;


                if (pokemon.Id != 0)
                {
                    datos.Modificar(pokemon);
                    MessageBox.Show("Pokemon modificado exitosamente");
                }
                else
                {
                    //mandamos la info a la base de datos
                    //agrego - muestro mensaje - cierro
                    datos.Agregar(pokemon);
                    MessageBox.Show("Pokemon agregado correctamente");
                }
                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception)
            {
                pbxPokemon.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }
        }
    }
}
