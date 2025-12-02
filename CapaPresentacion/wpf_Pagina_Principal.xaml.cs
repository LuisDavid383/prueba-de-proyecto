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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CapaPresentacion
{
    /// <summary>
    /// Interaction logic for wpf_Pagina_Principal.xaml
    /// </summary>
    public partial class wpf_Pagina_Principal : Window
    {
        public wpf_Pagina_Principal()
        {
            InitializeComponent();
        }

        private void LoadContent(object content)
        {
            ContentArea.Content = content;
        }

        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGestionEquipos_Click(object sender, RoutedEventArgs e)
        {
            ucGestionEquipo GestionEquipo = new ucGestionEquipo();
            LoadContent(GestionEquipo);
        }

        private void BtnGestionTorneos_Click(object sender, RoutedEventArgs e)
        {
            ucGestionTorneo GestionTorneo = new ucGestionTorneo();
            LoadContent(GestionTorneo);
        }

        private void BtnInvitaciones_Click(object sender, RoutedEventArgs e)
        {
            int idUsuario = clsDatosUsuario.IDUsuario;

            usInvitaciones Invitaciones = new usInvitaciones(idUsuario);
            LoadContent(Invitaciones);
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            clsDatosUsuario.CerrarSesion();

            MessageBox.Show("Sesión cerrada correctamente.");

            // Redirigir al login
            wpf_Inicio_Sesion ventanaLogin = new wpf_Inicio_Sesion();
            ventanaLogin.Show();

            this.Close();
        }
    }
}
