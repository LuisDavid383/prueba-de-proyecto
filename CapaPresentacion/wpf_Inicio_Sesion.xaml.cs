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
    /// Interaction logic for wpf_Inicio_Sesion.xaml
    /// </summary>
    public partial class wpf_Inicio_Sesion : Window
    {
        clsLoginCN ObjLogin = new clsLoginCN();
        
        public wpf_Inicio_Sesion()
        {
            InitializeComponent();
        }

        private void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(pwdContrasena.Password))
            {
                MessageBox.Show("Por favor, ingresa tu usuario/correo y contraseña.",
                                "Error de Ingreso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataTable dt = ObjLogin.mtdLoginCN(txtUsuario.Text, pwdContrasena.Password);

            if (dt != null)
            {
                // --- GUARDAR DATOS EN LA CLASE GLOBAL ---
                clsDatosUsuario.IDUsuario = Convert.ToInt32(dt.Rows[0]["ID"]);
                clsDatosUsuario.NombreUsuario = dt.Rows[0]["Usuario"].ToString();

                MessageBox.Show("Bienvenido " + clsDatosUsuario.NombreUsuario);

                wpf_Pagina_Principal Pagina_Principal = new wpf_Pagina_Principal();
                Pagina_Principal.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private void CrearCuenta_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Redirigiendo a la página de Creación de Cuenta...", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);

            wpf_Crear_Cuenta Crear_Cuenta = new wpf_Crear_Cuenta();
            Crear_Cuenta.Show();
            this.Close();
        }
    }
}
