using Employee.api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DesignationMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public DesignationMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: api/DesignationMaster
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await (from d in _context.Designations
                                  join dept in _context.Departments on d.departmentId equals dept.departmentId
                                  select new
                                  {
                                      d.designationId,
                                      d.designationName,
                                      d.departmentId,
                                      departmentName = dept.departmentName
                                  }).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        //GEt by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var designation = await _context.Designations.FindAsync(id);
                if (designation == null)
                    return NotFound(new { Message = "Designation not found" });
                return Ok(designation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Designation model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Designations.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Designation created successfully", Data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //UPDATE
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Designation model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existing = await _context.Designations.FindAsync(model.designationId);
                if (existing == null)
                    return NotFound(new { Message = "Designation not found" });

                existing.departmentId = model.departmentId;
                existing.designationName = model.designationName;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Designation updated succesfuly", Data = existing });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var designation = await _context.Designations.FindAsync(id);
                if (designation == null)
                    return NotFound(new { Message = "Designation not found" });

                _context.Designations.Remove(designation);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Designation deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


        //FILTER
        [HttpGet("filter")]
        public async Task<IActionResult> filter(int? departmentId, string? search)
        {
            try
            {
                var query = _context.Designations.AsQueryable();

                if (departmentId.HasValue)
                    query = query.Where(x => x.departmentId == departmentId);

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(x => x.designationName.Contains(search));

                var data = await query.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)

            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}