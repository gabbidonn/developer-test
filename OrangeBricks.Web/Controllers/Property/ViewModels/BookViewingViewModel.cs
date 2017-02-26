using System;

namespace OrangeBricks.Web.Controllers.Property.ViewModels
{
    public class BookViewingViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public DateTime ViewingDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}