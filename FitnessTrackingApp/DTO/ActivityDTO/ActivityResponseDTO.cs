using System.Diagnostics;
using FitnessTrackingApp.Data;

namespace FitnessTrackingApp.DTO.ActivityDTO
{
    /// <summary>
    /// Активность как объект для отображения.
    /// </summary>
    public class ActivityResponseDTO
    {
        /// <summary>
        /// Сводка активности за день.
        /// </summary>
        public class DailySummaryDTO
        {
            public DateTime Date { get; set; }
            public int TotalMinutes { get; set; }

            // Список всех активностей (упражнений) за день.
            public List<ActivityItemDTO> Activities { get; set; } = new();
        }

        /// <summary>
        /// Данные упражнения.
        /// </summary>
        public class ActivityItemDTO
        {
            public int Id { get; set; }
            public int Minutes { get; set; }
            public string? Notes { get; set; }
            public int ExerciseId { get; set; }
            public int ProgramId { get; set; }
        }

        /// <summary>
        /// Преобразует Activity в DailySummaryDTO.
        /// </summary>
        /// <param name="date">Дата (без времени).</param>
        /// <param name="context">Контекст БД.</param>
        /// <returns>Активность как объект для отображения.</returns>
        public static DailySummaryDTO MapToResponseDTO(DateTime date, ApplicationDbContext context)
        {
            var activitiesPerDay = new DailySummaryDTO()
            {
                Date = date.Date,
                TotalMinutes = 0
            };

            activitiesPerDay.Activities = context.Activity
                .Select(activity => new ActivityItemDTO()
                {
                    Id = activity.Id,
                    Minutes = activity.Minutes,
                    Notes = activity.Notes,
                    ExerciseId = (activity.Exercise).Id,
                    ProgramId = (activity.Exercise).TrainingProgramId
                })
                .ToList();

            activitiesPerDay.TotalMinutes = activitiesPerDay.Activities
                .Sum(item => item.Minutes);

            return activitiesPerDay;
        }
    }
}