using Blog.Models;
using Blog.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        List<Post> GetAllPosts(string category);
        void RemovePost(int id);
        void UpdatePost(Post post);
        void AddPost(Post post);
        Task<bool> SaveChangesAsync();

        void AddSubComment(SubComment subcomment);
    }
}
