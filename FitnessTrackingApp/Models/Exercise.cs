using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackingApp.Models
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [Required]
        public int TrainingProgramId { get; set; }

        [ForeignKey(nameof(TrainingProgramId))]
        public required TrainingProgram Program { get; set; }
    }
}
