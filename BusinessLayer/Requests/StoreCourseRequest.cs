
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests
{
    public class StoreCourseRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int ClassId { get; set; }

        public List<LessonRequestBase> Lessons { get; set; } = new List<LessonRequestBase>();
    }
}
