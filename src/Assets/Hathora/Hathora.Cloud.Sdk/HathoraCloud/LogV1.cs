
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasyapi.dev). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraCloud
{
    using HathoraCloud.Models.Operations;
    using HathoraCloud.Models.Shared;
    using HathoraCloud.Utils;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System;
    using UnityEngine.Networking;

    /// <summary>
    /// Operations to get logs by <a href="https://hathora.dev/docs/concepts/hathora-entities#application">applications</a>, <a href="https://hathora.dev/docs/concepts/hathora-entities#process">processes</a>, and <a href="https://hathora.dev/docs/concepts/hathora-entities#deployment">deployments</a>. We store 20GB of logs data.
    /// </summary>
    public interface ILogV1
    {

        /// <summary>
        /// Returns a stream of logs for an <a href="https://hathora.dev/docs/concepts/hathora-entities#application">application</a> using `appId`.
        /// </summary>
        Task<GetLogsForAppResponse> GetLogsForAppAsync(GetLogsForAppRequest? request = null);

        /// <summary>
        /// Returns a stream of logs for a <a href="https://hathora.dev/docs/concepts/hathora-entities#deployment">deployment</a> using `appId` and `deploymentId`.
        /// </summary>
        Task<GetLogsForDeploymentResponse> GetLogsForDeploymentAsync(GetLogsForDeploymentRequest? request = null);

        /// <summary>
        /// Returns a stream of logs for a <a href="https://hathora.dev/docs/concepts/hathora-entities#process">process</a> using `appId` and `processId`.
        /// </summary>
        Task<GetLogsForProcessResponse> GetLogsForProcessAsync(GetLogsForProcessRequest? request = null);
    }

    /// <summary>
    /// Operations to get logs by <a href="https://hathora.dev/docs/concepts/hathora-entities#application">applications</a>, <a href="https://hathora.dev/docs/concepts/hathora-entities#process">processes</a>, and <a href="https://hathora.dev/docs/concepts/hathora-entities#deployment">deployments</a>. We store 20GB of logs data.
    /// </summary>
    public class LogV1: ILogV1
    {
        public SDKConfig Config { get; private set; }
        private const string _target = "unity";
        private const string _sdkVersion = "0.26.0";
        private const string _sdkGenVersion = "2.195.2";
        private const string _openapiDocVersion = "0.0.1";
        private const string _userAgent = "speakeasy-sdk/unity 0.26.0 2.195.2 0.0.1 hathora-cloud";
        private string _serverUrl = "";
        private ISpeakeasyHttpClient _defaultClient;
        private ISpeakeasyHttpClient _securityClient;

        public LogV1(ISpeakeasyHttpClient defaultClient, ISpeakeasyHttpClient securityClient, string serverUrl, SDKConfig config)
        {
            _defaultClient = defaultClient;
            _securityClient = securityClient;
            _serverUrl = serverUrl;
            Config = config;
        }
        

        [Obsolete("This method will be removed in a future release, please migrate away from it as soon as possible")]
        public async Task<GetLogsForAppResponse> GetLogsForAppAsync(GetLogsForAppRequest? request = null)
        {
            request.AppId ??= Config.AppId;
            string baseUrl = this.Config.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/logs/v1/{appId}/all", request);
            

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);
            
            
            var client = _securityClient;
            
            var httpResponse = await client.SendAsync(httpRequest);
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    var errorMsg = httpResponse.error;
                    httpRequest.Dispose();
                    throw new Exception(errorMsg);
            }

            var contentType = httpResponse.GetResponseHeader("Content-Type");
            
            var response = new GetLogsForAppResponse
            {
                StatusCode = (int)httpResponse.responseCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            
            if((response.StatusCode == 200))
            {
                if(Utilities.IsContentTypeMatch("text/plain",response.ContentType))
                {
                    response.Res = httpResponse.downloadHandler.text;
                }
                
                return response;
            }
            if((response.StatusCode == 404))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {
                    response.ApiError = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = new JsonConverter[] { new FlexibleObjectDeserializer(), new DateOnlyConverter(), new EnumSerializer() }});
                }
                
                return response;
            }
            return response;
        }
        

        [Obsolete("This method will be removed in a future release, please migrate away from it as soon as possible")]
        public async Task<GetLogsForDeploymentResponse> GetLogsForDeploymentAsync(GetLogsForDeploymentRequest? request = null)
        {
            request.AppId ??= Config.AppId;
            string baseUrl = this.Config.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/logs/v1/{appId}/deployment/{deploymentId}", request);
            

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);
            
            
            var client = _securityClient;
            
            var httpResponse = await client.SendAsync(httpRequest);
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    var errorMsg = httpResponse.error;
                    httpRequest.Dispose();
                    throw new Exception(errorMsg);
            }

            var contentType = httpResponse.GetResponseHeader("Content-Type");
            
            var response = new GetLogsForDeploymentResponse
            {
                StatusCode = (int)httpResponse.responseCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            
            if((response.StatusCode == 200))
            {
                if(Utilities.IsContentTypeMatch("text/plain",response.ContentType))
                {
                    response.Res = httpResponse.downloadHandler.text;
                }
                
                return response;
            }
            if((response.StatusCode == 404))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {
                    response.ApiError = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = new JsonConverter[] { new FlexibleObjectDeserializer(), new DateOnlyConverter(), new EnumSerializer() }});
                }
                
                return response;
            }
            return response;
        }
        

        public async Task<GetLogsForProcessResponse> GetLogsForProcessAsync(GetLogsForProcessRequest? request = null)
        {
            request.AppId ??= Config.AppId;
            string baseUrl = this.Config.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/logs/v1/{appId}/process/{processId}", request);
            

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);
            
            
            var client = _securityClient;
            
            var httpResponse = await client.SendAsync(httpRequest);
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    var errorMsg = httpResponse.error;
                    httpRequest.Dispose();
                    throw new Exception(errorMsg);
            }

            var contentType = httpResponse.GetResponseHeader("Content-Type");
            
            var response = new GetLogsForProcessResponse
            {
                StatusCode = (int)httpResponse.responseCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            
            if((response.StatusCode == 200))
            {
                if(Utilities.IsContentTypeMatch("text/plain",response.ContentType))
                {
                    response.Res = httpResponse.downloadHandler.text;
                }
                
                return response;
            }
            if((response.StatusCode == 404) || (response.StatusCode == 500))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {
                    response.ApiError = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = new JsonConverter[] { new FlexibleObjectDeserializer(), new DateOnlyConverter(), new EnumSerializer() }});
                }
                
                return response;
            }
            return response;
        }
        
    }
}