namespace ST10378422_PROG7311_POE.Models
{
    //Model Class for the Farmer Table
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string FullName { get; set; }
        public string FarmLocation { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

}
