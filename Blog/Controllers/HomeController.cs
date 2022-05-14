using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Repository;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        public HomeController(IRepository repo)
            => _repository = repo;

        public IActionResult Index() => View();
        public IActionResult Post() => View();
        [HttpGet]
        public IActionResult Edit() => View(new Post());
        [HttpPost]
        public async Task<IActionResult> Edit(Post post) 
        { 
            _repository.AddPost(post);

            if(await _repository.SaveChangesAsync())
                return RedirectToAction("Index");

            return View(post);
        }
    }
}
