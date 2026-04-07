using SV22T1020040.Models.Partner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1020040.DataLayers.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<bool> ValidateEmailAsync(string email, int supplierID = 0);
    }
}
