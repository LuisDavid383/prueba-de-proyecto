using CapaDatos;
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
    /// Interaction logic for wpfVerPartidos.xaml
    /// </summary>
    public partial class wpfVerPartidos : Window
    {
        private int _idTorneo; // ID del torneo que recibes para listar los partidos

        public wpfVerPartidos(int idTorneo, string nombreTorneo)
        {
            InitializeComponent();
            _idTorneo = idTorneo;

            // Cambiar el título dinámicamente
            txtTituloTorneo.Text = $"GESTIÓN DEL TORNEO: {nombreTorneo}";

            // Cargar los enfrentamientos
            CargarEnfrentamientos();
        }

        private void CargarEnfrentamientos()
        {
            try
            {
                List<Enfrentamiento> lista = clsEnfrentamientosCN.ListarPorTorneo(_idTorneo);
                dgEnfrentamientos.ItemsSource = lista; // Asignamos la lista al DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los enfrentamientos: " + ex.Message);
            }
        }
    }
}
