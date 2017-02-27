using System;
using System.Data.Entity;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Tests.Controllers.Property.Commands
{
    [TestFixture]
    public class MakeOfferCommandHandlerTest
    {
        private CreatePropertyCommandHandler _handler;
        private MakeOfferCommandHandler _offerHandler;
        private IOrangeBricksContext _context;       

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(Substitute.For<IDbSet<Domain.Models.Property>>());
            _handler = new CreatePropertyCommandHandler(_context);
            _offerHandler = new MakeOfferCommandHandler(_context);
        }

        [Test]
        public void HandleShouldAddOfferToProperty()
        {
            // Arrange
            var offerCommand = new MakeOfferCommand()
            {
                PropertyId = 1,
                UserId = Guid.NewGuid().ToString(),
                Offer = 1
            };

            var command = new ListPropertyCommand
            {
                PropertyId = 1
            };

            var property = new Domain.Models.Property
            {
                Id = 1,
                Description = "Test Property"
               
            };

            _context.Properties.Find(1).Returns(property);

            // Act
            _offerHandler.Handle(offerCommand);

            // Assert
            var testProperty = _context.Properties.Find(1);

            Assert.That(testProperty.Offers.Count,Is.EqualTo(1));
            Assert.That(testProperty.Offers.First().UserId, Is.EqualTo(offerCommand.UserId));
            Assert.That(testProperty.Offers.First().Amount, Is.EqualTo(offerCommand.Offer));
        }
    }
}
