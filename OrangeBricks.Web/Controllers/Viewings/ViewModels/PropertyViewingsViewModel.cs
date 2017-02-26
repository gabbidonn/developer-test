using System;
using System.Collections.Generic;

namespace OrangeBricks.Web.Controllers.Viewings.ViewModels
{
    public class PropertyViewingsViewModel
    {
        public string PropertyType { get; set; }
        public int NumberOfBedrooms{ get; set; }
        public string StreetName { get; set; }
        public bool HasViewings { get; set; }
        public IEnumerable<ViewingViewModel> Viewings { get; set; }
        public int PropertyId { get; set; }
    }

    public class ViewingViewModel
    {
        public int Id;
        public DateTime ViewingDate { get; set; }
        public string ViewingStatus { get; set; }
        public bool IsPending { get; set; }
    }
}