using System.ComponentModel.DataAnnotations;

namespace TheCodeCamp.Models
{
    public class TalkModel
    {
        public int TalkId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(4000,MinimumLength =100)]
        public string Abstract { get; set; }
        [Required]
        [Range(100,500)]
        public int Level { get; set; }
        [Required]
        public SpeakerModel Speaker { get; set; }
    }
}