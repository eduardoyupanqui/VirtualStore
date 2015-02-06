using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualStore.Helpers
{
    public class PasswordHashHelper
    {
        public static bool Autenticar(string usuario, string password)
        {
            string sql = @"SELECT COUNT(*)
                      FROM Usuarios
                      WHERE NombreLogin = @nombre AND Password = @password";


            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VirtualStore"].ToString()))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@nombre", usuario);

                string hash = Helper.EncodePassword(string.Concat(usuario, password));
                command.Parameters.AddWithValue("@password", hash);

                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                    return false;
                else
                    return true;

            }
        }

        public static UsuarioEntity Insert(UsuarioEntity usuario)
        {

            string sql = @"INSERT INTO Usuarios (
                           Nombre
                          ,Apellido
                          ,NombreLogin
                          ,Password)
                      VALUES (
                            @Nombre, 
                            @Apellido, 
                            @NombreLogin,
                            @Password)
                    SELECT SCOPE_IDENTITY()";


            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VirtualStore"].ToString()))
            {

                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("NombreLogin", usuario.NombreLogin);

                string password = Helper.EncodePassword(string.Concat(usuario.NombreLogin, usuario.Password));
                command.Parameters.AddWithValue("Password", password);

                conn.Open();

                usuario.Id = Convert.ToInt32(command.ExecuteScalar());

                return usuario;
            }
        }
    }

    public class UsuarioEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreLogin { get; set; }
        public string Password { get; set; }

        public UsuarioEntity()
        {

        }
    }
}
