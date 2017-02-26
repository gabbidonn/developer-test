using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using OrangeBricks.Web.Controllers.Property.Builders;
using OrangeBricks.Web.Models;

namespace OrangeBricks.Web.Tests.Controllers.Property.Builders
{
    public static class ExtentionMethods
    {
        public static IDbSet<T> Initialize<T>(this IDbSet<T> dbSet, IQueryable<T> data) where T : class
        {
            dbSet.Provider.Returns(data.Provider);
            dbSet.Expression.Returns(data.Expression);
            dbSet.ElementType.Returns(data.ElementType);
            dbSet.GetEnumerator().Returns(data.GetEnumerator());
            return dbSet;
        }
    }

    [TestFixture]
    public class PropertiesViewModelBuilderTest
    {
        private IOrangeBricksContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<IOrangeBricksContext>();
        }

        [Test]
        public void BuildShouldReturnPropertiesWithMatchingStreetNamesWhenSearchTermIsProvided()
        {
            // Arrange
            var builder = new PropertiesViewModelBuilder(_context);

            var properties = new List<Models.Property>{
                new Models.Property{ StreetName = "Smith Street", Description = "", IsListedForSale = true },
                new Models.Property{ StreetName = "Jones Street", Description = "", IsListedForSale = true}
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>>()
                .Initialize(properties.AsQueryable());

            _context.Properties.Returns(mockSet);

            var query = new PropertiesQuery
            {
                Search = "Smith Street"
            };

            // Act
            var viewModel = builder.Build(query);

            // Assert
            Assert.That(viewModel.Properties.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildShouldReturnPropertiesWithMatchingDescriptionsWhenSearchTermIsProvided()
        {
            // Arrange
            var builder = new PropertiesViewModelBuilder(_context);

            var properties = new List<Models.Property>{
                new Models.Property{ StreetName = "", Description = "Great location", IsListedForSale = true },
                new Models.Property{ StreetName = "", Description = "Town house", IsListedForSale = true }
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>>()
                .Initialize(properties.AsQueryable());

            _context.Properties.Returns(mockSet);

            var query = new PropertiesQuery
            {
                Search = "Town"
            };

            // Act
            var viewModel = builder.Build(query);

            // Assert
            Assert.That(viewModel.Properties.Count, Is.EqualTo(1));
            Assert.That(viewModel.Properties.All(p => p.Description.Contains("Town")));
        }

        [Test]
        public void BuildShouldReturnPropertiesWithOfferAcceptedForBuyer()
        {
            // Arrange
            var builder = new PropertiesViewModelBuilder(_context);
            var userId = System.Guid.NewGuid().ToString();

            var offersAccepted = new List<Models.Offer>
            {
                new Models.Offer { Amount = 1000, Id = 1, Status = OfferStatus.Rejected, UserId = Guid.NewGuid().ToString()   },
                new Models.Offer { Amount = 2000, Id = 2, Status = OfferStatus.Accepted, UserId = userId }
            };

            var offersRejected = new List<Models.Offer>
            {
                new Models.Offer { Amount = 1000, Id = 3, Status = OfferStatus.Rejected, UserId = userId },
                new Models.Offer { Amount = 2000, Id = 4, Status = OfferStatus.Pending, UserId = Guid.NewGuid().ToString() }
            };
            
            var properties = new List<Models.Property>{
                new Models.Property{ StreetName = "", Description = "Great location", IsListedForSale = true, Offers = offersAccepted  },
                new Models.Property{ StreetName = "", Description = "Town house", IsListedForSale = true, Offers = offersRejected }
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>>()
                .Initialize(properties.AsQueryable());

            _context.Properties.Returns(mockSet);

            var query = new PropertiesQuery
            {
                Search = "",
                UserId = userId
            };

            // Act
            var viewModel = builder.Build(query);

            // Assert
            Assert.That(viewModel.Properties.Count, Is.EqualTo(2));
            Assert.That(viewModel.Properties.First().BuyerOfferAccepted != null, Is.True);
            Assert.That(viewModel.Properties.Last().BuyerOfferAccepted != null, Is.False);
        }

        [Test]
        public void BuildShouldReturnPropertiesWithViewingBookedByBuyer()
        {
            // Arrange
            var builder = new PropertiesViewModelBuilder(_context);
            var userId = System.Guid.NewGuid().ToString();

            var viewings = new List<Models.Viewing>
            {
                new Models.Viewing { Id = 1, ViewingDate = DateTime.Now.AddDays(1) , ViewingStatus = ViewingStatus.Rejected, UserId = Guid.NewGuid().ToString()   },
                new Models.Viewing { Id = 2, ViewingDate = DateTime.Now.AddDays(1), ViewingStatus = ViewingStatus.Confirmed, UserId = userId }
            };

            var viewings2 = new List<Models.Viewing>
            {
                new Models.Viewing { Id = 3, ViewingDate = DateTime.Now.AddDays(1), ViewingStatus = ViewingStatus.Rejected, UserId = userId },
                new Models.Viewing { Id = 4, ViewingDate = DateTime.Now.AddDays(1), ViewingStatus = ViewingStatus.Pending, UserId = Guid.NewGuid().ToString() }
            };

            var properties = new List<Models.Property>{
                new Models.Property{ Id = 1, StreetName = "", Description = "Great location", IsListedForSale = true, Viewings = viewings  },
                new Models.Property{ Id = 2, StreetName = "", Description = "Town house", IsListedForSale = true, Viewings = viewings2 }
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>>()
                .Initialize(properties.AsQueryable());

            _context.Properties.Returns(mockSet);

            var query = new PropertiesQuery
            {
                Search = "",
                UserId = userId
            };

            // Act
            var viewModel = builder.Build(query);

            // Assert
            Assert.That(viewModel.Properties.Count, Is.EqualTo(2));
            Assert.That(viewModel.Properties.Single(p => p.Id == 1).BuyerBookedViewing != null, Is.True);
            Assert.That(viewModel.Properties.Single(p => p.Id == 2).BuyerBookedViewing != null, Is.True);
        }

        [Test]
        public void BuildShouldReturnPropertiesWithViewingBookedByBuyerAndConfirmed()
        {
            // Arrange
            var builder = new PropertiesViewModelBuilder(_context);
            var userId = System.Guid.NewGuid().ToString();

            var viewings = new List<Models.Viewing>
            {
                new Models.Viewing { Id = 1, ViewingDate = DateTime.Now.AddDays(1) , ViewingStatus = ViewingStatus.Rejected, UserId = Guid.NewGuid().ToString()   },
                new Models.Viewing { Id = 2, ViewingDate = DateTime.Now.AddDays(1), ViewingStatus = ViewingStatus.Confirmed, UserId = userId }
            };

            var viewings2 = new List<Models.Viewing>
            {
                new Models.Viewing { Id = 3, ViewingDate = DateTime.Now.AddDays(1), ViewingStatus = ViewingStatus.Rejected, UserId = userId },
                new Models.Viewing { Id = 4, ViewingDate = DateTime.Now.AddDays(1), ViewingStatus = ViewingStatus.Pending, UserId = Guid.NewGuid().ToString() }
            };

            var properties = new List<Models.Property>{
                new Models.Property{ Id = 1, StreetName = "", Description = "Great location", IsListedForSale = true, Viewings = viewings  },
                new Models.Property{ Id = 2, StreetName = "", Description = "Town house", IsListedForSale = true, Viewings = viewings2 }
            };

            var mockSet = Substitute.For<IDbSet<Models.Property>>()
                .Initialize(properties.AsQueryable());

            _context.Properties.Returns(mockSet);

            var query = new PropertiesQuery
            {
                Search = "",
                UserId = userId
            };

            // Act
            var viewModel = builder.Build(query);

            // Assert
            Assert.That(viewModel.Properties.Single(p => p.Id == 1).BuyerBookedViewing.IsConfirmed, Is.True);
            Assert.That(viewModel.Properties.Single(p => p.Id == 2).BuyerBookedViewing.IsConfirmed, Is.False);
        }

    }
}
