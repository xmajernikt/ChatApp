using Npgsql;


namespace ChatAppServer.Database
{
    internal class DbContext
    {
        private string? dbConnectionString;

        // Initialize Connection property to null
        public NpgsqlConnection? Connection { get; private set; }

        public void ConnectToDatabase()
        {
            try
            {
                dbConnectionString = GetDbConnectionString();

                NpgsqlConnection npgsqlConnection = new NpgsqlConnection(dbConnectionString);
                npgsqlConnection.Open();
                Connection = npgsqlConnection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                // Handle the exception (e.g., log, throw, etc.)
            }
        }

        private string GetDbConnectionString()
        {
            string[] envLines = File.ReadAllLines("C:\\Users\\admin\\source\\repos\\ChatApp\\.env");
            Dictionary<string, string> env = new Dictionary<string, string>();

            foreach (string line in envLines)
            {
                string[] parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    env[key] = value;
                }
            }

            return $"Host={env["DB_HOST"]};Port={env["DB_PORT"]};Database={env["DB_NAME"]};User Id={env["DB_USERNAME"]};Password={env["DB_PASSWORD"]};";
        }
    }
}
