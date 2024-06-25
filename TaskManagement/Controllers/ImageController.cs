using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.DataConnection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DbContextClass _context;

        public ImageController(DbContextClass context)
        {
            _context = context;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetImages()
        {
            var images = await _context.Images.ToListAsync();
            var imageDtos = images.Select(image => new ImageDto
            {
                Id = image.Id,
                Task_Id = image.Task_Id,
                Image_Path = image.Image_Path,
                Uploaded_At = image.Uploaded_At
            }).ToList();

            return imageDtos;
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDto>> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            var imageDto = new ImageDto
            {
                Id = image.Id,
                Task_Id = image.Task_Id,
                Image_Path = image.Image_Path,
                Uploaded_At = image.Uploaded_At
            };

            return imageDto;
        }

        // PUT: api/Image/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, ImageDto imageDto)
        {
            if (id != imageDto.Id)
            {
                return BadRequest();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            image.Task_Id = imageDto.Task_Id;
            image.Image_Path = imageDto.Image_Path;
            image.Uploaded_At = imageDto.Uploaded_At;

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/Image
        [HttpPost]
        public async Task<ActionResult<ImageDto>> PostImage(ImageDto imageDto)
        {
            var image = new Image
            {
                Task_Id = imageDto.Task_Id,
                Image_Path = imageDto.Image_Path,
                Uploaded_At = imageDto.Uploaded_At
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            imageDto.Id = image.Id;

            return CreatedAtAction("GetImage", new { id = image.Id }, imageDto);
        }

        // DELETE: api/Image/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
