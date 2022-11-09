using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestaurantEntity;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestaurantMVCUI.Controllers
{
    public class EmployeeController : Controller
    {
        private IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login1()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login1(Employee employee)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Employee/Login";
                using (var response = await client.PostAsync(endPoint, content))
                {
                    string employee_designation = await response.Content.ReadAsStringAsync();
                    TempData["employee_designation"] = employee_designation;
                    TempData.Keep();
                    if (employee_designation == "CHEF")
                        return RedirectToAction("Index", "Chef");
                    else if (employee_designation == "HEADCHEF")
                        return RedirectToAction("Index", "HeadChef");
                    else if (employee_designation == "HALLMANAGER")
                        return RedirectToAction("Index", "HallManager");
                    else if (employee_designation == "ADMIN")
                        return RedirectToAction("Index", "Admin1");
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong credentials!";
                    }
                }
            }
            return View();



        }

    }
}
