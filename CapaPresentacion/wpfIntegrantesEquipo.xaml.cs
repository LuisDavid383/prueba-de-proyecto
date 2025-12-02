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
    /// Interaction logic for wpfIntegrantesEquipo.xaml
    /// </summary>
    public partial class wpfIntegrantesEquipo : Window
    {
        clsGestionEquiposCN ObjEquipo = new clsGestionEquiposCN();

        public wpfIntegrantesEquipo(int idEquipo, string nombreEquipo)
        {
            InitializeComponent();

            // Mostrar nombre del equipo en el TextBlock
            txtNombreEquipo.Text = nombreEquipo;

            // Cargar integrantes
            CargarIntegrantes(idEquipo);
        }

        private void CargarIntegrantes(int idEquipo)
        {
            DataTable dt = ObjEquipo.mtdListarIntegrantesCN(idEquipo);

            // Asignar al DataGrid
            dgIntegrantes.ItemsSource = dt.DefaultView;

            // Ocultar columna ID si no quieres mostrarla
            if (dgIntegrantes.Columns.Count > 0)
                dgIntegrantes.Columns[0].Visibility = Visibility.Collapsed;

            dgIntegrantes.Items.Refresh();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
