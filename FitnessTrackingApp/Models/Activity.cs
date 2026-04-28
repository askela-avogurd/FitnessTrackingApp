using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackingApp.Models
{
    /// <summary>
    /// Активность (одно упражнение).
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// ID активности.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Дата (без времени).
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Время в минутах.
        /// </summary>
        [Range(1, 1440)]
        public int Minutes { get; set; }

        /// <summary>
        /// Заметка.
        /// </summary>
        [MaxLength(250)]
        public string? Notes { get; set; } = null;

        /// <summary>
        /// ID упражнения.
        /// </summary>
        [Required]
        public int ExerciseId { get; set; }

        /// <summary>
        /// Ссылка на упражнение.
        /// </summary>
        [ForeignKey(nameof(ExerciseId))]
        public required Exercise Exercise { get; set; }
    }
}
