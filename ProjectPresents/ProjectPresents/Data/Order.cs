namespace ProjectPresents.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public Client Clietns { get; set; }
        public int PresentsId { get; set; }
        public Present Presents { get; set; }
        public int Quantity { get; set; }
        public DateTime DateUpdate { get; set; } = DateTime.Now;
    }
}