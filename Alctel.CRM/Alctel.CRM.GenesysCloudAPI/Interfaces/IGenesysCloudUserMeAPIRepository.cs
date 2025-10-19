using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.GenesysCloudAPI.Entities;

namespace Alctel.CRM.GenesysCloudAPI.Interfaces;

public interface IGenesysCloudUserMeAPIRepository
{
    Task<ApiResult> Request();
}
