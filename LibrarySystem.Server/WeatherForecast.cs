using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace LibrarySystem.Server
{
    public class WeatherForecast
    {
        const string connectionString = "Data Source=LAPTOP-KH5U492M;Initial Catalog=librarydb;Integrated Security=True;Trust Server Certificate=True";
        public Book[] GetBooks()
        {
            var books = new List<Book>();
            

            using (var cursor = new SqlConnection(connectionString))
            {
                cursor.Open();

                var command = new SqlCommand("SELECT * FROM books WHERE available=1", cursor);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var book = new Book
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Title = reader["title"].ToString(),
                        YearOfRelease = Convert.ToInt32(reader["releaseYear"]),
                        Author = reader["author"].ToString()
                    };
                    books.Add(book);
                }
            }

            return books.ToArray();
        }

        public bool isAvailable(int id)
        {
            bool available = false;

            using (var cursor = new SqlConnection(connectionString))
            {
                cursor.Open();
                var command = new SqlCommand("SELECT available FROM books WHERE id = @id", cursor);
                command.Parameters.AddWithValue("@id", id);

                available = (bool)command.ExecuteScalar();
                
            }

            return available;
        }

        public void borrowBook(int id)
        {
            using(var cursor = new SqlConnection(connectionString))
            {
                cursor.Open();
                var updateCommand = new SqlCommand("UPDATE books SET available = 0 WHERE id = @id", cursor);
                updateCommand.Parameters.AddWithValue("@id", id);
                updateCommand.ExecuteNonQuery();
            }      
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public string Author { get; set; }
    }
}
