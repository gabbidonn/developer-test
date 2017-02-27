using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property.Builders;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Tests.Controllers.Property.Builders
{
    [TestFixture]
    public class BookViewingsViewModelBuilderTest
    {
        private IOrangeBricksContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
        }

        [Test]
        public void BuildShouldReturnBookViewingViewModelWithMatchingPropertyId()
        {
            // Arrange
            var builder = new BookViewingViewModelBuilder(_context);

            var viewingsProperty1 = new List<Viewing>{
                new Viewing{ ViewingDate = DateTime.Now },
                new Viewing{ ViewingDate = DateTime.Now.AddDays(1) }
            };

            var property = new Domain.Models.Property { Id = 1, StreetName = "Smith Street", Description = "", IsListedForSale = true };
            
            _context.Properties.Find(property.Id).Returns(property);
            
            // Act
            var viewModel = builder.Build(property.Id);

            // Assert
            Assert.That(viewModel.PropertyId, Is.EqualTo(property.Id));
        }

        [Test]
        public void BuildShouldReturnBookViewingViewModelWithValidViewingDate()
        {
            // Arrange
            var builder = new BookViewingViewModelBuilder(_context);

            var viewingsProperty1 = new List<Viewing>{
                new Viewing{ ViewingDate = DateTime.Now },
                new Viewing{ ViewingDate = DateTime.Now.AddDays(1) }
            };

            var property = new Domain.Models.Property { Id = 5000, StreetName = "Smith Street", Description = "", IsListedForSale = true };

            _context.Properties.Find(property.Id).Returns(property);

            // Act
            var viewModel = builder.Build(property.Id);

            // Assert
            Assert.That(viewModel.ViewingDate, Is.AtMost(DateTime.Now));
        }
    }
}
