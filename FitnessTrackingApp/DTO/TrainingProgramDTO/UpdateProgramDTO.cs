using System.ComponentModel.DataAnnotations;

namespace FitnessTrackingApp.DTO.TrainingProgramDTO
{
    /// <summary>
    /// Тренировочная программа как объект с данными для обновления.
    /// </summary>
    public class UpdateProgramDTO
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
