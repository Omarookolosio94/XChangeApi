using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class ReviewsRepository : BaseRepository<Reviews>, IReviewsRepository
    {

        private static string ModuleName = "ReviewsRepository";

        public ReviewsRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> AddReview(Reviews review)
        {
            try
            {
                await InsertAsync(review);
                await Commit();
                return true;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "AddReviews", "Error Adding Reviews" + ex + "\n");
                throw;
            }
        }

        public async Task<List<Reviews>> GetAllReviews()
        {
            try
            {
                return Query().OrderByDescending(reviews => reviews.LastUpdateTime).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetAllReviews", "Error Fetching All Reviews. " + ex + "\n");
                throw;
            }
        }

        public async Task<Reviews> GetReview(int reviewId)
        {
            try
            {
                return Query().Where(o => o.ReviewId == reviewId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetReview", "Error Fetching single review: " + reviewId + "error : " + ex + "\n");
                throw;
            }
        }

        public async Task<List<Reviews>> GetReviewsOfProduct(int productId)
        {
            try
            {
                return Query().OrderByDescending(o => o.LastUpdateTime).Where(o => o.ProductId == productId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetReviewsOfProduct", "Error Fetching reviews of product: " + productId + " error : " + ex + "\n");
                throw;
            }

        }


        public async Task<Reviews> GetSingleReviewByUser(int userId, int reviewId)
        {
            try
            {
                return Query().Where(o => o.UserId == userId && o.ReviewId == reviewId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetSingleReviewByUser", "Error Fetching single review by user: " + userId + " review: " + reviewId + "error : " + ex + "\n");
                throw;
            }
        }


        public async Task<List<Reviews>> GetReviewsByUser(int userId)
        {
            try
            {
                return Query().OrderByDescending(o => o.LastUpdateTime).Where(o => o.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetReviewsByUser", "Error Fetching reviews of user: " + userId + " error : " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> UpdateReview(Reviews review)
        {
            try
            {
                Update(review);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateReview", "Error Updating reviews. Exception error: " + ex + "\n");
                throw;
            }
        }

        public async Task<bool> DeleteReview(int userId, int reviewId)
        {
            try
            {
                var review = Query().Where(o => o.ReviewId == reviewId && o.UserId == userId);

                if (review.Count() > 0)
                {
                    DeleteRange(review);
                    await Commit();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "DeleteReview", "Error Deleting Review " + ex + "\n");
                throw;
            }
        }

        public async Task<int> GetReviewsCount()
        {
            try
            {
                return Query().Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetReviewsCount", "Error getting reviews count exception error: " + ex + "/n");
                return 0;
            }
        }

        public async Task<int> GetReviewsOfProductCount(int productId)
        {
            try
            {
                return Query().Where(o => o.ProductId == productId).Count();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetReviewsOfProduct", "Error getting reviews count of product exception error: " + ex + "/n");
                return 0;
            }
        }
    }
}
