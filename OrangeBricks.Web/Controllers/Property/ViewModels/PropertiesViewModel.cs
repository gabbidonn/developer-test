using System.Collections.Generic;
using OrangeBricks.Web.Controllers.Property.Builders;

namespace OrangeBricks.Web.Controllers.Property.ViewModels
{
    public class PropertiesViewModel
    {
        public List<PropertyViewModel> Properties { get; set; }
        public PropertiesQuery SearchQuery { get; set; }
    }
}