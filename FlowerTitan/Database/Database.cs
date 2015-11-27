using System;
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
            System.Windows.Forms.MessageBox.Show(Properties.Resources.Database_error_text + "\n" + e.Message, Properties.Resources.Database_error_title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                command.CommandText = "SELECT id, generated_number FROM Properties LIMIT 1;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = (long)reader["id"];
                    num = (long)reader["generated_number"];
                }
                reader.Close();
                //set new last number
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nLN", num + increment);
                command.CommandText = "UPDATE Properties SET generated_number = @nLN WHERE id = @id;";
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
        /// <param name="mainWindow">Owner of measuring lines.</param>
        public long SaveTemplate(AllLines[] allLines, Bitmap[] allImages, bool isTemplate, string temp_id, string name, double scale, Color[] linesColors, string[] linesNames, MainWindow mainWindow)
        {
            try
            {
                openConnection();
                bool exists = false;
                long id = 0;
                templateExists(temp_id, ref exists, ref id, isTemplate);
                SQLiteCommand command = connection.CreateCommand();
                command.Parameters.AddWithValue("@date", DateTime.Now.ToUniversalTime());
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@scale", scale);
                command.Parameters.AddWithValue("@temp_id", temp_id);
                command.Parameters.AddWithValue("@is_template", isTemplate);
                byte[] tempImage;
                if (isTemplate) tempImage = new byte[1] { 0 };
                else
                {
                    ImageConverter converter = new ImageConverter();
                    tempImage = (byte[])converter.ConvertTo(new Bitmap(mainWindow.pictureBoxID.Image), typeof(byte[]));
                }
                command.Parameters.AddWithValue("@temp_id_image", tempImage);
                command.CommandText = "INSERT INTO Templates (date, name, scale, temp_id, is_template, temp_id_image) VALUES (@date, @name, @scale, @temp_id, @is_template, @temp_id_image);";
                command.ExecuteNonQuery();
                long newID = getLastID();
                saveAllLines(allLines, allImages, newID, linesColors, linesNames, isTemplate, mainWindow);
                if (exists)
                {
                    deleteOldTemplate(id);
                }
                Action stateChanged = new Action(() =>
                {
                    mainWindow.toolStripProgressBar.Value = 100;
                });
                mainWindow.Invoke(stateChanged);
                closeConnection();
                return newID;
            }
            catch (Exception e)
            {
                showError(e);
            }
            return -1L;
        }

        /// <summary>
        /// Delete template from DB.
        /// </summary>
        /// <param name="id">template id</param>
        public void DeleteTemplate(long id)
        {
            try
            {
                openConnection();
                deleteOldTemplate(id);
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
                    deleteChildren(line, "Points", "line");
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

        private void saveAllLines(AllLines[] allLines, Bitmap[] allImages, long id, Color[] colors, string[] names, bool isTemplate, MainWindow mainWindow)
        {
            int i = 0;
            foreach (AllLines al in allLines)
            {
                saveImage(al, allImages[i], id, colors, names, mainWindow.GetTemplateScale());
                i++;
                Action stateChanged = new Action(() =>
                {
                    mainWindow.toolStripProgressBar.Value = (96 / allLines.Length) * i;
                });
                mainWindow.Invoke(stateChanged);
                if (isTemplate)
                {
                    Action stateChanged2 = new Action(() =>
                    {
                        mainWindow.toolStripProgressBar.Value = 75;
                    });
                    mainWindow.Invoke(stateChanged2);
                    break;
                }
            }
        }

        private void saveImage(AllLines al, Bitmap bitmap, long id, Color[] colors, string[] names, double scale)
        {
            SQLiteCommand command = connection.CreateCommand();
            ImageConverter converter = new ImageConverter();
            command.Parameters.AddWithValue("@image", (byte[])converter.ConvertTo(bitmap, typeof(byte[])));
            command.Parameters.AddWithValue("@template", id);
            command.Parameters.AddWithValue("@image_box_id", al.ImageBoxID);
            command.CommandText = "INSERT INTO Images (image, template, image_box_id) VALUES (@image, @template, @image_box_id);";
            command.ExecuteNonQuery();
            long imageID = getLastID();
            int i = 0;
            foreach (Line line in al.Lines)
            {
                saveLine(line, imageID, colors[i], names[i], scale);
                i++;
            }
        }

        private void saveLine(Line line, long imageID, Color color, string name, double scale)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@color", color.ToArgb());
            command.Parameters.AddWithValue("@length_mm", LengthConverter.LengthConverter.GetInstance().ConvertLineLengthToMM(line, scale));
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

        private void templateExists(string temp_id, ref bool exists, ref long id, bool isTemplate)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@temp_id", temp_id);
            command.Parameters.AddWithValue("@is_template", isTemplate);
            command.CommandText = "SELECT id, temp_id FROM Templates WHERE temp_id=@temp_id AND is_template=@is_template LIMIT 1;";
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
            return getProperty("save_file_path");
        }

        /// <summary>
        /// Gets open file dialog's last path.
        /// </summary>
        /// <returns>last path</returns>
        public string GetOpenFilePath()
        {
            return getProperty("open_file_path");
        }

        /// <summary>
        /// Gets default XLS path.
        /// </summary>
        /// <returns>XLS path</returns>
        public string GetXLSPath()
        {
            return getProperty("default_xls_path");
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
            setProperty("save_file_path", path);
        }

        /// <summary>
        /// Saves last used open file dialog's path.
        /// </summary>
        /// <param name="path">last used path</param>
        public void SetOpenFilePath(string path)
        {
            setProperty("open_file_path", path);
        }

        /// <summary>
        /// Saves default XLS path.
        /// </summary>
        /// <param name="path">new XLS path</param>
        public void SetXLSPath(string path)
        {
            setProperty("default_xls_path", path);
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

        /// <summary>
        /// Loads all templates which were saved as template or all saved templates from DB.
        /// </summary>
        /// <param name="ids">templates' real (db) ids</param>
        /// <param name="dates">templates' dates</param>
        /// <param name="names">templates' names</param>
        /// <param name="tempIDs">templates' visible ids</param>
        /// <param name="isTemplate">determines what kind of templates will be loaded (templates/templates template)</param>
        public void LoadTemplates(List<long> ids, List<DateTime> dates, List<string> names, List<long> tempIDs, bool isTemplate)
        {
            try
            {
                openConnection();
                SQLiteCommand command = connection.CreateCommand();
                command.Parameters.AddWithValue("@is_template", isTemplate);
                command.CommandText = "SELECT * FROM Templates WHERE is_template=@is_template;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ids.Add((long)reader["id"]);
                    dates.Add((DateTime)reader["date"]);
                    names.Add((string)reader["name"]);
                    tempIDs.Add((long)reader["temp_id"]);
                }
                reader.Close();
                closeConnection();
            }
            catch (Exception e)
            {
                showError(e);
            }
        }

        /// <summary>
        /// Loads template from db AS template.
        /// </summary>
        /// <param name="id">template id</param>
        /// <param name="colors">template's lines' colors</param>
        /// <param name="names">template's lines' names</param>
        /// <returns>all template's lines</returns>
        public Line[] LoadAsTemplate(long id, List<int> colors, List<string> names)
        {
            try
            {
                openConnection();
                SQLiteCommand command = connection.CreateCommand();
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@is_template", true);
                command.CommandText = "SELECT id, template FROM Images WHERE template=@id;";
                SQLiteDataReader reader = command.ExecuteReader();
                long imageID = 0;
                while (reader.Read())
                {
                    imageID = (long)reader["id"];
                }
                reader.Close();
                Line[] lines = getLines(imageID, colors, names);
                closeConnection();
                return lines;
            }
            catch (Exception e)
            {
                showError(e);
            }
            return null;
        }

        private Line[] getLines(long imageID, List<int> colors, List<string> names)
        {
            List<Line> lines = new List<Line>();
            List<long> linesIDs = new List<long>();
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@id", imageID);
            command.CommandText = "SELECT * FROM Lines WHERE image=@id;";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                linesIDs.Add((long)reader["id"]);
                names.Add((string)reader["name"]);
                colors.Add((int)((long)reader["color"]));
            }
            reader.Close();
            foreach (long lID in linesIDs){
                lines.Add(getLine(lID));
            }
            return lines.ToArray();
        }

        private Line getLine(long lineID)
        {
            List<Point> linePoints = new List<Point>();
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@id", lineID);
            command.CommandText = "SELECT * FROM Points WHERE line=@id;";
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                linePoints.Add(new Point((int)((long)reader["x"]), (int)((long)reader["y"])));
            }
            reader.Close();
            Line l = new Line();
            l.Points = linePoints;
            return l;
        }

        /// <summary>
        /// Load template from DB.
        /// </summary>
        /// <param name="id">template's db id</param>
        /// <param name="colors">lines' colors</param>
        /// <param name="names">lines' names</param>
        /// <param name="name">template's name</param>
        /// <param name="scale">template's scale</param>
        /// <param name="temp_id">template's visible id</param>
        /// <param name="images">template's images</param>
        /// <param name="tempIdImage">template's id image</param>
        /// <returns>all template's lines</returns>
        public AllLines[] LoadTemplate(long id, List<int> colors, List<string> names, ref string name, ref double scale, ref long temp_id, List<byte[]> images, ref List<byte> tempIdImage)
        {
            try
            {
                openConnection();
                SQLiteCommand command = connection.CreateCommand();
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@is_template", false);
                command.CommandText = "SELECT * FROM Templates WHERE id=@id;";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    name = (string)reader["name"];
                    scale = (double)reader["scale"];
                    temp_id = (long)reader["temp_id"];
                    tempIdImage = ((byte[])reader["temp_id_image"]).ToList<byte>();
                }
                reader.Close();
                AllLines[] allLines = getImages(id, images, colors, names);
                closeConnection();
                return allLines;
            }
            catch (Exception e)
            {
                showError(e);
            }
            return null;
        }

        private AllLines[] getImages(long id, List<byte[]> images, List<int> colors, List<string> names)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@id", id);
            command.CommandText = "SELECT * FROM Images WHERE template=@id;";
            SQLiteDataReader reader = command.ExecuteReader();
            List<long> imageIDs = new List<long>();
            List<long> imageBoxes = new List<long>();
            while (reader.Read())
            {
                imageIDs.Add((long)reader["id"]);
                images.Add((byte[])reader["image"]);
                imageBoxes.Add((long)reader["image_box_id"]);
            }
            reader.Close();
            List<AllLines> allLines = new List<AllLines>();
            int i = 0;
            foreach (long imageID in imageIDs)
            {
                AllLines al = new AllLines(imageBoxes[i]);
                al.Lines = getLines(imageID, colors, names).ToList<Line>();
                allLines.Add(al);
                i++;
            }
            return allLines.ToArray();
        }
    }
}
