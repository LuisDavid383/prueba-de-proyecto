using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CapaPresentacion
{
    /// <summary>
    /// Interaction logic for wpfCrearTorneo.xaml
    /// </summary>
    public partial class wpfCrearTorneo : Window
    {
        clsGestionTorneoCN ObjTorneo = new clsGestionTorneoCN();
        
        public wpfCrearTorneo()
        {
            InitializeComponent();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Obtener valores
            string nombre = txtNombreTorneo.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            int idCreador = clsDatosUsuario.IDUsuario;

            // Validar campos
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del torneo es obligatorio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Insertar
            try
            {
                ObjTorneo.mtdInsertarTorneoCN(idCreador, nombre, descripcion);

                MessageBox.Show("Torneo creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar campos
                txtNombreTorneo.Text = "";
                txtDescripcion.Text = "";
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el torneo: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
