using System.ComponentModel.DataAnnotations;

namespace FitnessTrackingApp.DTO.ActivityDTO
{
    /// <summary>
    /// Активность как данные для создания новой записи.
    /// </summary>
    public class CreateActivityDTO
    {
        /// <summary>
        /// Дата (без времени).
        /// </summary>
        [Required(ErrorMessage = "Дата обязательна")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Длительность активности в минутах.
        /// </summary>
        [Required(ErrorMessage = "Количество минут обязательно")]
        [Range(1, 1440, ErrorMessage = "Минуты должны быть от 1 до 1440")]
        public int Minutes { get; set; }

        /// <summary>
        /// Заметка.
        /// </summary>
        [MaxLength(250, ErrorMessage = "Примечание не длиннее 250 символов")]
        public string? Notes { get; set; }

        /// <summary>
        /// ID упражнения из активности.
        /// </summary>
        [Required(ErrorMessage = "ID упражнения обязателен")]
        public int ExerciseId { get; set; }
    }
}
