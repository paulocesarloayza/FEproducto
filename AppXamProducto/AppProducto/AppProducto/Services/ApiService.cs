namespace AppProducto.Services
{
    using AppProducto.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Plugin.Connectivity;

    public class ApiService
    {
        public async Task<TokenResponse> GetToken(
            string urlBase,
            string username,
            string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);                
                StringContent head = new StringContent(
                    string.Format(
                        "grant_type=password&username={0}&password={1}",
                        username,
                        password),
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.PostAsync(
                        "token",
                        head);
                var result = await response.Content.ReadAsStringAsync();
                var tokresult = JsonConvert.DeserializeObject<TokenResponse>(
                        result);
                return tokresult;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<T>> Get<T>(
            string urlBase, 
            string servicePrefix, 
            string controller,
            string tokenType,
            string accessToken
            )
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    tokenType, accessToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return list;
            }
            catch 
            {
                return null;
            }
        }
    }
}
