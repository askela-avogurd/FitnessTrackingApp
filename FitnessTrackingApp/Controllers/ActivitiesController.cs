using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTrackingApp.Data;
using FitnessTrackingApp.DTO.ActivityDTO;
using FitnessTrackingApp.Models;
using FitnessTrackingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FitnessTrackingApp.DTO.ActivityDTO.ActivityResponseDTO;

namespace FitnessTrackingApp.Controllers
{
    /// <summary>
    /// Контроллер для работы с активностями пользователя.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        /// <summary>
        /// Настройки контекста БД.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Создает соответствующий контекст для работы с БД.
        /// </summary>
        /// <param name="context">Контекст определенной БД.</param>
        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получает список активностей по дате.
        /// </summary>
        /// <param name="date">Дата, без времени, в виде YYYY-MM-DD из строки запроса.</param>
        /// <returns>Код ответа HTTP и общие параметры активностей одного дня и список упражнений с индивидуальными параметрами.</returns>
        /// 
        /// GET: api/Activities/by-date?date=2025-04-26
        [HttpGet("by-date")]
        public async Task<ActionResult<DailySummaryDTO>> GetByDate([FromQuery] DateTime date)
        {
            var activity = _context.Activity
                .Where(a => a.Date == date.Date);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(ActivityResponseDTO.MapToResponseDTO(date, _context));
        }


        /// <summary>
        /// Получает список активностей за месяц.
        /// </summary>
        /// <param name="year">Номер месяца целым числом из строки запроса.</param>
        /// <param name="month">Год целым числом в виде YYYY из строки запроса.</param>
        /// <returns>Код ответа HTTP и список активностей за период, сгруппированных по дням: общие параметры активностей одного дня и список упражнений с индивидуальными параметрами.</returns>
        /// 
        /// GET: api/Activities/by-month?year=2025&month=4
        [HttpGet("by-month")]
        public async Task<ActionResult<IEnumerable<DailySummaryDTO>>> GetByMonth(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var activities = await _context.Activity
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .Select(day => ActivityResponseDTO.MapToResponseDTO(day.Date, _context))
                .ToListAsync();

            return Ok(activities);
        }

        /// <summary>
        /// Получает список активностей за все время.
        /// </summary>
        /// <returns>Код ответа HTTP и список активностей за период, сгруппированных по дням: общие параметры активностей одного дня и список упражнений с индивидуальными параметрами.</returns>
        /// 
        /// GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailySummaryDTO>>> GetAll()
        {
            var allActivities = await _context.Activity
                .Select(day => MapToResponseDTO(day.Date, _context))
                .ToListAsync();

            return Ok(allActivities);
        }

        // * Поиск по ID по заданию не требуется, на всякий случай оставлю.
        /// <summary>
        /// Получает активность по ID.
        /// </summary>
        /// <param name="id">ID активности.</param>
        /// <returns>Код ответа HTTP и общие параметры активностей одного дня и список упражнений с индивидуальными параметрами.</returns>
        ///
        /// GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DailySummaryDTO>> GetActivity(int id)
        {
            var activity = await _context.Activity.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return Ok(MapToResponseDTO(activity.Date, _context));
        }

        /// <summary>
        /// Обновляет данные активности.
        /// </summary>
        /// <param name="id">ID активности.</param>
        /// <param name="dto">Данные для обновления.</param>
        /// <returns>Код ответа HTTP и общие параметры активностей одного дня и список упражнений с индивидуальными параметрами.</returns>
        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DailySummaryDTO>> UpdateActivity(int id, [FromBody] UpdateActivityDTO dto)
        {
            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            // Обновляем поля при необходимости.
            if (dto.Minutes.HasValue)
            {
                activity.Minutes = dto.Minutes.Value;
            }

            if (dto.Notes != null)
            {
                activity.Notes = dto.Notes;
            }

            // Запрашиваем обновление всех данных сущности кроме ключа.
            _context.Entry(activity).State = EntityState.Modified;

            // Пытаемся сохранить данные.
            await _context.SaveChangesAsync();

            return Ok(MapToResponseDTO(activity.Date, _context));
        }

        /// <summary>
        /// Создает новую активность.
        /// </summary>
        /// <param name="dto">Данные на создание активности из тела запроса.</param>
        /// <returns>Код ответа HTTP и общие параметры активностей одного дня и список упражнений с индивидуальными параметрами.</returns>
        /// 
        //// POST: api/Activities
        [HttpPost]
        public async Task<ActionResult<DailySummaryDTO>> CreateActivity([FromBody] CreateActivityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Проверка, существует и активно ли упражнение.
            var exercise = await _context.Exercise.FindAsync(dto.ExerciseId);
            if (exercise == null || !exercise.IsActive)
            {
                return BadRequest("The exercise does not exist or is inactive");
            }

            // Проверка лимита 1440 минут.
            var totalForDay = await _context.Activity
                .Where(a => a.Date.Date == dto.Date.Date)
                .SumAsync(a => a.Minutes);

            if (totalForDay + dto.Minutes > 1440)
                return BadRequest($"The limit has been exceeded. {totalForDay} minutes have already been accumulated");

            // Создаем активность по внесенным данным.
            var activity = new Activity()
            {
                Date = dto.Date.Date,
                Minutes = dto.Minutes,
                Notes = dto.Notes,
                ExerciseId = dto.ExerciseId,
                Exercise = await _context.Exercise.FindAsync(dto.ExerciseId)
            };

            _context.Activity.Add(activity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivity", new { id = activity.Id }, ActivityResponseDTO.MapToResponseDTO(activity.Date, _context));
        }

        /// <summary>
        /// Удаляет активность по ID.
        /// </summary>
        /// <param name="id">ID активности.</param>
        /// <returns>Код ответа HTTP.</returns>
        /// 
        /// DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Activity.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
