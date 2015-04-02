using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerTitan.Database
{
    public class Database
    {
        //singleton instance
        private static Database database = null;

        //private static flowerTitanEntities db;

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
                //db = new flowerTitanEntities();
            }
            return database;
        }

        public long GetLastGeneratorNumber(long newLastNumber)
        {
            long num = 0;

            return num;
        }
    }
}
