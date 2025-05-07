using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VideoSurveillanceApp.Server.Data;
using VideoSurveillanceApp.Pages;

namespace VideoSurveillanceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly VideoContext _context;

        public CameraController(VideoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCameras()
        {
            return Ok(await _context.Cameras.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddCamera([FromBody] Cameras camera)
        {
            if (string.IsNullOrEmpty(camera.Name) || string.IsNullOrEmpty(camera.RtspUrl))
                return BadRequest("Название и RTSP URL обязательны");

            _context.Cameras.Add(camera);
            await _context.SaveChangesAsync();
            return Ok(camera);
        }

        [HttpPost("start-record/{id}")]
        public async Task<IActionResult> StartRecord(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null)
                return NotFound("Камера не найдена");

            // Здесь будет вызов сервиса записи
            return Ok("Запись начата");
        }
    }
}