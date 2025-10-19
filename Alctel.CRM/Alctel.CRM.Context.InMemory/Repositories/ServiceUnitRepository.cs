using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;

namespace Alctel.CRM.Context.InMemory.Repositories;

public class ServiceUnitRepository : BaseRepository<ServiceUnit>, IServiceUnitRepository
{
    public ServiceUnitRepository(CRMContext context) : base(context)
    {
    }
}
