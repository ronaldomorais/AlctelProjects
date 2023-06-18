using alc.meliapimiddle.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace alc.meliapimiddle.Services
{
    public class CaseCreationService
    {
        private RequestMeliAPI<CaseCreationData> _requestMeliAPI;
        private LoggerService _loggerService;

        public CaseCreationService()
        {
            _loggerService = new LoggerService();

            try
            {
                string url = ConfigurationManager.AppSettings["case_creation_url"];
                string authorizatonCode = ConfigurationManager.AppSettings["authorization_code"];
                _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "CaseCreationService.Constructor - url: {0}, authorization code: {1}", url, authorizatonCode);
                _requestMeliAPI = new RequestMeliAPI<CaseCreationData>(url, authorizatonCode);
            }
            catch (Exception ex)
            {
                _loggerService.WriteEventLog(EventLogEntryType.Error, 2001, "CaseCreationService.Constructor - Message: {0}, Trace: {1}", ex.Message, ex.StackTrace);
                _requestMeliAPI = null;
            }
        }

        public CaseCreationResultData RequestCaseCreation(CaseCreationData caseCreationData)
        {
            _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "[{0}] CaseCreationService.RequestCaseCreation - pin: {1}, secret: {2}, type: {3}, ivr_option: {4}, callid: {5}, provider: {6}.", caseCreationData.pin, caseCreationData.pin, caseCreationData.secret, caseCreationData.type, caseCreationData.ivr_option, caseCreationData.callid, caseCreationData.provider);

            CaseCreationResultData caseCreationResultData = new CaseCreationResultData();

            if (_requestMeliAPI != null)
            {
                var result = _requestMeliAPI.SendRequest("application/json", "POST", caseCreationData);

                if (result != null)
                {
                    try
                    {
                        int iCaseNumber = result["case_number"];
                        string sCaseNumber = string.Join("|", iCaseNumber.ToString().Select(n => new string(n, 1)).ToList());
                        caseCreationResultData.case_number = iCaseNumber;
                        caseCreationResultData.case_number_splitted = sCaseNumber;
                        caseCreationResultData.provider = result["provider"];
                        caseCreationResultData.from_faq = result["from_faq"];
                        caseCreationResultData.is_created = true;
                        _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "[{0}] CaseCreationService.RequestCaseCreation - pin: {1}, Case Created Successfully. Case_number: {2}, provider: {3}, from_faq: {4}.", caseCreationData.pin, caseCreationData.pin, caseCreationResultData.case_number, caseCreationResultData.provider, caseCreationResultData.from_faq);

                    }
                    catch (Exception ex)
                    {
                        string message = "";
                        try
                        {
                            message = result["message:"];
                        }
                        catch (Exception)
                        { }

                        _loggerService.WriteEventLog(EventLogEntryType.Error, 2001, "[{0}] CaseCreationService.RequestCaseCreation - callid: {1}, Server Message: {2}, Exception: {3}, Trace: {4}", caseCreationData.pin, caseCreationData.callid, message, ex.Message, ex.StackTrace);

                        caseCreationResultData.case_number = 0;
                        caseCreationResultData.case_number_splitted = "*";
                        caseCreationResultData.provider = "*";
                        caseCreationResultData.from_faq = false;
                        caseCreationResultData.is_created = false;
                    }
                }
            }

            return caseCreationResultData;
        }
    }
}
