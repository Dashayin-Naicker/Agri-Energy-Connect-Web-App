namespace ST10378422_PROG7311_POE.Models
{
    //Model Class for the Product Table
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }

        public int FarmerId { get; set; }
        public virtual Farmer Farmer { get; set; }
    }

}
