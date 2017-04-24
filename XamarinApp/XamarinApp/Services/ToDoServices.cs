using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XamarinApp.Models;
using XamarinApp.Models.Entities;

namespace XamarinApp.Services
{
    public class ToDoServices
    {
        private HttpClient CreateAuthenticatedClient(string accessToken)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            return client;
        }

        public async Task<List<Todo>> GeTodosAsync(string accessToken)
        {
            var client = CreateAuthenticatedClient(accessToken);

            var json = string.Empty;
            AuthResponse result = null;

            try
            {
                json = await client.GetStringAsync(
                    Constants.BaseServiceUrl + "api/todoes/forcurrentuser")
                    .ConfigureAwait(false);

                Debug.WriteLine(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var todos = JsonConvert.DeserializeObject<List<Todo>>(json);

            return todos;
        }

        public async Task<bool> PostTodoAsync(Todo todo, string accessToken)
        {
            var client = CreateAuthenticatedClient(accessToken);

            var json = await Task.Run(() => 
                JsonConvert.SerializeObject(todo)).ConfigureAwait(false);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response;

            try
            {
                response = await client.PostAsync(
                    Constants.BaseServiceUrl + "api/todoes/create",
                    httpContent);

                Debug.WriteLine("response : " + response);
            }
            catch (Exception e)
            {
                Debug.WriteLine("e : " + e);

                return false;
            }

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
