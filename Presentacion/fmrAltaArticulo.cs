using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;

namespace Presentacion
{
    public partial class fmrAltaArticulo : Form
    {
        private Articulo articulo = null; 

        private OpenFileDialog archivo = null;


        public fmrAltaArticulo()
        {
            InitializeComponent();
        }

        public fmrAltaArticulo( Articulo articulo)
        {
            InitializeComponent();

            this.articulo=articulo;

            Text = "Modificar Articulo";
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fmrAltaArticulo_Load(object sender, EventArgs e)
        {
            /*---Precargar el Articulo en modificar---*/
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";

            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Marca";

            MarcaNegocio marcaNegocio = new MarcaNegocio();

            if (articulo != null)
            {
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text= articulo.Nombre;
                txtDescripcion.Text= articulo.Descripcion;
                CargarImagen(articulo.ImagenUrl);//Validar si la imagen esta rota
                txtPrecio.Text= articulo.Precio;
                cboCategoria.SelectedValue = articulo.Categoria.Id;
                cboMarca.SelectedValue= articulo.Marca.Id;

                
            }
            try
            {
                /*---desplegables---*/
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboMarca.DataSource = marcaNegocio.listar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {            
            ArticuloNegocio artnegocio = new ArticuloNegocio();
            
            try
            {
                if (articulo == null)
                {
                    articulo= new Articulo();
                }
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Precio = txtPrecio.Text;

                if(articulo.Id!= 0)
                {
                    artnegocio.Modificar(articulo);
                    MessageBox.Show("Modificado correctamente");
                }
                else
                {
                    artnegocio.Agregar(articulo);
                    MessageBox.Show("Articulo agregado correctamente");
                }
                /*--Guardado de imagen solo al agregar con validacion de archivo local---*/
                if (archivo != null && !( txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["carpeta-imagen"] + archivo.SafeFileName);
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
        public void CargarImagen(string imagen)
        {           
            try
            {
                pbxNuevoArt.Load(imagen);
            }
            catch (Exception)
            {
                pbxNuevoArt.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }

        }

        private void btnA_Imagen_Click(object sender, EventArgs e)
        {
            /*---Abrir una ventana de dialogo---*/
            archivo = new OpenFileDialog();

            archivo.Filter= "jpg|*.jpg;| png|*.png";

            if(archivo.ShowDialog()== DialogResult.OK )
            {
                txtUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);                
            }
        }
    }
}
