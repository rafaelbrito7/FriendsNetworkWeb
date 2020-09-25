using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApplication.Models;
using FriendsNetwork.Repository.Models;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace WebApplication.Controllers
{
    public class FriendController : Controller
    {
        private readonly string _UriAPI = "https://localhost:5001/api/";
        private string ConnectionString { get; set; }

        public FriendController(IConfiguration configuration)
        {
            
            ConnectionString = configuration.GetConnectionString("BlobConnString");
        }

        

        // GET: FriendController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "People");
            var response = client.Get<List<Person>>(request);

            if (response.Data == null)
            {
                response.Data = new List<Person>();
            }
           
            return View(response.Data);
        }

        // GET: FriendController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "People/" + id);
            var response = client.Get<Person>(request);

            var request2 = new RestRequest(_UriAPI + "People/Person/" + id);
            var people = client.Get<List<Person>>(request2);
            ViewData["People"] = people.Data;

            var request3 = new RestRequest(_UriAPI + "Friendships/" + id, DataFormat.Json);
            var friends = client.Get<List<Friendship>>(request3);
            ViewData["Friends"] = friends.Data;

            if (friends.Data != null)
            {
                var friendIds = new List<Guid>();
                foreach (var data in friends.Data)
                {
                    friendIds.Add(data.FriendId);
                }
                ViewData["FriendIds"] = friendIds;
            }
            return View(response.Data);
        }

        // GET: FriendController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateFriendship()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFriendship(Guid personId, Guid friendId)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "Friendships", DataFormat.Json);
                FriendshipRequest friendshipRequest = new FriendshipRequest(personId, friendId);
                request.AddJsonBody(friendshipRequest);

                var response = client.Post<FriendshipRequest>(request);

                return RedirectToAction(nameof(Index));
            } catch
            {
                ModelState.AddModelError(string.Empty, "An error occured, please try again later!");
                return View();
            }
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

        // POST: FriendController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Person person, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(person);

                var photoUrl = await PostPhoto(file);

                if (photoUrl != null)
                {
                    var client = new RestClient();
                    var request = new RestRequest(_UriAPI + "People", DataFormat.Json);
                    person.PhotoUrl = photoUrl;
                    request.AddJsonBody(person);

                    var response = client.Post<Person>(request);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occured, please try again later!");
                return View(person);
            }
        }

        // GET: FriendsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "People/" + id, DataFormat.Json);
            var response = client.Get<Person>(request);

            return View(response.Data);
        }

        public ActionResult AddFriendShip(Guid personId, Guid friendId)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friendships");
            var model = new Friendship { PersonId = personId, FriendId = friendId };
            request.AddJsonBody(model);
            var response = client.Post<Friendship>(request);

            return RedirectToAction("Details", "People", new { id = personId });
        }

        public ActionResult DeleteFriendShip(Guid personId, Guid friendId)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "Friendships/" + personId + "/" + friendId);
            var model = new Friendship { PersonId = personId, FriendId = friendId };
            var response = client.Delete<Friendship>(request);

            return RedirectToAction(nameof(Index));
        }

        // POST: FriendsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Person person)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(person);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "People/" + id, DataFormat.Json);
                request.AddJsonBody(person);

                var response = client.Put<Person>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendsController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "People/" + id, DataFormat.Json);
            var response = client.Get<Person>(request);

            return View(response.Data);
        }

        // POST: FriendsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Person person)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "People/" + id, DataFormat.Json);
                request.AddJsonBody(person);

                var response = client.Delete<Person>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
