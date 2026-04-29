using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTrackingApp.Data;
using FitnessTrackingApp.Models;
using FitnessTrackingApp.DTO.ExerciseDTO;

namespace FitnessTrackingApp.Controllers
{
    /// <summary>
    /// Контроллер для работы с упражнениями пользователя.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        /// <summary>
        /// Настройки контекста БД.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Создает соответствующий контекст для работы с БД.
        /// </summary>
        /// <param name="context">Контекст определенной БД.</param>
        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает список всех упражнений.
        /// </summary>
        /// <returns>Код ответа HTTP и полная информация о всех упражнениях.</returns>
        /// 
        /// GET: api/Exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseResponseDTO>>> GetAll()
        {
            var exercises = await _context.Exercise
                .Select(e => ExerciseResponseDTO.MapToResponseDTO(e,_context))
                .ToListAsync();

            return Ok(exercises);
        }

        /// <summary>
        /// Поиск упражнения по названию.
        /// </summary>
        /// <param name="name">Название упражнения.</param>
        /// <returns>Код ответа HTTP и полная информация об упражнении.</returns>
        /// 
        /// GET: api/Exercises/Приседания
        [HttpGet("{name}")]
        public async Task<ActionResult<ExerciseResponseDTO>> GetByName(string name)
        {
            var exercise = await _context.Exercise
                .Where(e => e.Name.Equals(name))
                .FirstOrDefaultAsync();

            if (exercise == null)
            {
                return NotFound("There is no exercise wuth this name");
            }

            return Ok(exercise);
        }

        /// <summary>
        /// Обновляет данные упражнения.
        /// </summary>
        /// <param name="name">Название упражнения.</param>
        /// <param name="dto">Объект с данными для обновления.</param>
        /// <returns>Код ответа HTTP и полная информация об измененном объекте.</returns>
        /// 
        /// PUT: api/Exercises/Приседания
        [HttpPut("{name}")]
        public async Task<IActionResult> PutExercise(string name, [FromBody] UpdateExerciseDTO dto)
        {
            if (ExerciseExists(name))
            {
                return NotFound("There is no exercise wuth this name");
            }

            var exercise = await _context.Exercise
                .Where(e => e.Name.Equals(name))
                .FirstOrDefaultAsync();

            if (dto.Name != null)
            {
                exercise.Name = dto.Name;
            }
            if (dto.IsActive.HasValue)
            {
                exercise.IsActive = dto.IsActive.Value;
            }
            if (dto.TrainingProgramName != null)
            {
                exercise.TrainingProgramId = await _context.TrainingProgram
                    .Where(tp => tp.Name.Equals(dto.TrainingProgramName))
                    .Select(tp => tp.Id)
                    .FirstOrDefaultAsync();
            }

            _context.Entry(dto).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(ExerciseResponseDTO.MapToResponseDTO(exercise, _context));
        }

        /// <summary>
        /// Создает новое упражнение.
        /// </summary>
        /// <param name="dto">Объект с данными для создания упражнения.</param>
        /// <returns>Код ответа HTTP и полная информация о добавленном объекте.</returns>
        /// 
        /// POST: api/Exercises
        [HttpPost]
        public async Task<ActionResult<Exercise>> PostExercise([FromBody] CreateExerciseDTO dto)
        {
            // Проверка на существование программы, к которой будет привязано упражнение.
            var program = await _context.TrainingProgram
                .Where(p => p.Name.Equals(dto.TrainingProgramName))
                .FirstOrDefaultAsync();

            if (program == null)
            {
                return BadRequest("There is no program with this name");
            }

            var exercise = new Exercise()
            {
                Name = dto.Name,
                TrainingProgramId = program.Id,
                Program = program
            };

            _context.Exercise.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercise", new { id = exercise.Id }, exercise);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 
        /// DELETE: api/Exercises/Приседания
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteExercise(string name)
        {
            if (ExerciseExists(name))
            {
                return NotFound("There is no exercise with this name");
            }

            var exercise = await _context.Exercise
                .Where(e => e.Name.Equals(name))
                .FirstOrDefaultAsync();

            _context.Exercise.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Устанавливает факт существования упражнения с определенным названием.
        /// </summary>
        /// <param name="name">Название упражнения.</param>
        /// <returns>Факт о существовании/несуществовании такого объекта.</returns>
        private bool ExerciseExists(string name)
        {
            return _context.Exercise.Any(e => e.Name == name);
        }
    }
}
