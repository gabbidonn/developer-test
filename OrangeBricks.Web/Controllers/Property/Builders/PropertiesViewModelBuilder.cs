using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Domain;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Web.Controllers.Property.Builders
{
    public class PropertiesViewModelBuilder
    {
        private readonly IOrangeBricksContext _context;

        public PropertiesViewModelBuilder(IOrangeBricksContext context)
        {
            _context = context;
        }

        public PropertiesViewModel Build(PropertiesQuery query)
        {
            var properties = _context.Properties
                .Where(p => p.IsListedForSale)
                .Include(o => o.Offers)
                .Include(v => v.Viewings);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                properties = properties.Where(x => x.StreetName.Contains(query.Search) 
                    || x.Description.Contains(query.Search));
            }

            return new PropertiesViewModel
            {
                // Show properties ordered by accepted offers first.
                Properties = properties
                    .ToList()
                    .Select(p => MapViewModel(p,query.UserId))
                    .OrderByDescending(p => p.BuyerOfferAccepted != null)
                    .ToList(),
                SearchQuery = query
            };
        }

        private static PropertyViewModel MapViewModel(Domain.Models.Property property, string userId)
        {
            var buyerAcceptedOffer = property.Offers?.Where(o => o.UserId == userId && o.Status == OfferStatus.Accepted)
                                            .Select(o => new AcceptedOfferViewModel()
                                            {
                                                Id = o.Id,
                                                Offer = o.Amount
                                            }).SingleOrDefault();

            var buyerBookedViewing = property.Viewings?.Where(v => v.UserId == userId 
                                                               && v.ViewingDate > DateTime.Now)
                                            .Select(v => new BookViewingViewModel()
                                            {
                                                Id = v.Id,
                                                ViewingDate = v.ViewingDate,
                                                IsConfirmed = v.ViewingStatus == ViewingStatus.Confirmed
                                            }).SingleOrDefault();

            return new PropertyViewModel
            {
                Id = property.Id,
                StreetName = property.StreetName,
                Description = property.Description,
                NumberOfBedrooms = property.NumberOfBedrooms,
                PropertyType = property.PropertyType,
                BuyerOfferAccepted =  buyerAcceptedOffer, // TODO: Ensure only one offer is accepted for a property at any time.      
                BuyerBookedViewing = buyerBookedViewing // TODO: Ensure only one viewing is allowed at any one time.
            };
        }
    }
}