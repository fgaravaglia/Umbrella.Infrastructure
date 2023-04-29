using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Umbrella.Infrastructure.Api
{
    public static class ContentTypes
    {
        public const string ApplicationJson = "application/json";

        public const string Text = "text/plain";

        public const string FormData = "multipart/form-data";
    }

    public class ApiCaller
    {
        readonly ILogger _Logger;

        public ApiCaller(ILogger logger)
        {
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiCallResponseDTO<T>> GetAsync<T>(ApiCallRequestDTO request) where T : class
        {
            var apiResponse = new ApiCallResponseDTO<T>();
            this._Logger.LogInformation("Start GetAsync - {BaseEndpoint}/{Method}", request.Url, request.MethodName);
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (String.IsNullOrEmpty(request.Url))
                    throw new ArgumentNullException(nameof(request), "Service Endpoint cannot be null");
                if (String.IsNullOrEmpty(request.MethodName))
                    throw new ArgumentNullException(nameof(request), "Service Method Name cannot be null");

                using (HttpClient client = new HttpClient())
                {
                    var queryString = request?.RequestObject?.ToQueryString() ?? "";
                    var url = RequestHelpers.GetRequestUrl(request?.Url ?? "", request?.MethodName ?? "", queryString);

                    // Set up Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    if (request != null && !string.IsNullOrEmpty(request.Token))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentTypes.ApplicationJson));
                    if (request.RequestHeader != null)
                    {
                        foreach (var parameter in request.RequestHeader)
                            client.DefaultRequestHeaders.Add(parameter.Key, parameter.Value);
                    }

                    // invoking api and parse the response
                    using (HttpResponseMessage httpResponse = (await client.GetAsync(url)))
                    {
                        using HttpContent content = httpResponse.Content;
                        var json = await content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(json))
                            json = "";
                        apiResponse.StatusCode = (int)httpResponse.StatusCode;

                        if (httpResponse.IsSuccessStatusCode && !String.IsNullOrEmpty(json))
                            apiResponse.Body = JsonSerializer.Deserialize<T>(json);
                        //Errors are returned in many different formats.  
                        //We just need to log the message so don't need to bind it to an object. 
                        else
                            apiResponse.Error = json;
                    }
                }   
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex, "Unexpected error invoking api");
                apiResponse.StatusCode = 500;
                apiResponse.Error = "{ \"message\": \"" + ex.Message + "\"}";
            }
            finally 
            {
                this._Logger.LogInformation("End GetAsync - {BaseEndpoint}/{Method}", request.Url, request.MethodName);
            }
            return apiResponse;
        }

        // public static async Task<APICallResponseDTO<T>> PostAsync(APICallRequestDTO request)
        // {
        //     try
        //     {
        //         using HttpClient client = new HttpClient();
        //         var url = RequestHelpers.GetRequestUrl(request.Url, request.MethodName);
        //         var postContent = RequestHelpers.CreateRequestBodyContent(request.RequestObject, request.ContentType);

        //         client.DefaultRequestHeaders.Accept.Clear();

        //         if (!string.IsNullOrEmpty(request.Token))
        //             client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);

        //         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ApplicationJson.GetDescription()));

        //         if (request.RequestHeader != null)
        //             foreach (var parameter in request.RequestHeader)
        //             {
        //                 client.DefaultRequestHeaders.TryAddWithoutValidation(parameter.Key, parameter.Value);
        //             }

        //         var requestJson = JsonConvert.SerializeObject(request.RequestObject);

        //         using (HttpResponseMessage httpResponse = (await client.PostAsync(url, postContent)))
        //         {
        //             using HttpContent content = httpResponse.Content;
        //             var json = await content.ReadAsStringAsync();

        //             var apiResponse = new APICallResponseDTO<T>();
        //             apiResponse.StatusCode = (int)httpResponse.StatusCode;

        //             if (httpResponse.IsSuccessStatusCode)
        //                 apiResponse.Body = JsonConvert.DeserializeObject<T>(json);
        //             else
        //                 apiResponse.Error = json;

        //             return apiResponse;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new CustomException(ex);
        //     }
        // }

        // public static async Task<APICallResponseDTO<T>> PutAsync(APICallRequestDTO request)
        // {
        //     try
        //     {
        //         using HttpClient client = new HttpClient();
        //         var url = RequestHelpers.GetRequestUrl(request.Url, request.MethodName);
        //         var postContent = RequestHelpers.CreateRequestBodyContent(request.RequestObject, request.ContentType);
        //         var contentJson = JsonConvert.SerializeObject(request);
        //         client.DefaultRequestHeaders.Accept.Clear();

        //         if (!string.IsNullOrEmpty(request.Token))
        //             client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);

        //         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ApplicationJson.GetDescription()));

        //         if (request.RequestHeader != null)
        //             foreach (var parameter in request.RequestHeader)
        //             {
        //                 client.DefaultRequestHeaders.Add(parameter.Key, parameter.Value);
        //             }

        //         using (HttpResponseMessage httpResponse = (await client.PutAsync(url, postContent)))
        //         {
        //             using HttpContent content = httpResponse.Content;
        //             var json = await content.ReadAsStringAsync();

        //             var apiResponse = new APICallResponseDTO<T>();
        //             apiResponse.StatusCode = (int)httpResponse.StatusCode;

        //             if (httpResponse.IsSuccessStatusCode)
        //                 apiResponse.Body = JsonConvert.DeserializeObject<T>(json);
        //             else
        //                 apiResponse.Error = json;

        //             return apiResponse;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new CustomException(ex);
        //     }
        // }

        // public static async Task<APICallResponseDTO<T>> DeleteAsync(APICallRequestDTO request)
        // {
        //     try
        //     {
        //         using (HttpClient client = new HttpClient())
        //         {
        //             var queryString = request.RequestObject.ToQueryString();
        //             var url = RequestHelpers.GetRequestUrl(request.Url, request.MethodName, queryString);

        //             client.DefaultRequestHeaders.Accept.Clear();

        //             if (!string.IsNullOrEmpty(request.Token))
        //                 client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);

        //             client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ApplicationJson.GetDescription()));

        //             if (request.RequestHeader != null)
        //                 foreach (var parameter in request.RequestHeader)
        //                 {
        //                     client.DefaultRequestHeaders.Add(parameter.Key, parameter.Value);
        //                 }

        //             using (HttpResponseMessage httpResponse = (await client.DeleteAsync(url)))
        //             {
        //                 using HttpContent content = httpResponse.Content;
        //                 var json = await content.ReadAsStringAsync();

        //                 var apiResponse = new APICallResponseDTO<T>();
        //                 apiResponse.StatusCode = (int)httpResponse.StatusCode;

        //                 if (httpResponse.IsSuccessStatusCode)
        //                     apiResponse.Body = JsonConvert.DeserializeObject<T>(json);
        //                 //Errors are returned in many different formats.  
        //                 //We just need to log the message so don't need to bind it to an object. 
        //                 else
        //                     apiResponse.Error = json;

        //                 return apiResponse;
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new CustomException(ex);
        //     }
        // }
    }
}