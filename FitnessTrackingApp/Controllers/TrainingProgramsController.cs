using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTrackingApp.Data;
using FitnessTrackingApp.DTO.TrainingProgramDTO;
using FitnessTrackingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace FitnessTrackingApp.Controllers
{
    /// <summary>
    /// Контроллер для работы с тренировочными программами.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramsController : ControllerBase
    {
        /// <summary>
        /// Настройки контекста с БД.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Создает соответствующий контекст для работы с БД.
        /// </summary>
        /// <param name="context">Контекст определенной БД.</param>
        public TrainingProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает все тренировочные программы пользователя.
        /// </summary>
        /// <returns>Код ответа HTTP и список объектов для демонстрации имеющихся моделей.</returns>
        /// 
        /// GET: api/TrainingPrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramResponseDTO>>> GetAll()
        {
            // Отображаем только активные программы.
            var result = await _context.TrainingProgram
                .Select(item => ProgramResponseDTO.MapToResponseDTO(item))
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Получает тренировочную программу по названию.
        /// </summary>
        /// <param name="name">Название программы.</param>
        /// <returns>Код ответа HTTP и объект-демонстрация тренировочной программы.</returns>
        ///
        /// GET: api/TrainingPrograms/Функциональный тренинг
        [HttpGet("{name}")]
        public async Task<ActionResult<ProgramResponseDTO>> GetTrainingProgramByName(string name)
        {
            var trainingProgram = GetProgramByName(name);

            if (trainingProgram == null)
            {
                return NotFound("The training program does not exist.");
            }

            return Ok(ProgramResponseDTO.MapToResponseDTO(trainingProgram));
        }

        /// <summary>
        /// Обновляет данные тренировочной программы по названию.
        /// </summary>
        /// <param name="name">Название программы.</param>
        /// <param name="dto">Данные на редактирование из HTTP запроса.</param>
        /// <returns>Код ответа HTTP и объект-демонстрацию измененной тренировочной программы.</returns>
        /// 
        /// PUT: api/TrainingPrograms/5
        [HttpPut("{name}")]
        public async Task<ActionResult<ProgramResponseDTO>> PutTrainingProgram(string name, [FromBody] UpdateProgramDTO dto)
        {
            var program = GetProgramByName(name);

            if (program == null)
            {
                return NotFound();
            }

            if (dto.IsActive.HasValue)
            {
                program.IsActive = dto.IsActive.Value;
            }
            
            if (program.IsActive)
            {
                if (dto.Name != null)
                {
                    program.Name = dto.Name;
                }
                if (dto.Type != null)
                {
                    program.Type = dto.Type;
                }

                // Проверка на случай, если уже существует такая программа.
                var existingProgram = await _context.TrainingProgram
                    .AnyAsync(p => p.Name == program.Name && p.Type == program.Type);

                if (existingProgram)
                {
                    return BadRequest("There is already such an instance");
                }
            }

            _context.Entry(program).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(ProgramResponseDTO.MapToResponseDTO(program));
        }

        /// <summary>
        /// Создает новую тренировочную программу.
        /// </summary>
        /// <param name="dto">Данные на создание объекта из  HTTP запроса.</param>
        /// <returns>Код ответа HTTP и объект-демонстрацию добавленной тренировочной программы.</returns>
        /// 
        /// POST: api/TrainingPrograms
        [HttpPost]
        public async Task<ActionResult<TrainingProgram>> PostTrainingProgram([FromBody] CreateProgramDTO dto)
        {
            var program = new TrainingProgram()
            {
                Name = dto.Name,
                Type = dto.Type
            };

            // Проверка на случай, если уже существует такая программа.
            var existingProgram = await _context.TrainingProgram
                .AnyAsync(p => p.Name == program.Name && p.Type == program.Type);
            
            if (existingProgram)
            {
                return BadRequest("There is already such an instance");
            }

            _context.TrainingProgram.Add(program);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainingProgram", new { id = program.Id }, ProgramResponseDTO.MapToResponseDTO(program));
        }

        /// <summary>
        /// Деактивирует тренировочную программу по ID.
        /// </summary>
        /// <param name="id">ID программы.</param>
        /// <returns>Код ответа HTTP.</returns>
        /// 
        /// DELETE: api/TrainingPrograms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingProgram(int id)
        {
            var program = await _context.TrainingProgram.FindAsync(id);
            if (program == null || !program.IsActive)
            {
                return NotFound("Training program does not exist or is already inactivated");
            }

            program.IsActive = false;

            _context.Entry(program).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TrainingProgramExists(string name)
        {
            return _context.TrainingProgram.Any(e => e.Name == name);
        }

        private TrainingProgram? GetProgramByName(string name)
        {
            return _context.TrainingProgram
                .Where(p => p.Name.Equals(name))
                .FirstOrDefault();
        }
    }
}
