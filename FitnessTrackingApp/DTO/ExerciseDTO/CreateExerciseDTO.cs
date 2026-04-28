using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.ExerciseDTO
{
    public class CreateExerciseDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int TrainingProgramId { get; set; }
    }
}
