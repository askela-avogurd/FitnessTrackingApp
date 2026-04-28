using FitnessTrackingApp.Data;

namespace FitnessTrackingApp.Services
{
    public enum StickerColor
    {
        Yellow,  // < 30 минут.
        Green,   // 30-90 минут.
        Red      // > 90 минут.
    }
    public interface IActivityService
    {
        Task<bool> ValidateMinutesLimitAsync(DateTime date, int minutes, int excludeActivityId = 0);
        Task<bool> CanChangeExerciseAsync(int activityId, int newExerciseId);
    }
}
