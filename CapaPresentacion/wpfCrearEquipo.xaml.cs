using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for wpfCrearEquipo.xaml
    /// </summary>
    public partial class wpfCrearEquipo : Window
    {
        clsGestionEquiposCN ObjEquipo = new clsGestionEquiposCN();
        
        public wpfCrearEquipo()
        {
            InitializeComponent();

            DataTable dt = ObjEquipo.mtdCListarDeportesActivosCN();

            cmbDeporte.ItemsSource = dt.DefaultView;
            cmbDeporte.DisplayMemberPath = "Deporte";
            cmbDeporte.SelectedValuePath = "IDDeporte";
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // VALIDAR NOMBRE
            if (string.IsNullOrWhiteSpace(txtNombreEquipo.Text))
            {
                MessageBox.Show("El nombre del equipo no puede estar vacío.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // VALIDAR COMBOBOX
            if (cmbDeporte.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un deporte.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // OBTENER ID DEPORTE
            int idDeporte = Convert.ToInt32(cmbDeporte.SelectedValue);

            // LLAMAR A CAPA NEGOCIO
            ObjEquipo.mtdInsertarEquipoCN(
                clsDatosUsuario.IDUsuario,
                txtNombreEquipo.Text.Trim(),
                idDeporte,
                txtDescripcion.Text.Trim()
            );

            MessageBox.Show("Equipo registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}
