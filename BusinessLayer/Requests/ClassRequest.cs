namespace BusinessLayer.Requests
{
    public class ClassRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ClassTranslationRequest>? Translations { get; set; } = new List<ClassTranslationRequest>();
    }



}