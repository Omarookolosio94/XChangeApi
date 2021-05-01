using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Reviews
    {
        public int ReviewId { get; set; }
        public string CustomerReview { get; set; }
        public string Rating { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime? TimeAdded { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
