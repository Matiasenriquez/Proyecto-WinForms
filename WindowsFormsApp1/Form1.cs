using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio; //añadimos la referncia del otro proyecto para que tome la libreria de mis objetos
using Datos;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //agregamos esta lista para mostrar los elementos en el pictureBox
        private List<Pokemon> listaPokemon;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //aca trabajamos con la lectura a la base de datos, ahora invocamos aca.
                //Objeto que creamos para traer los datos de la db.
                PokemonDatos Datos = new PokemonDatos();
                //data sourse = recibe un origen de datos, luego lo mostramos en el data grid view.
                listaPokemon = Datos.Listar();
                dgvPokemons.DataSource = listaPokemon;
                //oculto la columna de Url imagen dentro del formulario
                dgvPokemons.Columns["UrlImagen"].Visible = false;
                //pedir a la ia que me explique esto
                pbxPokemon.Load(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {
                // Si hay un error de conexión o de SQL, este cartel salta
                MessageBox.Show("Ocurrió un error al cargar la grilla: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            //pedir a la ia que exploque esto
            Pokemon PokemonSeleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            CargarImagen(PokemonSeleccionado.UrlImagen);
        }
        //Funcion para cargar la imagen en el placeholder o picturebox del programa
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //invoco a mi ventana de Alta pokemons y con showdialog permite que no se pueda abrir varias ventanas
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
        }
    }
}