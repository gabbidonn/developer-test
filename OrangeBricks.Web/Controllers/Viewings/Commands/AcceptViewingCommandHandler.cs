using System;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Controllers.Viewings.Commands
{
    public class AcceptViewingCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public AcceptViewingCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        public void Handle(AcceptViewingCommand command)
        {
            var viewing = _context.Viewings.Find(command.ViewingId);
            
            viewing.ViewingStatus = ViewingStatus.Confirmed;

            _context.SaveChanges();
        }
    }
}