using alc.meliapimiddle.Models;
using alc.meliapimiddle.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace alc.meliapimiddle
{
    /// <summary>
    /// Summary description for MeliWebService
    /// </summary>
    [WebService(Namespace = "http://alctel.meliwebservice.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MeliWebService : System.Web.Services.WebService
    {
        private TokenValidationService _tokenValidationService;
        private CaseCreationService _caseCreationService;
        private LoggerService _loggerService;

        public MeliWebService()
        {
            _loggerService = new LoggerService();
        }

        [WebMethod]
        public TokenValidationResultData IsTokenValidated(int pin, string secret, string type)
        {
            _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "[{0}] MeliWebService.IsTokenValidated - pin: {1}, secret: {2}, type: {3}.", pin, pin, secret, type);
            _tokenValidationService = new TokenValidationService();
            return _tokenValidationService.IsTokenValidated(new TokenValidationData() { pin = pin, secret = secret, type = type });
        }

        [WebMethod]
        public CaseCreationResultData CreateCase(string pin, string secret, string type, int ivr_option, string callid, string provider)
        {
            _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "[{0}] MeliWebService.CreateCase - pin: {1}, secret: {2}, type: {3}, ivr_option: {4} callid: {5}, provider: {6}.", pin, pin, secret, type, ivr_option, callid, provider);
            _caseCreationService = new CaseCreationService();
            return _caseCreationService.RequestCaseCreation(new CaseCreationData { pin = pin, secret = secret, type = type, ivr_option = ivr_option, callid = callid, provider = provider });
        }
    }
}
