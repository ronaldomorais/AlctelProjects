using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.Context.InMemory.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Alctel.CRM.Context.InMemory.Interfaces;

public interface IServiceUnitRepository : IBaseRepository<ServiceUnit>
{
   
}
