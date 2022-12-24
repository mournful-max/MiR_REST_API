using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Net.Cache;

using MiR_REST_API.Other;
using MiR_REST_API.RequestModels;
using MiR_REST_API.ResponseModels;

namespace MiR_REST_API
{
    public static class RequestHandler
    {
        private const int    _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS          = 1000;

        private const string _HTTP_WEB_REQUEST_JSON_CONTENT_TYPE            = "application/json";

        private const string _HTTP_WEB_REQUEST_AUTHORIZATION_HEADER_NAME    = "Authorization";
        private const string _HTTP_WEB_REQUEST_ACCEPT_LANGUAGE_HEADER_NAME  = "Accept-Language";

        private const string _HTTP_WEB_REQUEST_ACCEPT_LANGUAGE_HEADER_VALUE = "en_US";

        private const string _HTTP_WEB_REQUEST_URI_HEADING                  = "http://";
        private const string _HTTP_WEB_REQUEST_API_STRING                   = "api";
        private const string _HTTP_WEB_REQUEST_URI_SEPARATOR                = "/";

        private const string _HTTP_WEB_REQUEST_MISSION_QUEUE_METHOD         = "mission_queue";
        private const string _HTTP_WEB_REQUEST_REGISTERS_METHOD             = "registers";
        private const string _HTTP_WEB_REQUEST_STATUS_METHOD                = "status";


        private const string _HTTP_GET_REQUEST                              = "GET";
        private const string _HTTP_PUT_REQUEST                              = "PUT";
        private const string _HTTP_POST_REQUEST                             = "POST";
        private const string _HTTP_DELETE_REQUEST                           = "DELETE";

        private const string _MIR_STANDARD_API_VER                          = "v2.0.0";

