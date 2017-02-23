using System;
using System.Data.Entity;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property.Commands;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Tests.Controllers.Property.Commands
{
    [TestFixture]
    public class BookViewingCommandHandlerTest
    {
        private BookViewingCommandHandler _bookViewingHandler;
        private IOrangeBricksContext _context;       

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
            _context.Properties.Returns(Substitute.For<IDbSet<Models.Property>>());
            _bookViewingHandler = new BookViewingCommandHandler(_context);
        }

        [Test]
        public void HandleShouldAddViewingToProperty()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var bookViewingCommand = new BookViewingCommand()
            {
                PropertyId = 1,
                ViewingDate = DateTime.Now,
                UserId = userId
                
            };

            var property = new Models.Property
            {
                Id = 1,
                Description = "Test Property"
            };

            _context.Properties.Find(1).Returns(property);

            // Act
            _bookViewingHandler.Handle(bookViewingCommand);

            // Assert
            var testProperty = _context.Properties.Find(1);

            Assert.That(testProperty.Viewings.Count,Is.EqualTo(1));
            Assert.That(testProperty.Viewings.First().UserId, Is.EqualTo(userId));
            Assert.That(testProperty.Viewings.First().ViewingDate, Is.EqualTo(bookViewingCommand.ViewingDate));
        }
    }
}
