using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeBricks.Domain.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int Amount { get; set; }

        public OfferStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}