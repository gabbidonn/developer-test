using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OrangeBricks.Web.Controllers.Property.ViewModels;
using OrangeBricks.Web.Models;

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
                .Include(o => o.Offers);

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
                    .OrderByDescending(p => p.UserHasOfferAccepted)
                    .ToList(),
                Search = query.Search
            };
        }

        private static PropertyViewModel MapViewModel(Models.Property property, string userId)
        {
            var offers = property.Offers ?? new List<Offer>();

            return new PropertyViewModel
            {
                Id = property.Id,
                StreetName = property.StreetName,
                Description = property.Description,
                NumberOfBedrooms = property.NumberOfBedrooms,
                PropertyType = property.PropertyType,
                UserHasOfferAccepted = offers.Any(o => o.UserId == userId && o.Status == OfferStatus.Accepted),
                UserOfferAccepted =  offers.Where(o => o.UserId == userId && o.Status == OfferStatus.Accepted)
                                            .Select(o => new AcceptedOfferViewModel()
                                            {
                                                Id = o.Id,
                                                Offer = o.Amount
                                            }).LastOrDefault() // TODO: Ensure only one offer is accepted for a property at any time.                                               
            };
        }
    }
}