using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.ExerciseDTO
{
    /// <summary>
    /// Упражнение как данные для обновления объекта.
    /// </summary>
    public class UpdateExerciseDTO
    {
        /// <summary>
        /// Название.
        /// </summary>
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Активно/неактивно.
        /// </summary>
        public bool? IsActive { get; set; } = true;

        /// <summary>
        /// Тренировочная программа.
        /// </summary>
        public string? TrainingProgramName { get; set; }
    }
}
