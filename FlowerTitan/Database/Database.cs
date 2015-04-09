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
                try
                {
                    //get connection string from app.config file
                    connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
                    //create a new connection
                    connection = new SQLiteConnection(connectionString);
                }
                catch (Exception e)
                {
                    showError(e);
                }
            }
            return database;
        }

        /// <summary>
        /// Shows occured exception.
        /// </summary>
        /// <param name="e">occured exception</param>
        private static void showError(Exception e)
        {
            System.Windows.Forms.MessageBox.Show(Properties.Resources.Database_error_text + e.Message, Properties.Resources.Database_error_title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        /// <summary>
        /// Gets last generated template's number and store a new one.
        /// </summary>
        /// <param name="increment">increment to last generated number</param>
        /// <returns>last generated template's number</returns>
        public long GetAndSetLastGeneratorNumber(long increment)
        {
            long num = 0;
            long id = 0;
            try
            {
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
            }
            catch (Exception e)
            {
                showError(e);
            }
            return num;
        }

        /// <summary>
        /// Gets save file dialog's last path.
        /// </summary>
        /// <returns>last path</returns>
        public string GetSaveFilePath()
        {
            return getProperty("saveFilePath");
        }

        /// <summary>
        /// Gets open file dialog's last path.
        /// </summary>
        /// <returns>last path</returns>
        public string GetOpenFilePath()
        {
            return getProperty("openFilePath");
        }

        /// <summary>
        /// Gets default XLS path.
        /// </summary>
        /// <returns>XLS path</returns>
        public string GetXLSPath()
        {
            return getProperty("defaultXLSPath");
        }

        /// <summary>
        /// Gets column value from table Properties
        /// </summary>
        /// <param name="column">column name</param>
        /// <returns>column value</returns>
        private string getProperty(string column)
        {
            string val = "";
            try
            {
                openConnection();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = String.Format("SELECT {0} FROM Properties LIMIT 1", column);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    val = reader[column].ToString();
                }
                reader.Close();
                closeConnection();
            }
            catch (Exception e)
            {
                showError(e);
            }
            return val;
        }

        /// <summary>
        /// Saves last used save file dialog's path.
        /// </summary>
        /// <param name="path">last used path</param>
        public void SetSaveFilePath(string path)
        {
            setProperty("saveFilePath", path);
        }

        /// <summary>
        /// Saves last used open file dialog's path.
        /// </summary>
        /// <param name="path">last used path</param>
        public void SetOpenFilePath(string path)
        {
            setProperty("openFilePath", path);
        }

        /// <summary>
        /// Saves default XLS path.
        /// </summary>
        /// <param name="path">new XLS path</param>
        public void SetXLSPath(string path)
        {
            setProperty("defaultXLSPath", path);
        }

        /// <summary>
        /// Sets column value in table Properties
        /// </summary>
        /// <param name="column">column name</param>
        /// <param name="value">column value</param>
        private void setProperty(string column, string value)
        {
            try
            {
                openConnection();
                SQLiteCommand command = connection.CreateCommand();
                command.Parameters.AddWithValue("@val", value);
                command.CommandText = String.Format("UPDATE Properties SET {0} = @val WHERE id = 1", column);
                command.ExecuteNonQuery();
                closeConnection();
            }
            catch (Exception e)
            {
                showError(e);
            }
        }

        /// <summary>
        /// Opens connection.
        /// </summary>
        private void openConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                //enable foreign keys constraints
                command.CommandText = "PRAGMA foreign_keys = ON";
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Closes connection.
        /// </summary>
        private void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
