namespace GenericRepository_Pattern.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
