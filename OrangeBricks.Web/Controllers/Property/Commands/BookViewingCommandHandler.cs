using System;
using System.Collections.Generic;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    public class BookViewingCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public BookViewingCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        public void Handle(BookViewingCommand command)
        {
            var property = _context.Properties.Find(command.PropertyId);

            var viewing = new Viewing
            {
                ViewingDate = command.ViewingDate,
                UserId = command.UserId
            };

            if (property.Viewings == null)
            {
                property.Viewings = new List<Viewing>();
            }
                
            property.Viewings.Add(viewing);
            
            _context.SaveChanges();
        }
    }
}