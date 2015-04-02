using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;

namespace FlowerTitan.Database
{
    /// <summary>
    /// Singleton class which handles all operations on the database.
    /// </summary>
    public class Database
    {
        //singleton instance
        private static Database database = null;
        //holds connection string to database
        private static String connectionString;
        //holds connection to database
        private static SQLiteConnection connection;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private Database() { }

        /// <summary>
        /// Returns singleton instance.
        /// </summary>
        /// <returns>Database instance.</returns>
        public static Database GetInstance()
        {            
            if (database == null)
            {
                database = new Database();
                //get connection string from app.config file
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
                //create a new connection
                connection = new SQLiteConnection(connectionString);
            }
            return database;
        }

        /// <summary>
        /// Gets last generated template's number and store a new one.
        /// </summary>
        /// <param name="increment">increment to last generated number</param>
        /// <returns>last generated template's number</returns>
        public long GetLastGeneratorNumber(long increment)
        {
            long num = 0;
            long id = 0;
            openConnection();
            SQLiteCommand command = connection.CreateCommand();
            //get last number
            command.CommandText = "SELECT id, generatedNumber FROM Properties LIMIT 1";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = (long)reader["id"];
                num = (long)reader["generatedNumber"];
            }
            reader.Close();
            //set new last number
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@nLN", num + increment);
            command.CommandText = "UPDATE Properties SET generatedNumber = @nLN WHERE id = @id";
            command.ExecuteNonQuery();
            closeConnection();
            return num;
        }

        /// <summary>
        /// Gets save file dialog's last path.
        /// </summary>
        /// <returns>last path</returns>
        public string GetSaveFilePath()
        {
            string path = "";
            openConnection();
            SQLiteCommand command = connection.CreateCommand();
            //get save file dialog path
            command.CommandText = "SELECT saveFilePath FROM Properties LIMIT 1";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                path = reader["saveFilePath"].ToString();
            }
            reader.Close();
            closeConnection();
            return path;
        }

        /// <summary>
        /// Saves last used save file dialog's path.
        /// </summary>
        /// <param name="path">last used path</param>
        public void SetSaveFilePath(string path)
        {
            openConnection();
            SQLiteCommand command = connection.CreateCommand();
            //set new last path
            command.Parameters.AddWithValue("@nLP", path);
            command.CommandText = "UPDATE Properties SET saveFilePath = @nLP WHERE id = 1";
            command.ExecuteNonQuery();
            closeConnection();
        }

        /// <summary>
        /// Opens connection.
        /// </summary>
        private void openConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    showError(e);
                }
            }
        }

        /// <summary>
        /// Closes connection.
        /// </summary>
        private void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    connection.Close();
                }
                catch (Exception e)
                {
                    showError(e);
                }
            }
        }

        /// <summary>
        /// Shows occured exception.
        /// </summary>
        /// <param name="e">occured exception</param>
        private void showError(Exception e)
        {
            System.Windows.Forms.MessageBox.Show("Error occured during database connection.\n" + e.Message, "Database error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }
}
