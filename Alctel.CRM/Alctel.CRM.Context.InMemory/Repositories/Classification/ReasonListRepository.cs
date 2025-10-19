using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Entities.Classification;
using Alctel.CRM.Context.InMemory.Interfaces.Classification;

namespace Alctel.CRM.Context.InMemory.Repositories.Classification;

public class ReasonListRepository : BaseRepository<ReasonList>, IReasonListRepository
{
    public ReasonListRepository(CRMContext context) : base(context)
    {
    }
}
