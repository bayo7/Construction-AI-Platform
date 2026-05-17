namespace Construction.Entity.Entities
{
    public class ProjectImage : BaseEntity
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string ImageUrl { get; set; }   // wwwroot/images/ altındaki dosya adı
        public int SortOrder { get; set; } = 0; // sıralama
        public string? Caption { get; set; }   // isteğe bağlı açıklama
    }
}
