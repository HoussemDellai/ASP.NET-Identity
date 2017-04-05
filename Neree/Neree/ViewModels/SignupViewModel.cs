using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Neree.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Neree.ViewModels
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private string _result;

        public CreateUserBindingModel Model { get; set; }

        public ICommand SignupCommand => new Command(async () => await Signup());

        private async Task Signup()
        {

            var json = JsonConvert.SerializeObject(Model);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response;

            var client = new HttpClient();
            //using (var client = new HttpClient())
            //{
            response = await client.PostAsync(
                "http://localhost:51502/api/accounts/create",
                httpContent);
            //}

            if (!response.IsSuccessStatusCode)
            {
                // something went wrong...
            }
            else
            {
                var r = await response.Content.ReadAsStringAsync();
            }
        }


        public ICommand SigninCommand => new Command(async () => await Signin());

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value; 
                OnPropertyChanged();
            }
        }

        private async Task Signin()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:51502")
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", Model.Username),
                new KeyValuePair<string, string>("password", Model.Password),
                new KeyValuePair<string, string>("grant_type", "password")
            };

            request.Content = new FormUrlEncodedContent(keyValues);

            var response = await client.SendAsync(request);

            // {"access_token":"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiI1Nzg3NzhmMC00NGEzLTRhYWUtOGI4Yi01OTRlODhlNTdmYjQiLCJ1bmlxdWVfbmFtZSI6IlRlc3RVc2VyMSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vYWNjZXNzY29udHJvbHNlcnZpY2UvMjAxMC8wNy9jbGFpbXMvaWRlbnRpdHlwcm92aWRlciI6IkFTUC5ORVQgSWRlbnRpdHkiLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6Ijg4OWIwZmI2LTYyMzktNDRhNi1iZjZjLTM3ODE3M2Y0MmNiMyIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE1MDIiLCJhdWQiOiIwOTkxNTNjMjYyNTE0OWJjOGVjYjNlODVlMDNmMDAyMiIsImV4cCI6MTQ5MTQ5NDU4NSwibmJmIjoxNDkxNDA4MTg1fQ.zZCsghMOxJq1Jn3S0F9NNI7I0nX7TdIIvOPXFH0pfQQ","token_type":"bearer","expires_in":86399}
            Result = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(Result);

            JObject r = JsonConvert.DeserializeObject<dynamic>(Result);

            var accessToken = r.Value<string>("access_token");//.Value<JArray>("photo");

            Debug.WriteLine(accessToken);
        }

        public SignupViewModel()
        {
            Model = new CreateUserBindingModel
            {
                //Username = "TestUser2",
                //Password = "@Aa123456",
                //ConfirmPassword = "@Aa123456",
                //Email = "h2@d.c",
                //FirstName = "Test2",
                //LastName = "User2",
                Username = "TestUser1",
                Password = "@Aa123456",
                ConfirmPassword = "@Aa123456",
                Email = "h1@d.c",
                FirstName = "Test1",
                LastName = "User1",
                //RoleName = ""
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
