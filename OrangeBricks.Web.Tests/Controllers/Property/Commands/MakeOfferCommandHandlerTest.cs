using System.Data.Entity;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Tests.Controllers.Property.Commands
{
    [TestFixture]
    public class MakeOfferCommandHandlerTest
    {
        private CreatePropertyCommandHandler _handler;
        private MakeOfferCommandHandler _offerHandler;
        private IOrangeBricksContext _context;
        private IDbSet<Models.Property> _properties;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(Substitute.For<IDbSet<Models.Property>>());
            _handler = new CreatePropertyCommandHandler(_context);
            _offerHandler = new MakeOfferCommandHandler(_context);
            _properties = Substitute.For<IDbSet<Models.Property>>();
        }

        [Test]
        public void HandleShouldAddOffer()
        {
            // Arrange
            var offerCommand = new MakeOfferCommand();

            var command = new ListPropertyCommand
            {
                PropertyId = 1
            };

            var property = new Models.Property
            {
                Description = "Test Property",
                IsListedForSale = false,


            };

            _properties.Find(1).Returns(property);

            // Act

            _offerHandler.Handle(offerCommand);

            // Assert
            _context.Properties.Received(1).Add(Arg.Any<Models.Property>());
        }
    }
}
