using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace alc.meliapimiddle.Services
{
    public sealed class RequestMeliAPI<T> where T : class
    {
        private readonly HttpWebRequest _httpWebRequest;
        private readonly string _authorizationCode;
        private LoggerService _loggerService;

        public RequestMeliAPI(string url, string authorizationCode)
        {
            _loggerService = new LoggerService();
            _authorizationCode = authorizationCode;
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        }

        public dynamic SendRequest(string contentType, string method, T body)
        {
            _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "RequestMeliAPI.SendRequest - Sending Request: content-type: {0}, method: {1}", contentType, method);
            string result = "";

            try
            {
                string json = new JavaScriptSerializer().Serialize(body);
                Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(json);

                _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "RequestMeliAPI.SendRequest - body: {0}", json);

                _httpWebRequest.ContentType = contentType;
                _httpWebRequest.Method = method;
                _httpWebRequest.ProtocolVersion = HttpVersion.Version11;
                _httpWebRequest.Headers.Add("Authorization", _authorizationCode);

                _httpWebRequest.ContentLength = data.Length;

                Stream streamWriter = _httpWebRequest.GetRequestStream();
                streamWriter.Write(data, 0, data.Length);
                streamWriter.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)_httpWebRequest.GetResponse();

                _loggerService.WriteEventLog(EventLogEntryType.Information, 1001, "RequestMeliAPI.SendRequest - body: {0}, Status Code Result: {1}", json, httpWebResponse.StatusCode.ToString());

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {                    
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        streamReader.Close();
                    }

                    if (!string.IsNullOrEmpty(result))
                        return new JavaScriptSerializer().Deserialize(result, typeof(object));
                }
            }
            catch (Exception ex)
            {
                _loggerService.WriteEventLog(EventLogEntryType.Error, 2001, "RequestMeliAPI.SendRequest - Message: {0}, Trace: {1}", ex.Message, ex.StackTrace);
                result = "{ \"message:\":\"" + ex.Message + "\" }";
            }

            return new JavaScriptSerializer().Deserialize(result, typeof(object)); ;
        }
    }
}