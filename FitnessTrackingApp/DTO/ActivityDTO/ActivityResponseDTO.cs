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
            /// <summary>
            /// Дата (без времени).
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Суммарное время всех активностей (упражнений) за день.
            /// </summary>
            public int TotalMinutes { get; set; }

            /// <summary>
            /// Список всех активностей (упражнений) за день.
            /// </summary>
            public List<ActivityItemDTO> Activities { get; set; } = new();
        }

        /// <summary>
        /// Данные упражнения.
        /// </summary>
        public class ActivityItemDTO
        {
            /// <summary>
            /// ID активности.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Время в минутах.
            /// </summary>
            public int Minutes { get; set; }

            /// <summary>
            /// Заметка.
            /// </summary>
            public string? Notes { get; set; }

            /// <summary>
            /// ID упражнения.
            /// </summary>
            public int ExerciseId { get; set; }

            /// <summary>
            /// ID тренировочной программы.
            /// </summary>
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