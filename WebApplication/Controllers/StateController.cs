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
    public class StateController : Controller
    {
        private readonly string _UriAPI = "https://localhost:44355/api/";

        private string ConnectionString { get; set; }

        public StateController(IConfiguration configuration)
        {

            ConnectionString = configuration.GetConnectionString("BlobConnString");
        }

        // GET: StateController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States");
            var response = client.Get<List<State>>(request);

            if (response.Data == null)
            {
                response.Data = new List<State>();
            }

            return View(response.Data);
        }

        // GET: StateController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
            var response = client.Get<State>(request);
            return View(response.Data);
        }

        // GET: StateController/Create
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

        // POST: StateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(State state, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(state);

                var photoUrl = await PostPhoto(file);

                if (photoUrl != null)
                {
                    var client = new RestClient();
                    var request = new RestRequest(_UriAPI + "States", DataFormat.Json);
                    state.PhotoUrl = photoUrl;
                    request.AddJsonBody(state);

                    var response = client.Post<State>(request);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured, please try again later!");
                return View(state);
            }
        }

        // GET: StateController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
            var response = client.Get<State>(request);

            return View(response.Data);
        }

        // POST: StateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, State state)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(state);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
                request.AddJsonBody(state);

                var response = client.Put<State>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StateController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
            var response = client.Get<State>(request);

            return View(response.Data);
        }

        // POST: StateController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, State state)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "States/" + id, DataFormat.Json);
                request.AddJsonBody(state);

                var response = client.Delete<State>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
