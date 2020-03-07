using Npgsql;
using System.Collections.Generic;

namespace Controllers
{
    class PsGre
    {
        private NpgsqlConnection conn;
        public PsGre(string connectionString)
        {
            conn = new NpgsqlConnection(connectionString);
        }
        
        public List<string> SelectDatabases()
        {
            List<string> dbs = new List<string>();
            this.conn.Open();
            using (var cmd = new NpgsqlCommand($"SELECT datname FROM pg_database;", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    dbs.Add(reader.GetString(0));
            this.conn.Close();
            return dbs;
        }

        public string SelectSize(string dbname)
        {
            string size = "";
            this.conn.Open();
            using (var cmd = new NpgsqlCommand($"SELECT pg_size_pretty( pg_database_size('{dbname}') );", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    size = reader.GetString(0);
            this.conn.Close();
            return size;
        }
    }
}
