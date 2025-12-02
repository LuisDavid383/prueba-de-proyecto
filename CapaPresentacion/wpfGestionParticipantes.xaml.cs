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
    /// Interaction logic for wpfGestionParticipantes.xaml
    /// </summary>
    public partial class wpfGestionParticipantes : Window
    {
        clsGestionTorneoCN ObjTorneo = new clsGestionTorneoCN();
        
        public int IDTorneo { get; set; }
        public string NombreTorneo { get; set; }

        public wpfGestionParticipantes(int idTorneo, string nombreTorneo)
        {
            InitializeComponent();

            IDTorneo = idTorneo;
            NombreTorneo = nombreTorneo;

            // Mostrar en el título
            txtTituloTorneo.Text = $"GESTIÓN DEL TORNEO: {NombreTorneo}";

            // Cargar equipos participantes del torneo
            //CargarParticipantes();
            //CargarCombosEquipos();
        }

        private void BtnBuscarEquipo_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtBuscarEquipo.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese un nombre para buscar equipos.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataTable dt = ObjTorneo.mtdBuscarEquipoPorNombreCN(nombre);

            dgParticipantes.ItemsSource = dt.DefaultView;
        }


        private void BtnInvitarEquipo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRetirarEquipo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGenerarEnfrentamientos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConfirmarCalendario_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAgregarPartidoManual_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEliminarEnfrentamiento_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
