using System;
using System.Data;
using System.Data.SqlClient;

namespace academicaAlumnos
{
    public class Conexion
    {
        
        private string cadenaConexion = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SistemaUsuarios;Integrated Security=True;Encrypt=True";
        
        private SqlConnection conexion;
        private SqlDataAdapter adaptador;
        private DataSet ds = new DataSet();

        
        public DataSet obtenerDatos()
        {
            ds.Clear();
            conexion = new SqlConnection(cadenaConexion);
            string query = @"SELECT 
                                idAlumno    AS id,
                                codigo      AS codigo,
                                nombre      AS nombre,
                                direccion   AS direccion,
                                telefono    AS telefono 
                             FROM alumnos 
                             ORDER BY nombre";

            adaptador = new SqlDataAdapter(query, conexion);
            adaptador.Fill(ds, "alumnos");
            return ds;
        }

       
        public string administrarDatosAlumnos(string[] valores, string accion)
        {
            string salida = "1";

            try
            {
                conexion = new SqlConnection(cadenaConexion);
                conexion.Open();

                SqlCommand comando = new SqlCommand();
                comando.Connection = conexion;

                switch (accion)
                {
                    case "nuevo":
                        comando.CommandText = "INSERT INTO alumnos (codigo, nombre, direccion, telefono) VALUES (@codigo, @nombre, @direccion, @telefono)";
                        comando.Parameters.AddWithValue("@codigo", valores[1]);
                        comando.Parameters.AddWithValue("@nombre", valores[2]);
                        comando.Parameters.AddWithValue("@direccion", valores[3]);
                        comando.Parameters.AddWithValue("@telefono", valores[4]);
                        break;

                    case "modificar":
                        comando.CommandText = "UPDATE alumnos SET codigo=@codigo, nombre=@nombre, direccion=@direccion, telefono=@telefono WHERE idAlumno=@id";
                        comando.Parameters.AddWithValue("@id", valores[0]);
                        comando.Parameters.AddWithValue("@codigo", valores[1]);
                        comando.Parameters.AddWithValue("@nombre", valores[2]);
                        comando.Parameters.AddWithValue("@direccion", valores[3]);
                        comando.Parameters.AddWithValue("@telefono", valores[4]);
                        break;

                    case "eliminar":
                        comando.CommandText = "DELETE FROM alumnos WHERE idAlumno=@id";
                        comando.Parameters.AddWithValue("@id", valores[0]);
                        break;
                }

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                salida = "Error: " + ex.Message;
            }
            finally
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
                    conexion.Close();
            }

            return salida;
        }

        
        public bool probarConexion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cadenaConexion))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}