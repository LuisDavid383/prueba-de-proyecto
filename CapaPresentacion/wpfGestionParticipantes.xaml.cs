using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<enfrentamiento> listaEnfrentamientos = new ObservableCollection<enfrentamiento>();

        public int IDTorneo { get; set; }
        public string NombreTorneo { get; set; }

        public wpfGestionParticipantes(int idTorneo, string nombreTorneo)
        {
            InitializeComponent();

            dgEnfrentamientos.ItemsSource = listaEnfrentamientos;

            IDTorneo = idTorneo;
            NombreTorneo = nombreTorneo;

            // Mostrar en el título
            txtTituloTorneo.Text = $"GESTIÓN DEL TORNEO: {NombreTorneo}";
            CargarCombosEquipos();
        }

        private void CargarCombosEquipos()
        {
            DataTable dtEquipos = ObjTorneo.ObtenerEquiposAceptados(IDTorneo);

            cmbLocal.ItemsSource = dtEquipos.DefaultView;
            cmbLocal.DisplayMemberPath = "NombreEquipo";
            cmbLocal.SelectedValuePath = "IDEquipo";

            cmbVisitante.ItemsSource = dtEquipos.DefaultView;
            cmbVisitante.DisplayMemberPath = "NombreEquipo";
            cmbVisitante.SelectedValuePath = "IDEquipo";
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
            if (dgParticipantes.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un equipo.");
                return;
            }

            DataRowView fila = (DataRowView)dgParticipantes.SelectedItem;

            int idEquipo = (int)fila["IDEquipo"];
            int idTorneo = IDTorneo;
            int idUsuarioEmisor = clsDatosUsuario.IDUsuario;

            string respuesta = ObjTorneo.InvitarEquipo(idTorneo, idEquipo, idUsuarioEmisor);

            if (respuesta == "OK")
                MessageBox.Show("Invitación enviada correctamente.");
            else
                MessageBox.Show("Error: " + respuesta);
        }


        private void BtnRetirarEquipo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGenerarEnfrentamientos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConfirmarCalendario_Click(object sender, RoutedEventArgs e)
        {
            if (dgEnfrentamientos.Items.Count == 0)
            {
                MessageBox.Show("No hay enfrentamientos para guardar.");
                return;
            }

            int idTorneo = IDTorneo;
            bool todoOK = true;

            foreach (var item in dgEnfrentamientos.Items)
            {
                enfrentamiento enf = item as enfrentamiento;
                if (enf == null) continue;

                bool resultado = ObjTorneo.RegistrarEnfrentamiento(
                    idTorneo,
                    enf.IDLocal,
                    enf.IDVisitante,
                    enf.FechaPartido
                );

                if (!resultado)
                    todoOK = false;
            }

            if (todoOK)
            {
                MessageBox.Show("Enfrentamientos guardados correctamente.");
                listaEnfrentamientos.Clear();
                dgEnfrentamientos.ItemsSource = null;
            }
            else
            {
                MessageBox.Show("Algunos enfrentamientos no se pudieron guardar.");
            }
        }


        private void BtnAgregarPartidoManual_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones
            if (cmbLocal.SelectedValue == null || cmbVisitante.SelectedValue == null)
            {
                MessageBox.Show("Seleccione ambos equipos.");
                return;
            }

            if (cmbLocal.SelectedValue.ToString() == cmbVisitante.SelectedValue.ToString())
            {
                MessageBox.Show("Un equipo no puede jugar contra sí mismo.");
                return;
            }

            if (!dpFechaPartido.SelectedDate.HasValue)
            {
                MessageBox.Show("Seleccione una fecha.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHoraPartido.Text))
            {
                MessageBox.Show("Ingrese una hora válida.");
                return;
            }

            DateTime fechaHora = dpFechaPartido.SelectedDate.Value
                .Add(TimeSpan.Parse(txtHoraPartido.Text));

            // Crear enfrentamiento
            var enfrentamiento = new enfrentamiento()
            {
                IDEnfrentamiento = listaEnfrentamientos.Count + 1,
                IDLocal = (int)cmbLocal.SelectedValue,
                IDVisitante = (int)cmbVisitante.SelectedValue,

                LocalNombre = cmbLocal.Text,
                VisitanteNombre = cmbVisitante.Text,

                FechaPartido = fechaHora,
                Sede = "Por definir",
                EstadoPartido = "Pendiente"
            };


            // AGREGAR A LA LISTA (NO ROWS.ADD)
            listaEnfrentamientos.Add(enfrentamiento);

            MessageBox.Show("Partido agregado a la tabla.");
        }


        private void BtnEliminarEnfrentamiento_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
