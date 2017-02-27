using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using OrangeBricks.Domain.Models;

namespace OrangeBricks.Domain
{
    public interface IOrangeBricksContext
    {
        IDbSet<Property> Properties { get; set; }
        IDbSet<Offer> Offers { get; set; }
        IDbSet<Viewing> Viewings { get; set; }

        void SaveChanges();
    }
}
