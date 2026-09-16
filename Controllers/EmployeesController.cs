using CRUD_Practice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace CRUD_Practice.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync("api/Employees");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Employee>());
            }

            var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();

            return View(employees ?? new List<Employee>());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Employees/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var employee = await response.Content.ReadFromJsonAsync<Employee>();

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            await LoadDepartments();

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartments(employee.DepartmentId);
                return View(employee);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("api/Employees", employee);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("Email", "Employee with this email already exists.");

                await LoadDepartments(employee.DepartmentId);

                return View(employee);
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to create employee.");

                await LoadDepartments(employee.DepartmentId);

                return View(employee);
            }

            TempData["SuccessMessage"] = "Employee created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Employees/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var employee = await response.Content.ReadFromJsonAsync<Employee>();

            if (employee == null)
            {
                return NotFound();
            }

            await LoadDepartments(employee.DepartmentId);

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartments(employee.DepartmentId);

                return View(employee);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PutAsJsonAsync($"api/Employees/{id}", employee);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("Email", "Employee with this email already exists.");

                await LoadDepartments(employee.DepartmentId);

                return View(employee);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to update employee.");

                await LoadDepartments(employee.DepartmentId);

                return View(employee);
            }

            TempData["SuccessMessage"] = "Employee updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Employees/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var employee = await response.Content.ReadFromJsonAsync<Employee>();

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.DeleteAsync($"api/Employees/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete employee.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Employee deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Load Departments from Web API
        private async Task LoadDepartments(int? selectedDepartmentId = null)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync("api/Departments");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.DepartmentId = new SelectList(new List<Department>(), "Id", "DepartmentName");

                return;
            }

            var departments = await response.Content.ReadFromJsonAsync<List<Department>>();

            ViewBag.DepartmentId = new SelectList(departments ?? new List<Department>(), "Id", "DepartmentName", selectedDepartmentId);
        }
    }
}