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
        /// <returns>Список всех упражнений в виде .</returns>
        // GET: api/Exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercise()
        {
            return await _context.Exercise.ToListAsync();
        }

        // GET: api/Exercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercise>> GetExercise(int id)
        {
            var exercise = await _context.Exercise.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            return exercise;
        }

        // PUT: api/Exercises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return BadRequest();
            }

            _context.Entry(exercise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Exercises
        [HttpPost]
        public async Task<ActionResult<Exercise>> PostExercise([FromBody] CreateExerciseDTO dto)
        {
            // Проверка на существования программы, к которому будет привязано упражнение.
            var program = await _context.TrainingProgram.FindAsync(dto.TrainingProgramId);
            if (program == null)
            {
                return BadRequest("There is no program with this id");
            }

            var exercise = new Exercise()
            {
                Name = dto.Name,
                TrainingProgramId = dto.TrainingProgramId,
                Program = await _context.TrainingProgram.FindAsync(dto.TrainingProgramId)
            };

            _context.Exercise.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExercise", new { id = exercise.Id }, exercise);
        }

        // DELETE: api/Exercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            _context.Exercise.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercise.Any(e => e.Id == id);
        }
    }
}
