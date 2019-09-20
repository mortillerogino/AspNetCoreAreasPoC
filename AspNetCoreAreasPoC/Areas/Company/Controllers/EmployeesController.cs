using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreAreasPoC.Areas.Company.Models;
using AspNetCoreAreasPoC.Data;
using System.IO;
using System.Drawing;

namespace AspNetCoreAreasPoC.Areas.Company.Controllers
{
    [Area("Company")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationContext _context;

        public EmployeesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Company/Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            var dtos = new List<EmployeeDto>();

            foreach (Employee e in employees)
            {
                var dto = new EmployeeDto(e);
                dtos.Add(dto);
            }

            return View(dtos);
        }

        // GET: Company/Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(new EmployeeDto(employee));
        }

        // GET: Company/Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var employee = new Employee
            {
                Name = employeeDto.Name
            };

            using (var memoryStream = new MemoryStream())
            {
                await employeeDto.Image.CopyToAsync(memoryStream);
                employee.Image = memoryStream.ToArray();
            }


            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Company/Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var dto = new EmployeeDto(employee);

            return View(dto);
        }

        // POST: Company/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (employee == null)
                    {
                        return NotFound();
                    }

                    employee.Name = dto.Name;
                    if (dto.Image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await dto.Image.CopyToAsync(memoryStream);
                            employee.Image = memoryStream.ToArray();
                        }
                    }

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(dto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Company/Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Company/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
