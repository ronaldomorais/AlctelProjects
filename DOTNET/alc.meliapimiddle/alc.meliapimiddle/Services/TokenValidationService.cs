using alc.meliapimiddle.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alc.meliapimiddle.Services
{
    public class TokenValidationService
    {
        private RequestMeliAPI<TokenValidationData> _requestMeliAPI;
        private LoggerService _loggerService;

        public TokenValidationService()
        {
            _loggerService = new LoggerService();

            try
            {
                string url = ConfigurationManager.AppSettings["token_validation_url"];
                string authorizatonCode = ConfigurationManager.AppSettings["authorization_code"];

                _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "TokenValidationService.Constructor - url: {0}, authorization code: {1}", url, authorizatonCode);
                _requestMeliAPI = new RequestMeliAPI<TokenValidationData>(url, authorizatonCode);
            }
            catch (Exception ex)
            {
                _loggerService.WriteEventLog(EventLogEntryType.Error, 2001, "TokenValidationService.Constructor - Message: {0}, Trace: {1}", ex.Message, ex.StackTrace);
                _requestMeliAPI = null;
            }
        }

        public TokenValidationResultData IsTokenValidated(TokenValidationData tokenValidationData)
        {
            _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "[{0}] TokenValidationService.IsTokenValidated - pin: {1}, secret: {2}, type: {3}.", tokenValidationData.pin, tokenValidationData.pin, tokenValidationData.secret, tokenValidationData.type);

            TokenValidationResultData tokenValidationResultData = new TokenValidationResultData();
            tokenValidationResultData.valid = false;

            if (_requestMeliAPI != null)
            {
                var result = _requestMeliAPI.SendRequest("application/json", "POST", tokenValidationData);

                if (result != null)
                {                    
                    try
                    {
                        tokenValidationResultData.valid = result["valid"];
                        _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "[{0}] TokenValidationService.IsTokenValidated - pin: {1}, isValid: {2}", tokenValidationData.pin, tokenValidationData.pin, tokenValidationResultData.valid);
                    }
                    catch (Exception ex)
                    {
                        _loggerService.WriteEventLog(EventLogEntryType.Error, 2001, "[{0}] TokenValidationService.IsTokenValidated - Message: {1}, Trace: {2}", tokenValidationData.pin, ex.Message, ex.StackTrace);
                    }
                }
            }

            return tokenValidationResultData;
        }
    }
}
