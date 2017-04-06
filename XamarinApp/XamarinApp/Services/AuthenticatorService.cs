using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XamarinApp.Models;

namespace XamarinApp.Services
{
    /// <summary>
    /// Authenticate the user to the API.
    /// </summary>
    public class AuthenticatorService
    {
        /// <summary>
        /// Signup the user to the API.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AuthResponse> SignupAsync(CreateUserBindingModel model)
        {
            var json = JsonConvert.SerializeObject(model);

            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = null;
            AuthResponse result = null;
            try
            {
                using (var client = new HttpClient())
                {
                    response = await client.PostAsync(
                        "http://localhost:51502/api/accounts/create",
                        httpContent);
                }
            }
            catch (Exception e)
            {
                result = new AuthResponse
                {
                    IsSuccess = false,
                    Result = "Something went wrong..." + e.Message,
                };
            }

            var content = response?.Content?.ReadAsStringAsync();
            if (content != null)
            {
                var r = await content;

                if (!response.IsSuccessStatusCode)
                {
                    // something went wrong...
                    result = new AuthResponse
                    {
                        IsSuccess = false,
                        Result = "Something went wrong..." + r,
                    };
                }
                else
                {
                    result = new AuthResponse
                    {
                        IsSuccess = true,
                        Result = "Signup done successfully. " +
                            "Please confirm your registration " +
                            "by clicking the link in the provided email. " +
                            "Then you can login.",
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Signin the user to the API.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthResponse> SigninAsync(string username, string password)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "http://localhost:51502/oauth/token");

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            };

            request.Content = new FormUrlEncodedContent(keyValues);

            var client = new HttpClient();
            HttpResponseMessage response = null;
            AuthResponse result = null;

            try
            {
                response = await client.SendAsync(request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            var content = response?.Content?.ReadAsStringAsync().Result;
            Debug.WriteLine(content);
            //var jwt = await response?.Content?.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (content != null)
                {
                    JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);

                    var accessToken = jwtDynamic.Value<string>("access_token");

                    result = new AuthResponse
                    {
                        IsSuccess = true,
                        Result = accessToken
                    };
                }
            }
            else
            {
                result = new AuthResponse
                {
                    IsSuccess = false,
                    Result = content
                };
            }

            return result;
        }
    }
}
