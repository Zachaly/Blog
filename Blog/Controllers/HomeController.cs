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

        public IActionResult Index() 
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        public IActionResult Post(int id) 
        {
            var post = _repository.GetPost(id);
            return View(post);
        }

        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            if(id == null)
                return View(new Post());

            var post = _repository.GetPost((int)id);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post) 
        { 
            if(post.Id > 0)
                _repository.UpdatePost(post);
            else
                _repository.AddPost(post);

            if(await _repository.SaveChangesAsync())
                return RedirectToAction("Index");

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repository.RemovePost(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
