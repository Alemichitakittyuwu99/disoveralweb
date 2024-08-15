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