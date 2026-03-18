using Employee.api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        public EmployeeMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // Normal GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.Employees.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return NotFound(new { Message = "Employee not found" });
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool exists = await _context.Employees.AnyAsync(x => x.contactNo == model.contactNo || x.email == model.email);

                if (exists)
                    return BadRequest(new { Message = "Contact No or Email already exists" });

                model.createdDate = DateTime.Now;
                model.modifiedDate = DateTime.Now;

                _context.Employees.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Employee created successfully", Data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeModel model)
        {
            try
            {
                if (id != model.employeeId)
                    return BadRequest("ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existing = await _context.Employees.FindAsync(id);
                if (existing == null)
                    return NotFound(new { Message = "Employee not found" });

                bool exists = await _context.Employees.AnyAsync(x => (x.contactNo == model.contactNo || x.email == model.email) && x.employeeId != id);

                if (exists)
                    return BadRequest(new { Message = "Contact No or Email already exists" });

                existing.name = model.name;
                existing.contactNo = model.contactNo;
                existing.email = model.email;
                existing.city = model.city;
                existing.state = model.state;
                existing.pincode = model.pincode;
                existing.address = model.address;
                existing.designationName = model.designationName;
                existing.designationId = model.designationId;
                existing.modifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Employee updated successfully", Data = existing });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                    return NotFound(new { Message = "Employee not found" });

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //FILTER + SORT + PAGINATION
        [HttpGet("filter")]
        public async Task<IActionResult> Filter(
            string? search,
            int? designationId,
            string? sortBy = "name",
            string? sortDir = "asc",
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            try
            {
                var query = _context.Employees.AsQueryable();

                //Search
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x =>
                    x.name.Contains(search) ||
                    x.contactNo.Contains(search) ||
                    x.email.Contains(search) ||
                    x.city.Contains(search));
                }

                // Filter
                if (designationId.HasValue)
                {
                    query = query.Where(x => x.designationId == designationId);
                }

                // Sorting
                switch (sortBy?.ToLower())
                {
                    case "name":
                        query = sortDir == "desc" ? query.OrderByDescending(x => x.name) : query.OrderBy(x => x.name);
                        break;

                    case "createddate":
                        query = sortDir == "desc" ? query.OrderByDescending(x => x.createdDate) : query.OrderBy(x => x.createdDate);
                        break;

                    default:
                        query = query.OrderBy(x => x.employeeId);
                        break;
                }

                // Pagination 
                int totalRecords = await query.CountAsync();
                var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return Ok(new
                {
                    totalRecords = totalRecords,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _context.Employees.FirstOrDefaultAsync(x => x.email == model.email && x.contactNo == model.contactNo);

                if (user == null)
                    return Unauthorized(new { Message = "Invalid Credentials" });

                return Ok(new
                {
                    message = "Login successful",
                    data = new
                    {
                        user.employeeId,
                        user.name,
                        user.email,
                        user.contactNo,
                        user.designationName,
                        user.designationId,
                        user.role
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}