using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScoreboardClient.Client
{
    public class WebApiClient
    {
        private string _apiBaseAddress;

        public WebApiClient(string apiBaseUrl)
        {
            this._apiBaseAddress = apiBaseUrl;
        }

        public T Post<T>(string resource, string body, ref string errorMessage)
        {
            var client = new RestClient(this._apiBaseAddress);

            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(body);

            var content = default(T);

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                content = JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                errorMessage = string.IsNullOrEmpty(response.ErrorMessage) ? response.Content : response.ErrorMessage;
            }

            return content;
        }

        public T Get<T>(string resource, Parameter[] parameters, ref string errorMessage)
        {
            var client = new RestClient(this._apiBaseAddress);

            var request = new RestRequest(resource, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            foreach(var param in parameters)
            {
                request.AddOrUpdateParameter(param);
            }

            var content = default(T);

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                content = JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                errorMessage = string.IsNullOrEmpty(response.ErrorMessage) ? response.Content : response.ErrorMessage;
            }

            return content;
        }
    }
}
