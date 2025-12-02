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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CapaPresentacion
{
    /// <summary>
    /// Interaction logic for ucGestionEquipo.xaml
    /// </summary>
    public partial class ucGestionEquipo : UserControl
    {
        clsGestionEquiposCN ObjEquipo = new clsGestionEquiposCN();

        public ucGestionEquipo()
        {
            InitializeComponent();
            mtdCargarEquipos();
        }

        private void mtdCargarEquipos()
        {
            DataTable dt = ObjEquipo.mtdListarEquiposActivosCN(clsDatosUsuario.IDUsuario);

            dgEquipos.ItemsSource = dt.DefaultView;

            if (dgEquipos.Columns.Count > 0)
                dgEquipos.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void CrearEquipo_Click(object sender, RoutedEventArgs e)
        {
            wpfCrearEquipo crearEquipo = new wpfCrearEquipo();
            crearEquipo.Show();
            mtdCargarEquipos();
        }

        private void ModificarEquipo_Click(object sender, RoutedEventArgs e)
        {
            if (dgEquipos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un equipo.", "Aviso");
                return;
            }

            DataRowView fila = (DataRowView)dgEquipos.SelectedItem;
            int idEquipo = Convert.ToInt32(fila["IDEquipo"]);
            string nombre = fila["Nombre"].ToString();
            string descripcion = fila["Descripcion"].ToString();

            // Abrir el formulario de modificación
            wpfModificarEquipo ventana = new wpfModificarEquipo(idEquipo, nombre, descripcion);
            ventana.ShowDialog();

            // Refrescar DataGrid al cerrar la ventana
            mtdCargarEquipos();
        }

        private void InvitarJugadores_Click(object sender, RoutedEventArgs e)
        {
            // Validar que haya un equipo seleccionado
            if (dgEquipos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un equipo.", "Aviso");
                return;
            }

            // Obtener fila seleccionada
            DataRowView fila = (DataRowView)dgEquipos.SelectedItem;

            // Obtener ID del equipo y nombre del equipo
            int idEquipo = Convert.ToInt32(fila["IDEquipo"]);
            string nombreEquipo = fila["Nombre"].ToString();

            // Abrir el formulario de invitaciones y pasar el IDEquipo y nombre
            wpfInvitarJugadores ventanaInvitar = new wpfInvitarJugadores(idEquipo, nombreEquipo);
            ventanaInvitar.ShowDialog();
        }

        private void VerIntegrantes_Click(object sender, RoutedEventArgs e)
        {
            if (dgEquipos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un equipo.", "Aviso");
                return;
            }

            DataRowView fila = (DataRowView)dgEquipos.SelectedItem;
            int idEquipo = Convert.ToInt32(fila["IDEquipo"]);
            string nombreEquipo = fila["Nombre"].ToString();

            // Abrir ventana de integrantes
            wpfIntegrantesEquipo ventana = new wpfIntegrantesEquipo(idEquipo, nombreEquipo);
            ventana.ShowDialog();
        }

        private void EliminarLogico_Click(object sender, RoutedEventArgs e)
        {
            if (dgEquipos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un equipo.", "Aviso");
                return;
            }

            DataRowView fila = (DataRowView)dgEquipos.SelectedItem;
            int idEquipo = Convert.ToInt32(fila["IDEquipo"]);

            MessageBoxResult result = MessageBox.Show(
                "¿Seguro que desea eliminar este equipo?",
                "Confirmación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                ObjEquipo.mtdEliminarLogicoEquipoCN(idEquipo);

                MessageBox.Show("Equipo eliminado correctamente.");

                // Refrescar DataGrid
                mtdCargarEquipos();
            }
        }

        private void CargarEquipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //mtdCargarEquipos();
        }

        private void CargarEquipos_TextChanged(object sender, TextChangedEventArgs e)
        {
            //mtdCargarEquipos();
        }

        private void CargarEquipos_Checked(object sender, RoutedEventArgs e)
        {
            //mtdCargarEquipos();
        }
    }
}
