using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessTrackingApp.Data;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.ExerciseDTO
{
    /// <summary>
    /// Упражнение как объект для отображения.
    /// </summary>
    public class ExerciseResponseDTO
    {
        /// <summary>
        /// Название.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Активно/неактивно.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Название тренировочной программы.
        /// </summary>
        public required string TrainingProgramName { get; set; }

        /// <summary>
        /// Преобразует модель в объект для отображения.
        /// </summary>
        /// <param name="exercise">Объект модели.</param>
        /// <returns>Модель "Упражнение" как объект для отображения.</returns>
        public static ExerciseResponseDTO MapToResponseDTO(Exercise exercise, ApplicationDbContext context)
        {         
            var result = new ExerciseResponseDTO()
            {
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                TrainingProgramName = context.TrainingProgram.Find(exercise.TrainingProgramId).Name
            };

            return result;
        }
    }
}
