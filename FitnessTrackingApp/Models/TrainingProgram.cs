using System.ComponentModel.DataAnnotations;
using FitnessTrackingApp.DTO.TrainingProgramDTO;

namespace FitnessTrackingApp.Models
{
    /// <summary>
    /// Тренировочная программа.
    /// </summary>
    public class TrainingProgram
    {
        /// <summary>
        /// ID тренировочной программы.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// Тип программы.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public required string Type { get; set; }

        /// <summary>
        /// Активна/неактивна.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
