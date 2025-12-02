using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for wpf_Crear_Cuenta.xaml
    /// </summary>
    public partial class wpf_Crear_Cuenta : Window
    {
        clsLoginCN ObjLogin = new clsLoginCN();

        public wpf_Crear_Cuenta()
        {
            InitializeComponent();

            DataTable dt = ObjLogin.ListarActivos();

            cmbTipoDocumento.ItemsSource = dt.DefaultView;
            cmbTipoDocumento.DisplayMemberPath = "TipoDocumento";
            cmbTipoDocumento.SelectedValuePath = "IDTipoDocumento";
        }

        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            string Nombres = txtNombres.Text;
            string ApellidoPaterno = txtApellidoPaterno.Text;
            string ApellidoMaterno = txtApellidoMaterno.Text;

            int IDTipoDocumento = cmbTipoDocumento.SelectedIndex + 1;
            string Documento = txtDocumento.Text;

            // --- VALIDACIÓN DE FECHA DE NACIMIENTO ---
            DateTime FechaNacimiento = dpFechaNacimiento.SelectedDate ?? DateTime.Now;
            int edad = DateTime.Today.Year - FechaNacimiento.Year;
            if (FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 18)
            {
                MessageBox.Show("La persona no puede ser menor de 18 años.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string Genero = cmbGenero.SelectedIndex == 0 ? "M" : "F";

            string Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text)
                              ? null
                              : txtTelefono.Text;

            // Solo validar si ingresó teléfono
            if (Telefono != null)
            {
                // Ejemplo: solo números, 7 a 15 dígitos
                string patronTelefono = @"^[0-9]{7,15}$";

                if (!Regex.IsMatch(Telefono, patronTelefono))
                {
                    MessageBox.Show("El teléfono ingresado no es válido. Debe contener solo números (7 a 15 dígitos).",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            string Correo = txtCorreo.Text;
            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(Correo, patronCorreo))
            {
                MessageBox.Show("Ingrese un correo electrónico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string NombreUsuario = txtNombreUsuario.Text;
            string Contraseña = pwdContrasena.Password;
            string ConfirmarContraseña = pwdConfirmarContrasena.Password;

            // --- VALIDAR CONTRASEÑAS ---
            if (Contraseña != ConfirmarContraseña)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // --- VALIDAR CAMPOS OBLIGATORIOS (TELÉFONO ES OPCIONAL) ---
            if (string.IsNullOrWhiteSpace(Nombres) ||
                string.IsNullOrWhiteSpace(ApellidoPaterno) ||
                string.IsNullOrWhiteSpace(ApellidoMaterno) ||
                string.IsNullOrWhiteSpace(Documento) ||
                cmbTipoDocumento.SelectedIndex == -1 ||
                cmbGenero.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(Correo) ||
                string.IsNullOrWhiteSpace(NombreUsuario) ||
                string.IsNullOrWhiteSpace(Contraseña) ||
                string.IsNullOrWhiteSpace(ConfirmarContraseña) ||
                dpFechaNacimiento.SelectedDate == null)
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.",
                                "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- LLAMADA A LA CAPA DE NEGOCIO ---
            try
            {
                ObjLogin.mtdRegistrarUsuarioCN(
                    Nombres,
                    ApellidoPaterno,
                    ApellidoMaterno,
                    IDTipoDocumento,
                    Documento,
                    FechaNacimiento,
                    Telefono,        // opcional
                    Correo,
                    Genero,
                    NombreUsuario,
                    Contraseña       // se enviará sin hash (el hash se hace en CN)
                );

                MessageBox.Show("Usuario registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al registrar el usuario:\n{ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void VolverInicio_Click(object sender, RoutedEventArgs e)
        {
            wpf_Inicio_Sesion inicio_Sesion = new wpf_Inicio_Sesion();
            inicio_Sesion.Show();
            this.Close();
        }
    }
}
