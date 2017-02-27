using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Controllers.Property.Commands
{
    public class CreatePropertyCommandHandler
    {
        private readonly IOrangeBricksContext _context;

        public CreatePropertyCommandHandler(IOrangeBricksContext context)
        {
            _context = context;
        }

        public void Handle(CreatePropertyCommand command)
        {
            var property = new Domain.Models.Property
            {
               PropertyType = command.PropertyType,
               StreetName = command.StreetName,
               Description = command.Description,
               NumberOfBedrooms = command.NumberOfBedrooms
            };

            property.SellerUserId = command.SellerUserId;

            _context.Properties.Add(property);

            _context.SaveChanges();
        }
    }
}