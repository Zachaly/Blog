using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{  
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repository = repo;
            _fileManager = fileManager;
        }


        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new PostViewModel());

            var post = _repository.GetPost((int)id);
            return View(new PostViewModel()
            {
                Title = post.Title,
                Body = post.Body,
                Id = post.Id,
                CurrentImage = post.Image,
                Description = post.Description,
                Tags = post.Tags,
                Category = post.Category,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel viewModel)
        {
            var post = new Post()
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Body = viewModel.Body,
                Description = viewModel.Description,
                Tags = viewModel.Tags,
                Category = viewModel.Category,
            };

            if (viewModel.Image == null)
                post.Image = viewModel.CurrentImage;
            else
            {
                if(!string.IsNullOrEmpty(viewModel.CurrentImage))
                    _fileManager.RemoveImage(viewModel.CurrentImage);

                post.Image = await _fileManager.SaveImage(viewModel.Image);
            }

            if (post.Id > 0)
                _repository.UpdatePost(post);
            else
                _repository.AddPost(post);

            if (await _repository.SaveChangesAsync())
                return RedirectToAction("Index");

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _fileManager.RemoveImage(_repository.GetPost(id).Image);
            _repository.RemovePost(id);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
