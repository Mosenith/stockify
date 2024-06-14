using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> CreateAsync(Comment commentModel);
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment?> UpdateAsync(int id, Comment commentModel);
    }
}