using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;
using System;

namespace OrangeBricks.Web.Controllers.Property.Builders
{
    public class BookViewingViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public BookViewingViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public BookViewingViewModel Build(int id)
        {
            var property = _context.Properties.Find(id);

            return new BookViewingViewModel
            {
                PropertyId = property.Id,
                ViewingDate = DateTime.Now                
            };
        }
    }
}