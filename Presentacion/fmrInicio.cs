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
using Negocio;



namespace Presentacion
{
    public partial class fmrArticulos : Form
    {
        private List<Articulo> listaArticulo;

        private Articulo articulo = null;
        
        public fmrArticulos()
        {
            InitializeComponent();
            ActualizarGrilla();
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            fmrAltaArticulo alta= new fmrAltaArticulo();

            alta.ShowDialog();

            ActualizarGrilla();
        }

        

        private void fmrArticulos_Load(object sender, EventArgs e)
        {
            ActualizarGrilla();
            /*---Precargar en el load el desplegable de busqueda avanzada---*/
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripciòn");           
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            /*---Cambiar la imagen al hacer click---*/
            Articulo artSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            CargarImagen(artSeleccionado.ImagenUrl);
        }

        public void CargarImagen(string imagen)
        {
            /*---Carga en caso de que la imagen no este disponible---*/
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbxArticulo.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;

            seleccionado=(Articulo) dgvArticulos.CurrentRow.DataBoundItem;

            fmrAltaArticulo modificar= new fmrAltaArticulo(seleccionado);
            modificar.ShowDialog();
            ActualizarGrilla();
        }

        private void ActualizarGrilla()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                OultarColumnaImagen();
                OultarColumnaId();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OultarColumnaImagen()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }
        private void OultarColumnaId()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio eNegocio= new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Deseas Eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    eNegocio.Eliminar(seleccionado.Id);
                    ActualizarGrilla();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtBusquedaRapida_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> ListaFiltrada = new List<Articulo>();
            string filtro = txtBusquedaRapida.Text;

            if(filtro.Length >= 3)
            {
                ListaFiltrada= listaArticulo.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                ListaFiltrada=listaArticulo;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource= ListaFiltrada;
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opciones= cboCampo.SelectedItem.ToString();
                        
            if (opciones == "Nombre")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con:");
                cboCriterio.Items.Add("Termina con:");
                cboCriterio.Items.Add("Contiene");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con:");
                cboCriterio.Items.Add("Termina con:");
                cboCriterio.Items.Add("Contiene");
            }
        }
        private bool ValidarFiltro()
        {
            if(cboCampo.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el campo para filtrar");
                return true;
            }
            if(cboCriterio.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione el criterio para filtrar");
                return true;
            }
            return false;
        }

        private void btnBuscar_Click(object sender, EventArgs e) 
        {
            ArticuloNegocio negocio= new ArticuloNegocio();
            try
            {
               if(ValidarFiltro())
                {
                    return;
                }
                string campo = cboCampo.SelectedItem.ToString();

                string criterio = cboCriterio.SelectedItem.ToString();

                string filtro = txtBusquedaParametros.Text;

                dgvArticulos.DataSource= negocio.filtar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

       
    }
}
