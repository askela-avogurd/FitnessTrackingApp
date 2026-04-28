using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.ExerciseDTO
{
    /// <summary>
    /// Упражнение как данные для создания объекта.
    /// </summary>
    public class CreateExerciseDTO
    {
        /// <summary>
        /// Название упражнения.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// ID тренировочной программы.
        /// </summary>
        [Required]
        public int TrainingProgramId { get; set; }
    }
}
