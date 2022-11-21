namespace CartingService.DAL.Entities
{
    public class CartItem //: BaseEntity            // no need to have it as separate entity if case of no-sql db - it is nested collection
    {
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public  string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public uint Quantity { get; set; }
    }
}
