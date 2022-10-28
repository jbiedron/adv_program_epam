namespace CartingService.BLL
{
    /// <summary>
    /// This is just simple POCO to follow the rules. CartBO contains data/app logic
    /// </summary>
    public class CartItemBO 
    {
        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public uint Quantity { get; set; }
    }
}
