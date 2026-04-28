using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackingApp.Models
{
    /// <summary>
    /// Активность (одно упражнение).
    /// </summary>
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Range(1, 1440)]
        public int Minutes { get; set; }

        [MaxLength(250)]
        public string? Notes { get; set; } = null;

        // Ссылка на упражнение (внешний ключ)
        [Required]
        public int ExerciseId { get; set; }

        [ForeignKey(nameof(ExerciseId))]
        public required Exercise Exercise { get; set; }
    }
}
