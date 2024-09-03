namespace DataAccessLayer.Entities
{
    public class ClassTranslation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ClassId { get; set; }
        public int LanguageId { get; set; }
        public Class Class { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }
}
