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
    /// Interaction logic for wpfModificarEquipo.xaml
    /// </summary>
    public partial class wpfModificarEquipo : Window
    {
        private int IDEquipo; // Para almacenar el ID del equipo
        private clsGestionEquiposCN ObjEquipo = new clsGestionEquiposCN();

        public wpfModificarEquipo(int idEquipo, string nombre, string descripcion)
        {
            InitializeComponent();

            IDEquipo = idEquipo;
            txtNombre.Text = nombre;
            txtDescripcion.Text = descripcion;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del equipo no puede estar vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción no puede estar vacía.");
                return;
            }

            ObjEquipo.mtdModificarEquipoCN(
                IDEquipo,
                txtNombre.Text,
                txtDescripcion.Text
            );

            MessageBox.Show("Equipo modificado correctamente.");
            this.Close();
        }
    }
}
