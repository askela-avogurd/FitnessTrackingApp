using System.ComponentModel.DataAnnotations;

namespace FitnessTrackingApp.DTO.TrainingProgramDTO
{
    /// <summary>
    /// Тренировочная программа как объект с данными для обновления.
    /// </summary>
    public class UpdateProgramDTO
    {
        /// <summary>
        /// Название программы.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Тип программы.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Активна/неактивна.
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
