using Blog.Models;
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

        public Repository(AppDbContext context)
            => _dbContext = context;

        public void AddPost(Post post) => _dbContext.Add(post);
        
        public List<Post> GetAllPosts() => _dbContext.Posts.ToList();
        public List<Post> GetAllPosts(string category) 
            => _dbContext.Posts.Where(post => post.Category.ToLower() == category.ToLower()).ToList();

        public Post GetPost(int id) => GetAllPosts().FirstOrDefault(post => post.Id == id);
        
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
