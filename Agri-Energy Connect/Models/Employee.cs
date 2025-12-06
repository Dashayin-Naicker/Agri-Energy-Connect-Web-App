namespace ST10378422_PROG7311_POE.Models
{
    //Model Class for the Employee Table
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}
