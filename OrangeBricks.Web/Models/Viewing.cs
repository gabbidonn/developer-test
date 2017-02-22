using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeBricks.Web.Models
{
    public class Viewing
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime ViewingDate { get; set; }
    }
}