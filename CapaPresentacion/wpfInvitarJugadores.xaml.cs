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
    /// Interaction logic for wpfInvitarJugadores.xaml
    /// </summary>
    public partial class wpfInvitarJugadores : Window
    {
        clsGestionEquiposCN ObjEquipo = new clsGestionEquiposCN();
        private int IDEquipo;
        public wpfInvitarJugadores(int idEquipo, string nombreEquipo)
        {
            InitializeComponent();
            // Guardar el ID para usarlo en el envío de invitaciones
            this.IDEquipo = idEquipo;

            // Mostrar nombre del equipo en un TextBlock
            txtNombreEquipo.Text = nombreEquipo;

            // Opcional: cargar lista de usuarios disponibles para invitar
            CargarUsuariosDisponibles();
        }

        private void CargarUsuariosDisponibles()
        {
            // Aquí podrías cargar un DataGrid o ListBox con usuarios
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnEnviarInvitacion_Click(object sender, RoutedEventArgs e)
        {
            // Validar que se haya seleccionado un usuario
            if (dgUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un usuario para invitar.", "Aviso");
                return;
            }

            // Obtener fila seleccionada
            DataRowView fila = (DataRowView)dgUsuarios.SelectedItem;

            // Obtener ID del usuario receptor
            int idUsuarioReceptor = Convert.ToInt32(fila["ID"]);

            ObjEquipo.mtdInsertarInvitacionCN(IDEquipo, clsDatosUsuario.IDUsuario, idUsuarioReceptor);
            
            MessageBox.Show("Invitación enviada correctamente.");
        }

        private void CargarUsuarios(string busqueda = null)
        {
            DataTable dt = ObjEquipo.mtdListarUsuariosParaInvitacionCN(busqueda);

            dgUsuarios.ItemsSource = dt.DefaultView;

            // Ocultar columna ID si no deseas mostrarla
            if (dgUsuarios.Columns.Count > 0)
                dgUsuarios.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void TxtBuscarUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            CargarUsuarios(txtBuscarUsuario.Text);
        }
    }
}
