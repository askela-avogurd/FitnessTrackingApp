using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.ExerciseDTO
{
    /// <summary>
    /// Упражнение как объект для отображения.
    /// </summary>
    public class ExerciseResponseDTO
    {
        /// <summary>
        /// ID упражнения.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Активно/неактивно.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ID тренировочной программы.
        /// </summary>
        public int TrainingProgramId { get; set; }

        /// <summary>
        /// Преобразует модель в объект для отображения.
        /// </summary>
        /// <param name="exercise">Объект модели.</param>
        /// <returns>Модель "Упражнение" как объект для отображения.</returns>
        public static ExerciseResponseDTO MapToResponseDTO(Exercise exercise)
        {
            var result = new ExerciseResponseDTO()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                IsActive = exercise.IsActive,
                TrainingProgramId = exercise.TrainingProgramId
            };

            return result;
        }
    }
}
