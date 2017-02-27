using System;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Controllers.Viewings.Commands
{
    public class RejectViewingCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public RejectViewingCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        public void Handle(RejectViewingCommand command)
        {
            var viewing = _context.Viewings.Find(command.ViewingId);

            viewing.ViewingStatus = ViewingStatus.Rejected;

            _context.SaveChanges();
        }
    }
}