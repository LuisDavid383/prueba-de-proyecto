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
    /// Interaction logic for ucGestionTorneo.xaml
    /// </summary>
    public partial class ucGestionTorneo : UserControl
    {
        clsGestionTorneoCN ObjTorneo = new clsGestionTorneoCN();
        
        public ucGestionTorneo()
        {
            InitializeComponent();
            mtdCargarTorneosUsuario();
        }

        private void mtdCargarTorneosUsuario()
        {
            int idUsuario = clsDatosUsuario.IDUsuario;

            dgTorneos.ItemsSource = ObjTorneo.mtdListarTorneosActivosPorUsuarioCN(idUsuario).DefaultView;
        }


        private void CrearTorneo_Click(object sender, RoutedEventArgs e)
        {
            wpfCrearTorneo crearTorneo = new wpfCrearTorneo();
            crearTorneo.Show();
            mtdCargarTorneosUsuario();
        }

        private void ModificarTorneo_Click(object sender, RoutedEventArgs e)
        {
            if (dgTorneos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un torneo para modificar.", "Aviso",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView fila = dgTorneos.SelectedItem as DataRowView;

            int idTorneo = Convert.ToInt32(fila["IDTorneos"]);
            string nombre = fila["Nombre"].ToString();
            string descripcion = fila["Descripcion"].ToString();

            // Abre la ventana enviando los datos
            wpfModificarTorneo ventana = new wpfModificarTorneo(idTorneo, nombre, descripcion);
            ventana.ShowDialog();

            // Luego de cerrar la ventana, recarga la lista
            mtdCargarTorneosUsuario();
        }

        private void GestionarParticipantes_Click(object sender, RoutedEventArgs e)
        {
            if (dgTorneos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un torneo para gestionar participantes.", "Aviso",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView fila = dgTorneos.SelectedItem as DataRowView;

            int idTorneo = Convert.ToInt32(fila["IDTorneos"]);
            string nombreTorneo = fila["Nombre"].ToString();

            // Abre la ventana enviando el ID y el nombre
            wpfGestionParticipantes ventana = new wpfGestionParticipantes(idTorneo, nombreTorneo);
            ventana.ShowDialog();
        }

        private void VerFases_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EliminarLogico_Click(object sender, RoutedEventArgs e)
        {
            if (dgTorneos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un torneo para eliminar.", "Aviso",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView fila = dgTorneos.SelectedItem as DataRowView;
            int idTorneo = Convert.ToInt32(fila["IDTorneos"]);
            int idUsuario = clsDatosUsuario.IDUsuario;

            if (MessageBox.Show("¿Seguro que deseas eliminar este torneo?",
                                "Confirmar eliminación",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            ObjTorneo.mtdEliminarTorneoCN(idTorneo, idUsuario);

            MessageBox.Show("Torneo eliminado correctamente.", "Éxito",
                MessageBoxButton.OK, MessageBoxImage.Information);

            mtdCargarTorneosUsuario();
        }
    }
}
