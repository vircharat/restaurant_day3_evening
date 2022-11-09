using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestaurantEntity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMVCUI.Controllers
{
    
    public class OrderController : Controller
    {
        public static List<Order> orders = new List<Order>();
        private IConfiguration _configuration;
    
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Food> foodresult = null;
            using (HttpClient client = new HttpClient())
            {


                string endPoint = _configuration["WebApiBaseUrl"] + "Food/GetFoods";//api controller name and httppost name given inside httppost in moviecontroller of api

                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {   //dynamic viewbag we can create any variable name in run time
                        var result = await response.Content.ReadAsStringAsync();
                        foodresult = JsonConvert.DeserializeObject<IEnumerable<Food>>(result);
                    }



                }
            }
            return View(foodresult);
           
        }
        [HttpGet]
        public async Task<IActionResult> AddOrder1(int FoodId)
        {
           Order order = new Order();
            order.FoodId = FoodId;

            Food food = null;
            using (HttpClient client = new HttpClient())
            {


                string endPoint = _configuration["WebApiBaseUrl"] + "Food/GetFoodById?foodId=" + FoodId;//movieId is apicontroleer passing argument name//api controller name and httppost name given inside httppost in moviecontroller of api

                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {   //dynamic viewbag we can create any variable name in run time
                        var result = await response.Content.ReadAsStringAsync();
                        food = JsonConvert.DeserializeObject<Food>(result);
                    }



                }
            }
            TempData["foodcost1"]= Convert.ToInt32(food.FoodCost);
            TempData.Keep();

            return View(order);

          
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder1(Order order)
        {
            ViewBag.status = "";
            /*            if (Request.Form.Files.Count > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            Request.Form.Files[0].CopyTo(ms);
                            Employeev.ImgPoster = ms.ToArray();
                        }*/
            //using grabage collection only for inbuilt classes
            using (HttpClient client = new HttpClient())
            {   var foodcost = Convert.ToInt32(TempData["foodcost1"]);
                TempData.Keep();
                order.OrderTotal = order.Quantity *foodcost;
                order.OrderStatus = false;

                
                orders.Add(order);
                TempData["status"] = true;
                while (Convert.ToBoolean(TempData["status"]))
                {
                    TempData.Keep();
                    return RedirectToAction("Index", "Order");
                }
                foreach (var item in orders)
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(orders), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Order/AddOrder";//api controller name and its function

                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {   //dynamic viewbag we can create any variable name in run time
                            ViewBag.status = "Ok";
                            ViewBag.message = "Order "+item.OrderId+" Added Successfull!!";
                        }

                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries";
                        }

                    }
                }
            }
            return View();
        }
     
        public async Task<IActionResult> ConfirmOrder()
        {
            TempData["status"] = false;
            TempData.Keep();
            using (HttpClient client = new HttpClient())
            {

                foreach (var item in orders)
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Order/AddOrder";//api controller name and its function

                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {   //dynamic viewbag we can create any variable name in run time
                            ViewBag.status = "Ok";
                            ViewBag.message = "Order " + item.OrderId + " Added Successfull!!";
                        }

                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries";
                        }

                    }

                }
                return RedirectToAction("Index", "Order");
            }
            return View();
        }
           
        
            [HttpGet]

        public async Task<IActionResult> GetAllFoods(Food food)
        {
            IEnumerable<Food> foodresult = null;
            using (HttpClient client = new HttpClient())
            {


                string endPoint = _configuration["WebApiBaseUrl"] + "Food/GetFoods";//api controller name and httppost name given inside httppost in moviecontroller of api

                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {   //dynamic viewbag we can create any variable name in run time
                        var result = await response.Content.ReadAsStringAsync();
                        foodresult = JsonConvert.DeserializeObject<IEnumerable<Food>>(result);
                    }



                }
            }
            return View(foodresult);
        }

        [HttpGet]

        public IActionResult GetOrders1()
        {
           
            return View(orders);
        }



    }
}
