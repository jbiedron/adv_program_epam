namespace Carting.API.Messaging
{

    // TODO: to remove?
    public class ProductModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }                 // required, plain text, max-length = 50
        public string Description { get; set; }          // optional, can contain html
        public string Image { get; set; }                // optional, url
        public decimal Price { get; set; }               // required, money
    }
}
