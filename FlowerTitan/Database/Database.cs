﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;
using System.Drawing;
using FlowerTitan.MeasuringLines;

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
                command.CommandText = "SELECT id, generatedNumber FROM Properties LIMIT 1;";
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
                command.CommandText = "UPDATE Properties SET generatedNumber = @nLN WHERE id = @id;";
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
        /// Saves template to DB.
        /// </summary>
        /// <param name="allLines">Lines of all images.</param>
        /// <param name="allImages">All images images.</param>
        /// <param name="isTemplate">Determines, whether it should be saved as template.</param>
        /// <param name="temp_id">Template scanned id.</param>
        /// <param name="name">Template given name.</param>
        /// <param name="scale">Template scale.</param>
        /// <param name="linesColors">Lines colors.</param>
        /// <param name="linesNames">Lines names.</param>
        public void SaveTemplate(AllLines[] allLines, Bitmap[] allImages, bool isTemplate, string temp_id, string name, float scale, Color[] linesColors, string[] linesNames)
        {
            try
            {
                openConnection();
                bool exists = false;
                long id = 0;
                templateExists(temp_id, ref exists, ref id);
                SQLiteCommand command = connection.CreateCommand();
                command.Parameters.AddWithValue("@date", DateTime.Now.ToUniversalTime());
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@scale", scale);
                command.Parameters.AddWithValue("@temp_id", temp_id);
                command.Parameters.AddWithValue("@isTemplate", isTemplate);
                command.CommandText = "INSERT INTO Templates (date, name, scale, temp_id, isTemplate) VALUES (@date, @name, @scale, @temp_id, @isTemplate);";
                int a =command.ExecuteNonQuery();
                saveAllLines(allLines, allImages, getLastID(), linesColors, linesNames);
                if (exists)
                {
                    deleteOldTemplate(id);
                }
                closeConnection();
            }
            catch (Exception e)
            {
                showError(e);
            }
        }

        private void deleteOldTemplate(long id)
        {
            long[] images = getChildren(id, "Images", "template");
            foreach (long image in images)
            {
                long[] lines = getChildren(image, "Lines", "image");
                foreach (long line in lines)
                {
                    deleteChildren(image, "Points", "line");
                }
                deleteChildren(image, "Lines", "image");
            }
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "DELETE FROM Images WHERE template=@id;";
            command.ExecuteNonQuery();
            command.CommandText = "DELETE FROM Templates WHERE id=@id;";
            command.ExecuteNonQuery();
        }

        private void deleteChildren(long id, string table, string column)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "DELETE FROM " + table + " WHERE " + column + "=@id;";
            command.ExecuteNonQuery();
        }

        private long[] getChildren(long id, string table, string column)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "SELECT id FROM " + table + " WHERE " + column + "=@id;";
            SQLiteDataReader reader = command.ExecuteReader();
            List<long> cildren = new List<long>();
            while (reader.Read())
            {
                cildren.Add((long)reader["id"]);
            }
            reader.Close();
            return cildren.ToArray();
        }

        private void saveAllLines(AllLines[] allLines, Bitmap[] allImages, long id, Color[] colors, string[] names)
        {
            int i = 0;
            foreach (AllLines al in allLines)
            {
                saveImage(al, allImages[i], id, colors, names);
                i++;
            }
        }

        private void saveImage(AllLines al, Bitmap bitmap, long id, Color[] colors, string[] names)
        {
            SQLiteCommand command = connection.CreateCommand();
            ImageConverter converter = new ImageConverter();
            command.Parameters.AddWithValue("@image", (byte[])converter.ConvertTo(bitmap, typeof(byte[])));
            command.Parameters.AddWithValue("@template", id);
            command.Parameters.AddWithValue("@imageBoxID", al.ImageBoxID);
            command.CommandText = "INSERT INTO Images (image, template, imageBoxID) VALUES (@image, @template, @imageBoxID);";
            command.ExecuteNonQuery();
            long imageID = getLastID();
            int i = 0;
            foreach (Line line in al.Lines)
            {
                saveLine(line, imageID, colors[i], names[i]);
                i++;
            }
        }

        private void saveLine(Line line, long imageID, Color color, string name)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@color", color.ToArgb());
            command.Parameters.AddWithValue("@length_mm", LengthConverter.LengthConverter.GetInstance().ConvertLineLengthToMM(line));
            command.Parameters.AddWithValue("@image", imageID);
            command.CommandText = "INSERT INTO Lines (name, color, length_mm, image) VALUES (@name, @color, @length_mm, @image);";
            command.ExecuteNonQuery();
            long lineID = getLastID();
            foreach (Point point in line.Points)
            {
                savePoint(point, lineID);
            }
        }

        private void savePoint(Point point, long lineID)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@x", point.X);
            command.Parameters.AddWithValue("@y", point.Y);
            command.Parameters.AddWithValue("@line", lineID);
            command.CommandText = "INSERT INTO Points (x, y, line) VALUES (@x, @y, @line);";
            command.ExecuteNonQuery();
        }

        private void templateExists(string temp_id, ref bool exists, ref long id)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@temp_id", temp_id);
            command.CommandText = "SELECT id, temp_id FROM Templates WHERE temp_id=@temp_id LIMIT 1;";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = (long)reader["id"];
                exists = true;
            }
            reader.Close();
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
                command.CommandText = String.Format("SELECT {0} FROM Properties LIMIT 1;", column);
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

        private long getLastID()
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT last_insert_rowid();";
            return (long)command.ExecuteScalar();
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
                command.CommandText = String.Format("UPDATE Properties SET {0} = @val WHERE id = 1;", column);
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
