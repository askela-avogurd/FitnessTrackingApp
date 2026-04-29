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
        [MaxLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// Название тренировочной программы.
        /// </summary>
        public required string TrainingProgramName { get; set; }
    }
}
