using MySql.Data.MySqlClient;
using System;
using UnityEngine;
using TMPro;

namespace Example.MySql
{
    public class DatabaseManager : MonoBehaviour
    {

        #region VARIABLES

        [Header("Database Properties")]
        public string Host = "localhost";
        public string User = "root";
        public string Password = "";
        public string Database = "disoveral";

        private string connectionString = "server=localhost;database=disoveral;uid=root;pwd=;";

        public string nuevoCorreo = "nuevoCorreo@example.com";

        #endregion

        #region UI

        [Header("UI Elements")]
        public TMP_InputField usernameInputField;
        public TMP_InputField passwordInputField;

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            Connect();
        }

        #endregion

        #region METHODS

        private void Connect()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Host;
            builder.UserID = User;
            builder.Password = Password;
            builder.Database = Database;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
                {
                    connection.Open();
                    print("MySQL - Opened Connection");
                }
            }
            catch (MySqlException exception)
            {
                print(exception.Message);
            }
        }

        public void CheckLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM tb_user WHERE username = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                if (userCount > 0)
                {
                    Debug.Log("Login successful");

                    // Obtener el correo del usuario
                    query = "SELECT email FROM tb_user WHERE username = @username";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@username", username);

                    string email = cmd.ExecuteScalar().ToString();
                    Debug.Log("Correo del usuario: " + email);

                    // Cambiar el correo del usuario
                    query = "UPDATE tb_user SET email = @nuevoCorreo WHERE username = @username";
                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nuevoCorreo", nuevoCorreo);
                    cmd.Parameters.AddWithValue("@username", username);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Debug.Log("Correo actualizado exitosamente.");
                    }
                    else
                    {
                        Debug.Log("Error al actualizar el correo.");
                    }

                    // Aquí puedes agregar el código para avanzar a la siguiente escena o dar acceso al usuario.
                }
                else
                {
                    Debug.Log("Login failed: Incorrect username or password.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error connecting to database: " + ex.Message);
            }
        }
    }

        #endregion
    }
}