using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApplication.Models;
using FriendsNetwork.Repository.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Controllers
{
    public class CountryController : Controller
    {
        private readonly string _UriAPI = "https://localhost:44355/api/";

        private string ConnectionString { get; set; }

        public CountryController(IConfiguration configuration)
        {

            ConnectionString = configuration.GetConnectionString("BlobConnString");
        }

        // GET: CountryController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries");
            var response = client.Get<List<Country>>(request);

            if (response.Data == null)
            {
                response.Data = new List<Country>();
            }

            return View(response.Data);
        }

        // GET: CountryController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);
            return View(response.Data);
        }

        

        // GET: CountryController/Create
        public ActionResult Create()
        {
            return View();
        }
        public async Task<string> PostPhoto(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    BlobContainerClient blobServiceClient = new BlobContainerClient(ConnectionString, "blob-at");
                    blobServiceClient.CreateIfNotExists();
                    DateTime now = DateTime.UtcNow;
                    var blobClient = blobServiceClient.GetBlobClient($"{now.Ticks}-{file.FileName}");
                    await blobClient.UploadAsync(stream);
                    return blobClient.Uri.ToString();
                }
            }
            return null;
        }

        // POST: CountryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Country country, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(country);

                var photoUrl = await PostPhoto(file);

                if(photoUrl != null)
                {
                    var client = new RestClient();
                    var request = new RestRequest(_UriAPI + "Countries", DataFormat.Json);
                    country.PhotoUrl = photoUrl;
                    request.AddJsonBody(country);

                    var response = client.Post<Country>(request);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured, please try again later!");
                return View(country);
            }
        }

        // GET: CountryController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);

            return View(response.Data);
        }

        // POST: CountryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Country model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Put<Country>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CountryController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
            var response = client.Get<Country>(request);

            return View(response.Data);
        }

        // POST: CountryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Country model)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Countries/" + id, DataFormat.Json);
                request.AddJsonBody(model);

                var response = client.Delete<Country>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
