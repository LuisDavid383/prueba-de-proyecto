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
    /// Interaction logic for usInvitaciones.xaml
    /// </summary>
    public partial class usInvitaciones : UserControl
    {
        private int _idUsuario;
        clsGestionEquiposCN ObjEquipo = new clsGestionEquiposCN();

        public usInvitaciones(int idUsuario)
        {
            InitializeComponent();
            _idUsuario = idUsuario;
            CargarInvitacionesEquipos();
        }

        private void CargarInvitacionesEquipos()
        {
            dgInvitacionesEquipo.ItemsSource = ObjEquipo.mtdListarInvitacionesCN(_idUsuario).DefaultView;
        }

        private void BtnAceptarEquipo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar selección
                if (dgInvitacionesEquipo.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione una invitación.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener fila seleccionada
                if (dgInvitacionesEquipo.SelectedItem is not DataRowView fila)
                {
                    MessageBox.Show("Hubo un problema al obtener los datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int idInvitacion = Convert.ToInt32(fila["IDInvitacion"]);
                int idUsuarioReceptor = clsDatosUsuario.IDUsuario;

                // Registrar aceptación
                ObjEquipo.mtdAceptarInvitacionCN(idInvitacion, idUsuarioReceptor);

                MessageBox.Show("Invitación aceptada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar la tabla
                CargarInvitacionesEquipos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnRechazarEquipo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar selección
                if (dgInvitacionesEquipo.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione una invitación.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener fila seleccionada
                if (dgInvitacionesEquipo.SelectedItem is not DataRowView fila)
                {
                    MessageBox.Show("Hubo un problema al obtener los datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int idInvitacion = Convert.ToInt32(fila["IDInvitacion"]);
                int idUsuarioReceptor = clsDatosUsuario.IDUsuario;

                // Registrar rechazo
                ObjEquipo.mtdRechazarInvitacionCN(idInvitacion, idUsuarioReceptor);

                MessageBox.Show("Invitación rechazada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Recargar la tabla
                CargarInvitacionesEquipos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnAceptarTorneo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRechazarTorneo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
