using System.ComponentModel.DataAnnotations;
using FitnessTrackingApp.DTO.TrainingProgramDTO;

namespace FitnessTrackingApp.Models
{
    /// <summary>
    /// Тренировочная программа.
    /// </summary>
    public class TrainingProgram
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Type { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
