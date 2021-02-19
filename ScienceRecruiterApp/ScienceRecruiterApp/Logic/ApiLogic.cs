using Newtonsoft.Json;
using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScienceRecruiterApp.Logic
{
    public class ApiLogic
    {

        public async Task<List<T>> GetResults<T>(string APIUrl)
        {
            List<T> list = new List<T>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);
                var response = client.GetAsync(APIUrl);
                string json = await response.Result.Content.ReadAsStringAsync();

                list =  JsonConvert.DeserializeObject<List<T>>(json);
            }


            return list;

        }


        

        public async Task<List<T>> GetResults<T>(string id, string APIUrl)
        {
            List<T> list = new List<T>();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);
                UriBuilder uriBuilder = new UriBuilder(APIUrl);
                uriBuilder.Query = String.Concat("UserSpecKey=", id.ToString()); ;
                var response = client.GetAsync(APIUrl);
                string json = await response.Result.Content.ReadAsStringAsync();

                list = JsonConvert.DeserializeObject<List<T>>(json);
                
            }


            return list;

        }

        public async Task<UserSpec> GetUserId(string email)
        {
            UserSpec list = new UserSpec();
            
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);

                UriBuilder uriBuilder = new UriBuilder(Helpers.Constants.MailRetrieveUrl);
                uriBuilder.Query = String.Concat("email=", email);

                var response = await client.GetAsync(uriBuilder.Uri);
                string jsonresp = await response.Content.ReadAsStringAsync();

                list = JsonConvert.DeserializeObject<List<UserSpec>>(jsonresp)[0];
            }


            return list;

        }

        public async Task<bool> PostUser(UserSpec usertemp)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);
                    
                    string json = JsonConvert.SerializeObject(usertemp, Formatting.Indented) ;
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Helpers.Constants.UserPostUrl, content);
                    string result = response.Content.ReadAsStringAsync().Result;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async void DeleteResults(string id, string APIUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);

                UriBuilder uriBuilder = new UriBuilder(APIUrl);
                uriBuilder.Query = String.Concat("metaid=", id);
                var response = await client.DeleteAsync(uriBuilder.Uri);
                
                string jsonresp = await response.Content.ReadAsStringAsync();
            }
        }
        public bool PostResults<T>(T usertemp, string APIUrl)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);
                    
                    string json = JsonConvert.SerializeObject(usertemp, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(APIUrl, content);
                    string result = response.Result.Content.ReadAsStringAsync().Result;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        
    }

    internal sealed class FormatNumbersAsTextConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;
        public override bool CanConvert(Type type) => type == typeof(int);

        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            int number = (int)value;
            writer.WriteValue(number.ToString(CultureInfo.InvariantCulture));
            
        }

        public override object ReadJson(
            JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }

    internal sealed class PlainJsonStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue((string)value);
        }
    }

}
