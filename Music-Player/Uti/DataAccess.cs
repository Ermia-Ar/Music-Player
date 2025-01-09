using Music_Player.Model;
using Microsoft.Data.SqlClient;
using Dapper;
using Music_Player.Uti;


namespace Music_Player
{
    public class DataAccess
    {
        public static List<MUSIC> GetMusics()
        {
            using var connection = new SqlConnection(Helper.GetConnectionString("ConnectionDB"));
            connection.Open();

            var Query = "SELECT * FROM Musics";
            return connection.Query<MUSIC>(Query).ToList();
        }
        public static void AddMusic(string Name , string Path)
        {
            using var connection = new SqlConnection(Helper.GetConnectionString("ConnectionDB"));
            connection.Open();

            var Query = "INSERT INTO Musics (Name , path) VALUES (@name , @path)";
            connection.Execute(Query, new { name = Name, path = Path });
        }
        public static void DeleteMusic(int ID)
        {
            using var connection = new SqlConnection(Helper.GetConnectionString("ConnectionDB"));
            connection.Open();

            var Query = "DELETE FROM Musics WHERE Id = @Id";
            connection.Execute(Query, new {Id = ID});
        }
    }
}
