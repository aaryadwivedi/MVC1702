using APIuse1702.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace APIuse1702.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration config;
        private readonly string baseUrl; 
        public HomeController(IConfiguration configuration)
        {
            this.config = configuration;
            baseUrl = config["ApiBaseUrl"];
        }
        public async Task<IActionResult> Index()
        {
            List<User>? users = new List<User>();
            using(HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(this.baseUrl + "api/user"))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(apiresponse);
                }
            }
            if(users != null && users.Count > 0)
            {
                return View(users);
            }
            else
            {
                return View(new List<User>());
            }
        }

        public async Task<IActionResult> Deets(int id)
        {
            User users = new User();
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(this.baseUrl + "api/user/getusers/"+id))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    users = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(apiresponse);
                }
            }
            if (users != null)
            {
                return View(users);
            }
            else
            {
                return View(new User());
            }
        }
        public IActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            User? CreatedUser;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(this.baseUrl + "api/user/adduser/",content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    CreatedUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(apiresponse);
                }
            }
            if (CreatedUser != null)
            {
                TempData["message"] = $"User {CreatedUser.FirstName} created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult>Update(int id)
        {
            User getEditUser = new User();
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(this.baseUrl + "api/user/getusers/" + id))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    getEditUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(apiresponse);
                }
            }

            if (getEditUser != null)
            {
                return View(getEditUser);
            }
            return View(new User());
        }
        [HttpPost]

        public async Task<IActionResult> Update(User user)
        {
            User? UpdateUser;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user),
                Encoding.UTF8, "application/json");

                //var content = new MultipartFormDataContent();
                //content.Add(new StringContent(user.UserId.ToString()), "UserId");
                //content.Add(new StringContent(user.FirstName), "FirstName");
                //content.Add(new StringContent(user.LastName), "LastName");
                //content.Add(new StringContent(user.UserName), "UserName");
                //content.Add(new StringContent(user.Password), "Password");
                using (var response = await client.PutAsync(this.baseUrl + "api/User/updateuser/", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    UpdateUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(apiresponse);
                }
            }
            if (UpdateUser != null)
            {
                TempData["message"] = $"User {UpdateUser.FirstName} updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            User getDelUser=new User();
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(this.baseUrl + "api/user/getusers/" + id))
                {
                    string apiresponse=await response.Content.ReadAsStringAsync();
                    getDelUser=Newtonsoft.Json.JsonConvert.DeserializeObject<User> (apiresponse);
                }
            }
        
            if(getDelUser != null)
            {
                return View(getDelUser);
            }
            return View(new User());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            //int id=user.UserId;
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.DeleteAsync(this.baseUrl + "api/user/deleteuser/" + id))
                {
                    var apiresponse = await response.Content.ReadAsStringAsync();
                    //DeletedUser = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(apiresponse);
                }
            }
            return RedirectToAction("Index");
        }
    }
}