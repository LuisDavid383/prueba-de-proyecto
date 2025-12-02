using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class clsGestionTorneoCD
    {
        public void mtdInsertarTorneoCD(int idCreador, string nombre, string descripcion)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_Torneo_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDCreador", idCreador);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable mtdListarTorneosActivosPorUsuarioCD(int idUsuario)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_Torneos_ListarActivosPorUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public void mtdModificarTorneoCD(int idTorneo, int idUsuario, string nombre, string descripcion)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ModificarTorneo", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDTorneos", idTorneo);
                    cmd.Parameters.AddWithValue("@IDCreador", idUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void mtdEliminarTorneoCD(int idTorneo, int idUsuario)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_Torneo_EliminarLogico", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDTorneo", idTorneo);
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable mtdBuscarEquipoPorNombreCD(string nombre)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("spBuscarEquiposPorNombre", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cn.Open();   // <- necesario
                da.Fill(dt); // <- tu línea que está fallando
            }

            return dt;
        }

        public string InvitarEquipoATorneo(int idTorneo, int idEquipo, int idUsuarioEmisor)
        {
            try
            {
                using (SqlConnection conn = clsConexion.mtdObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spInvitarEquipoATorneo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDTorneo", idTorneo);
                    cmd.Parameters.AddWithValue("@IDEquipo", idEquipo);
                    cmd.Parameters.AddWithValue("@IDUsuarioEmisor", idUsuarioEmisor);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataTable ListarInvitaciones(int idUsuario)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = clsConexion.mtdObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spListarInvitacionesDeEquipos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar invitaciones: " + ex.Message);
            }

            return dt;
        }

        public void AceptarInvitacion(int idParticipacion)
        {
            using (SqlConnection conn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spAceptarInvitacionEquipo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDParticipacion", idParticipacion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RechazarInvitacion(int idParticipacion)
        {
            using (SqlConnection conn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spRechazarInvitacionEquipo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDParticipacion", idParticipacion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ListarEquiposAceptados(int idTorneo)
        {
            using (SqlConnection conn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spListarEquiposAceptados", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDTorneo", idTorneo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool CrearEnfrentamiento(int idTorneo, int idLocal, int idVisitante, DateTime fecha)
        {
            using (SqlConnection conn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spCrearEnfrentamiento", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDTorneo", idTorneo);
                cmd.Parameters.AddWithValue("@IDEquipoLocal", idLocal);
                cmd.Parameters.AddWithValue("@IDEquipoVisitante", idVisitante);
                cmd.Parameters.AddWithValue("@FechaPartido", fecha);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public List<Enfrentamiento> ListarPorTorneo(int idTorneo)
        {
            List<Enfrentamiento> lista = new List<Enfrentamiento>();

            using (SqlConnection con = clsConexion.mtdObtenerConexion())
            {
                string query = @"
        SELECT e.IDEnfrentamiento,
               e.FechaPartido,
               e.EstadoPartido,

               el.Nombre AS LocalNombre,
               ev.Nombre AS VisitanteNombre

        FROM tbEnfrentamientos e
        INNER JOIN tbEquipo el ON el.IDEquipo = e.IDEquipoLocal
        INNER JOIN tbEquipo ev ON ev.IDEquipo = e.IDEquipoVisitante
        WHERE e.IDTorneo = @idTorneo
        ORDER BY FechaPartido";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idTorneo", idTorneo);

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Enfrentamiento
                        {
                            IDEnfrentamiento = Convert.ToInt32(dr["IDEnfrentamiento"]),
                            LocalNombre = dr["LocalNombre"].ToString(),
                            VisitanteNombre = dr["VisitanteNombre"].ToString(),
                            FechaPartido = Convert.ToDateTime(dr["FechaPartido"]),
                            EstadoPartido = dr["EstadoPartido"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

    }
}
