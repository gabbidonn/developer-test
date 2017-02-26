using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OrangeBricks.Web.Controllers.Viewings.ViewModels;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Viewings.Builders
{
    public class PropertyViewingsViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public PropertyViewingsViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public PropertyViewingsViewModel Build(int id)
        {
            var property = _context.Properties
                .Where(p => p.Id == id)
                .Include(x => x.Offers)
                .Include(v => v.Viewings)
                .SingleOrDefault();

            var viewings = property.Viewings ?? new List<Viewing>();

            return new PropertyViewingsViewModel
            {
                HasViewings = viewings.Any(),
                Viewings = viewings.Select(x => new ViewingViewModel
                {
                    Id = x.Id,
                    ViewingDate = x.ViewingDate,
                    ViewingStatus = x.ViewingStatus.ToString(),
                    IsPending = x.ViewingStatus == ViewingStatus.Pending
                }),
                PropertyId = property.Id, 
                PropertyType = property.PropertyType,
                StreetName = property.StreetName,
                NumberOfBedrooms = property.NumberOfBedrooms
            };
        }
    }
}