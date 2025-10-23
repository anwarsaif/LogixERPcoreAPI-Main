namespace Logix.MVC.ViewModels
{
    public class AttFile
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? TypeName { get; set; }
        public int TypeId { get; set; }
        public string Url { get; set; } = null!;
        public string Ext { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

    }
}
