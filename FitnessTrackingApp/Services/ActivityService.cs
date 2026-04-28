using Microsoft.EntityFrameworkCore;
using FitnessTrackingApp.Data;

namespace FitnessTrackingApp.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;

        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateMinutesLimitAsync(DateTime date, int minutes, int excludeActivityId = 0)
        {
            var totalForDay = await _context.Activity
                .Where(a => a.Date.Date == date.Date && a.Id != excludeActivityId)
                .SumAsync(a => a.Minutes);

            return totalForDay + minutes <= 1440;
        }

        public async Task<bool> CanChangeExerciseAsync(int activityId, int newExerciseId)
        {
            var exercise = await _context.Exercise.FindAsync(newExerciseId);
            return exercise != null && exercise.IsActive;
        }
    }
}
