using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using FitnessTrackingApp.Models;

namespace FitnessTrackingApp.DTO.TrainingProgramDTO
{
    /// <summary>
    /// Тренировочная программа как объект для отображения.
    /// </summary>
    public class ProgramResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Преобразует модель тренировочной программы в вид для отображения.
        /// </summary>
        /// <param name="program">Модель.</param>
        /// <returns>Объект для отображения.</returns>
        public static ProgramResponseDTO MapToResponseDTO(TrainingProgram program)
        {
            return new ProgramResponseDTO()
            {
                Id = program.Id,
                Name = program.Name,
                Type = program.Type,
                IsActive = program.IsActive
            };
        }
    }
}
