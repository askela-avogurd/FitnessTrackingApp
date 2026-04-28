using System.ComponentModel.DataAnnotations;

namespace FitnessTrackingApp.DTO.TrainingProgramDTO
{
    /// <summary>
    /// Тренировочная программа как данные для создания нового объекта.
    /// </summary>
    public class CreateProgramDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; }
    }
}
