using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackingApp.Models
{
    /// <summary>
    /// Упражнение.
    /// </summary>
    public class Exercise
    {
        /// <summary>
        /// ID упражнения.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Активно/неактивно.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ID тренировочной программы.
        /// </summary>
        [Required]
        public int TrainingProgramId { get; set; }

        /// <summary>
        /// Ссылка на тренировочную программу.
        /// </summary>
        [ForeignKey(nameof(TrainingProgramId))]
        public required TrainingProgram Program { get; set; }
    }
}
