namespace Shipping.Models
{
    public class Government
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;

        public virtual List<City>? Cities { get; set; } = new List<City>();
        public virtual List<Branch>? Branches { get; set; }=new List<Branch>();
    }
}
