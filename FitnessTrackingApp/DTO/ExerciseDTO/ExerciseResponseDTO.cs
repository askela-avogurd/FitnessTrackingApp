using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.ExerciseDTO
{
    /// <summary>
    /// Упражнение как объект для отображения.
    /// </summary>
    public class ExerciseResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int TrainingProgramId { get; set; }
    }
}
