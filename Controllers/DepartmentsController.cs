using CRUD_Practice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRUD_Practice.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DepartmentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync("api/Departments");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Department>());
            }

            var departments = await response.Content.ReadFromJsonAsync<List<Department>>();

            return View(departments ?? new List<Department>());
        }


        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Departments/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var department = await response.Content.ReadFromJsonAsync<Department>();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }


        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("api/Departments", department);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("DepartmentName", "Department name already exists.");

                return View(department);
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to create department.");

                return View(department);
            }

            TempData["SuccessMessage"] = "Department created successfully.";

            return RedirectToAction(nameof(Index));
        }


        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Departments/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var department = await response.Content.ReadFromJsonAsync<Department>();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }


        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            // Convert MVC model to API DTO
            var dto = new DepartmentUpdateDto
            {
                DepartmentName = department.DepartmentName
            };

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PutAsJsonAsync($"api/Departments/{id}", dto);

            // Duplicate department name
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ModelState.AddModelError("DepartmentName", "Department name already exists.");

                return View(department);
            }

            // Department not found
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Unable to update department.");

                return View(department);
            }

            TempData["SuccessMessage"] ="Department updated successfully.";

            return RedirectToAction(nameof(Index));
        }


        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.GetAsync($"api/Departments/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var department = await response.Content.ReadFromJsonAsync<Department>();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }


        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.DeleteAsync($"api/Departments/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to delete department.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Department deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}