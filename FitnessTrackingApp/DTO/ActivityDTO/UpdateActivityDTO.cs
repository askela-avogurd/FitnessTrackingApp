using System.ComponentModel.DataAnnotations;

namespace FitnessTrackingApp.DTO.ActivityDTO
{
    /// <summary>
    /// АКтивность как данные для обновления объекта.
    /// </summary>
    public class UpdateActivityDTO
    {
        [Range(1, 1440, ErrorMessage = "Минуты должны быть от 1 до 1440")]
        public int? Minutes { get; set; }

        [MaxLength(250, ErrorMessage = "Примечание не длиннее 250 символов")]
        public string? Notes { get; set; } = null;
    }
}