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
using System.Runtime.CompilerServices;

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
            Cargar();
            //Items del menu desplegable Campo
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
        }

        private void Cargar()
        {
            try
            {
                //aca trabajamos con la lectura a la base de datos, ahora invocamos aca.
                //Objeto que creamos para traer los datos de la db.
                PokemonDatos Datos = new PokemonDatos();
                //data sourse = recibe un origen de datos, luego lo mostramos en el data grid view.
                listaPokemon = Datos.Listar();
                dgvPokemons.DataSource = listaPokemon;
                OcultarColumnas();
                pbxPokemon.Load(listaPokemon[0].UrlImagen);
            }
            catch (Exception ex)
            {
                // Si hay un error de conexión o de SQL, este cartel salta
                MessageBox.Show("Ocurrió un error al cargar la grilla: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OcultarColumnas()
        {
            //oculto la columna de Url imagen dentro del formulario
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPokemons.CurrentRow != null)
            {
                Pokemon PokemonSeleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                CargarImagen(PokemonSeleccionado.UrlImagen);
            }           
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
            //llamo a la funcion de cargar para que refresque la informacion del formulario.
            Cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //pokemon seleccionado para modificar
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            //invoco a mi ventana de modificar pokemons y con showdialog permite que no se pueda abrir varias ventanas
            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            //llamo a la funcion de cargar para que refresque la informacion del formulario.
            Cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            Eliminar(true); 
        }
        private void Eliminar (bool logico = false)
        {
            PokemonDatos datos = new PokemonDatos();
            Pokemon seleccionado;
            try
            {
                DialogResult Respuesta = MessageBox.Show("¿Borrar fila seleccionada?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Respuesta == DialogResult.Yes)
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    if (logico)
                    {
                        datos.Baja(seleccionado.Id);
                    } 
                    datos.Eliminar(seleccionado.Id);
                    //actualizamos la grilla con el metodo cargar luego de eliminar
                    Cargar();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        //validaciones de campos en formulario
        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar.");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Número")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro para numéricos.");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo números para filtrar por favor.");
                    return true;
                }
            }
           
            return false;
        }
        //validaciones de tipos de datos en campos
        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!char.IsNumber(caracter))
                {
                    return false;
                }
            }  
            return true;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            PokemonDatos datos = new PokemonDatos();
            try
            {
                if (validarFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvPokemons.DataSource = datos.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }            
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
         
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            //utlizamos el evento textchanged para que busque de manera instantanea segun lo que ingresa el usuario
            List<Pokemon> ListaFiltrada = new List<Pokemon>();

            string filtro = txtFiltro.Text;
            if (filtro.Length >= 2)
            {
                //implementamos la función lambda en el find para encontrar un elemento comparando al contenido del text box
                //to upper pasa todo a mayuscula la busqueda que hace el usuario para que compare todo por igual
                ListaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                ListaFiltrada = listaPokemon;
            }


            //primero inicializamos el origen de los datos en nulo para refrescar la información
            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = ListaFiltrada;
            OcultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Las opciones del menu Criterio se cargan en base al item seleccionado en el menú Campo
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Número")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}