        public static bool Process(string uri, string authorization, string httpMethod, out string jsonResponse, string jsonRequest = null, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS)
        {
            if (String.IsNullOrWhiteSpace(uri)
             || String.IsNullOrWhiteSpace(authorization)
             || String.IsNullOrWhiteSpace(httpMethod))
            {
                jsonResponse = "Uri, Authorization and HTTP method should be valid!";
                return false;
            }
            jsonResponse = null;
            HttpWebRequest request;
            try
            {
                request                                                        = (HttpWebRequest)WebRequest.Create(uri);
                request.Headers[_HTTP_WEB_REQUEST_AUTHORIZATION_HEADER_NAME]   = authorization;
                request.Headers[_HTTP_WEB_REQUEST_ACCEPT_LANGUAGE_HEADER_NAME] = _HTTP_WEB_REQUEST_ACCEPT_LANGUAGE_HEADER_VALUE;
                request.Timeout                                                = timeoutMs;
                request.Method                                                 = httpMethod;
                request.ContentType                                            = _HTTP_WEB_REQUEST_JSON_CONTENT_TYPE;
                request.CachePolicy                                            = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                if (!String.IsNullOrWhiteSpace(jsonRequest))
                {
                    byte[] jsonRequestBytes = Encoding.UTF8.GetBytes(jsonRequest);

                    request.ContentLength = jsonRequestBytes.Length;

                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(jsonRequestBytes, 0, jsonRequestBytes.Length);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }
            }
            catch (Exception exception)
            {
                jsonResponse = "HttpWebRequest configuring failure.";

                if (exception != null && !String.IsNullOrWhiteSpace(exception.Message))
                {
                    jsonResponse += " Exception: " + exception.Message.Replace(Environment.NewLine, " ") + ".";

                    if (exception.InnerException != null && !String.IsNullOrWhiteSpace(exception.InnerException.Message))
                    {
                        jsonResponse += " Inner exception: " + exception.InnerException.Message.Replace(Environment.NewLine, " ") + ".";
                    }
                }
                return false;
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        jsonResponse = streamReader.ReadToEnd();
                        return true;
                    }
                }
            }
            catch (WebException webException)
            {
                jsonResponse = "Error occurred when getting response.";
                try
                {
                    HttpWebResponse response = (HttpWebResponse)webException.Response;

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            jsonResponse += " " + reader.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    if (webException != null && !String.IsNullOrWhiteSpace(webException.Message))
                    {
                        jsonResponse += " WebException: " + webException.Message.Replace(Environment.NewLine, " ") + ".";

                        if (webException.InnerException != null && !String.IsNullOrWhiteSpace(webException.InnerException.Message))
                        {
                            jsonResponse += " Inner exception: " + webException.InnerException.Message.Replace(Environment.NewLine, " ") + ".";
                        }
                    }
                }
            }
            return false;
        }

        public static IntegerRegister GetIntegerRegister(string robotIp, string authorization, int registerId, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            if (registerId < 1 || registerId > 100)
            {
                throw new ArgumentException("Id of integerRegister shall be between 1 and 100.");
            }
            string registerIdString = registerId.ToString();

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_REGISTERS_METHOD
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + registerIdString;

            string jsonResponse = null;

            if (Process(uri, authorization, _HTTP_GET_REQUEST, out jsonResponse, null, timeoutMs))
            {
                return new IntegerRegister(JsonSerializer.Deserialize<Register>(jsonResponse, Helper.JsonSerializerOptions));
            }
            else
            {
                throw new Exception(jsonResponse);
            }
        }

        public static void SetIntegerRegister(string robotIp, string authorization, int registerId, int registerValue, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            if (registerId < 1 || registerId > 100)
            {
                throw new ArgumentException("Id of integerRegister shall be between 1 and 100.");
            }
            string registerIdString = registerId.ToString();

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_REGISTERS_METHOD
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + registerIdString;

            string jsonRequest  = JsonSerializer.Serialize(new IntegerRegisterValue(registerValue), Helper.JsonSerializerOptions);
            string jsonResponse = null;

            if (!Process(uri, authorization, _HTTP_PUT_REQUEST, out jsonResponse, jsonRequest, timeoutMs))
            {
                throw new Exception(jsonResponse);
            }
        }

        public static Status GetStatus(string robotIp, string authorization, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_STATUS_METHOD;

            string jsonResponse = null;

            if (Process(uri, authorization, _HTTP_GET_REQUEST, out jsonResponse, null, timeoutMs))
            {
                return JsonSerializer.Deserialize<Status>(jsonResponse, Helper.JsonSerializerOptions);
            }
            else
            {
                throw new Exception(jsonResponse);
            }
        }

        public static void SetState(string robotIp, string authorization, int stateValue, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_STATUS_METHOD;

            string jsonRequest  = JsonSerializer.Serialize(new StateValue(stateValue), Helper.JsonSerializerOptions);
            string jsonResponse = null;

            if (!Process(uri, authorization, _HTTP_PUT_REQUEST, out jsonResponse, jsonRequest, timeoutMs))
            {
                throw new Exception(jsonResponse);
            }
        }

        public static Mission[] GetMissionQueue(string robotIp, string authorization, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_MISSION_QUEUE_METHOD;

            string jsonResponse = null;

            if (Process(uri, authorization, _HTTP_GET_REQUEST, out jsonResponse, null, timeoutMs))
            {
                return JsonSerializer.Deserialize<Mission[]>(jsonResponse, Helper.JsonSerializerOptions);
            }
            else
            {
                throw new Exception(jsonResponse);
            }
        }

        public static void DeleteMission(string robotIp, string authorization, int missionId, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            string missionIdString = missionId.ToString();

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_MISSION_QUEUE_METHOD
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + missionIdString;

            string jsonResponse = null;

            if (Process(uri, authorization, _HTTP_DELETE_REQUEST, out jsonResponse, null, timeoutMs))
            {
                if (!String.IsNullOrWhiteSpace(jsonResponse))
                {
                    throw new Exception(jsonResponse);
                }
            }
            else
            {
                throw new Exception(jsonResponse);
            }
        }

        public static void AddMission(string robotIp, string authorization, string guid, int timeoutMs = _HTTP_WEB_REQUEST_STANDARD_TIMEOUT_MS, string apiVer = _MIR_STANDARD_API_VER)
        {
            RobotIpAuthorizationIsNullOrWhiteSpaceCheck(robotIp, authorization);

            string uri = _HTTP_WEB_REQUEST_URI_HEADING   + robotIp
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_API_STRING
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + apiVer
                       + _HTTP_WEB_REQUEST_URI_SEPARATOR + _HTTP_WEB_REQUEST_MISSION_QUEUE_METHOD;

            string jsonRequest  = JsonSerializer.Serialize(new PostMission() { MissionGuid = guid }, Helper.JsonSerializerOptions);
            string jsonResponse = null;

            if (!Process(uri, authorization, _HTTP_POST_REQUEST, out jsonResponse, jsonRequest, timeoutMs))
            {
                throw new Exception(jsonResponse);
            }
        }

        internal static void RobotIpAuthorizationIsNullOrWhiteSpaceCheck(string robotIp, string authorization)
        {
            if (String.IsNullOrWhiteSpace(robotIp))
            {
                throw new ArgumentException("Invalid IP address.");
            }
            if (String.IsNullOrWhiteSpace(authorization))
            {
                throw new ArgumentException("Invalid Authorization.");
            }
        }
    }
}
