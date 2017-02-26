using System.Web.Mvc;
using OrangeBricks.Web.Attributes;
using OrangeBricks.Web.Controllers.Viewings.Builders;
using OrangeBricks.Web.Controllers.Viewings.Commands;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Controllers.Viewings
{
    [OrangeBricksAuthorize(Roles = "Seller")]
    public class ViewingsController : Controller
    {
        private readonly IOrangeBricksContext _context;

        public ViewingsController(IOrangeBricksContext context)
        {
            _context = context;
        }

        public ActionResult OnProperty(int id)
        {
            var builder = new PropertyViewingsViewModelBuilder(_context);
            var viewModel = builder.Build(id);

            return View(viewModel);
        }

        [HttpPost]        
        public ActionResult Accept(AcceptViewingCommand command)
        {
            var handler = new AcceptViewingCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }

        [HttpPost]
        public ActionResult Reject(RejectViewingCommand command)
        {
            var handler = new RejectViewingCommandHandler(_context);

            handler.Handle(command);

            return RedirectToAction("OnProperty", new { id = command.PropertyId });
        }
    }
}