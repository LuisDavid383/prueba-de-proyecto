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
    /// Interaction logic for wpfModificarTorneo.xaml
    /// </summary>
    public partial class wpfModificarTorneo : Window
    {
        int IDTorneo;
        clsGestionTorneoCN ObjTorneo = new clsGestionTorneoCN();

        public wpfModificarTorneo(int idTorneo, string nombre, string descripcion)
        {
            InitializeComponent();

            IDTorneo = idTorneo;
            txtNombreTorneo.Text = nombre;
            txtDescripcion.Text = descripcion;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            ObjTorneo.mtdModificarTorneoCN(
            IDTorneo,
            clsDatosUsuario.IDUsuario,
            txtNombreTorneo.Text,
            txtDescripcion.Text
        );

            MessageBox.Show("Torneo actualizado correctamente.");
            this.Close();
        }
    }
}
