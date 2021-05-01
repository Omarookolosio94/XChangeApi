using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IReviewsRepository
    {
        Task<bool> AddReview(Reviews review);
        Task<List<Reviews>> GetAllReviews();
        Task<Reviews> GetReview(int reviewId);
        Task<List<Reviews>> GetReviewsOfProduct(int productId);
        Task<List<Reviews>> GetReviewsByUser(int userId);
        Task<Reviews> GetSingleReviewByUser(int userId, int reviewId);
        Task<bool> UpdateReview(Reviews review);
        Task<bool> DeleteReview(int userId, int reviewId);
    }
}
