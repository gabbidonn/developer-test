using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeBricks.Domain.Models
{
    public class Viewing
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime ViewingDate { get; set; }

        public ViewingStatus ViewingStatus { get; set; }
    }
}