namespace AmisduMalade.Models
{
    // مركز الجمعية في البلدية
    public class Center
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";          // اسم المركز
        public string Municipality { get; set; } = "";  // البلدية
        public string Address { get; set; } = "";       // العنوان
        public string? Phone { get; set; }              // الهاتف
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}