namespace ProjectPresents.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateUpdate { get; set; } = DateTime.Now;

        public ICollection<Present> Presents { get; set; }
    }
}