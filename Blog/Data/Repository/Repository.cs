using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _dbContext;
        private const int PageSize = 4;

        public Repository(AppDbContext context)
            => _dbContext = context;

        public void AddPost(Post post) => _dbContext.Add(post);

        public void AddSubComment(SubComment subcomment) => _dbContext.SubComments.Add(subcomment);

        public List<Post> GetAllPosts() => _dbContext.Posts.ToList();
        public List<Post> GetAllPosts(string category) 
            => _dbContext.Posts.
            Where(post => post.Category.
            ToLower() == category.ToLower()).
            ToList();

        public Post GetPost(int id) 
            => _dbContext.Posts.
            Include(post => post.Comments).
            ThenInclude(comment => comment.SubComments).
            FirstOrDefault(post => post.Id == id);

        public IndexViewModel GetPostsOnPage(int pageNumber, string category)
        {
            var query = _dbContext.Posts.AsQueryable();

            if(!string.IsNullOrEmpty(category))
                query = query.Where(post => post.Category.ToLower() == category.ToLower());

            int pageCount = query.Count() / PageSize;
            bool canGoNext = pageNumber <= pageCount;

            query = query.
                    Skip(PageSize * (pageNumber - 1)).
                    Take(PageSize);

            return new IndexViewModel
            {
                Posts = query.ToList(),
                PageNumber = pageNumber,
                CanGoNextPage = canGoNext,
                Category = category
            };
        }

        public void RemovePost(int id)
        {
            Post post = GetPost(id);
            _dbContext.Posts.Remove(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if(await _dbContext.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public void UpdatePost(Post post) => _dbContext.Posts.Update(post);
    }
}
