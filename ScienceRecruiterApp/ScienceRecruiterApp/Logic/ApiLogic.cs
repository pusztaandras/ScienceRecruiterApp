using MySqlX.XDevAPI;
using Newtonsoft.Json;
using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ScienceRecruiterApp.Model;

namespace ScienceRecruiterApp.Logic
{
    public class ApiLogic
    {

        public async Task<List<T>> GetResults<T>(string APIUrl)
        {
            List<T> list = new List<T>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);
                    var response = client.GetAsync(APIUrl);
                    string json = await response.Result.Content.ReadAsStringAsync();

                    list = JsonConvert.DeserializeObject<List<T>>(json);
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return list;
        }




        public async Task<List<T>> GetResults<T>(string id, string APIUrl) where T:ResultsTasks
        {
            List<T> list = new List<T>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);
                    //UriBuilder uriBuilder = new UriBuilder(String.Concat(Helpers.Constants.UserUrl, "/", id));
                    var response = client.GetAsync(APIUrl);
                    string json = await response.Result.Content.ReadAsStringAsync();

                    list = JsonConvert.DeserializeObject<List<T>>(json);
                    list = list.Where(e => e.UserSpecKey == id).ToList();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


            return list;

        }

        public async Task<UserSpec> GetUserId(string email)
        {
            UserSpec list = new UserSpec();
            if (!validatemail(email)) // Get USER by Used ID
            {

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");
                        UriBuilder uriBuilder = new UriBuilder(String.Concat(Helpers.Constants.UserUrl, "/", email));
                        //uriBuilder.Host = Helpers.Constants.HostUrl;
                        var response = await client.GetAsync(String.Concat(Helpers.Constants.UserUrl, "/", email));
                        string jsonresp = await response.Content.ReadAsStringAsync();

                        list = JsonConvert.DeserializeObject<UserSpec>(jsonresp);
                    }
                }
                catch (Exception ex)
                {
                    string message=ex.Message;
                    Debug.WriteLine(message);
                }
            }

            else // Get User by email (for login)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");
                        UriBuilder uriBuilder = new UriBuilder(Helpers.Constants.UserUrl);
                        var response = await client.GetAsync(uriBuilder.Uri);
                        string jsonresp = await response.Content.ReadAsStringAsync();

                        UserSpec users = JsonConvert.DeserializeObject<UserSpec>(jsonresp);

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return list;
        }

        public async Task<bool> PostUser(UserSpec usertemp)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string json = JsonConvert.SerializeObject(usertemp, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Helpers.Constants.UserUrl, content);
                    string result = response.Content.ReadAsStringAsync().Result;
                }
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }

        public async void DeleteResults(string id, string APIUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);

                    //UriBuilder uriBuilder = new UriBuilder(APIUrl);
                    //uriBuilder.Query = String.Concat("metaid=", id);
                    var response = await client.DeleteAsync(String.Concat(APIUrl,"/", id));

                    string jsonresp = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public async Task<bool> PostResults<T>(T usertemp, string APIUrl)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);

                    string json = JsonConvert.SerializeObject(usertemp, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(APIUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Error : " +response.StatusCode);
                    }
                    else
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        Debug.WriteLine("Response :" + result);
                    }

                    
                }
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<bool> PutResults<T>(T usertemp, string APIUrl, int id)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Helpers.Constants.ApiKey);

                    string json = JsonConvert.SerializeObject(usertemp, Formatting.Indented);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(String.Concat(APIUrl, "/",id.ToString()), content);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Error : " + response.StatusCode);
                    }
                    else
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        Debug.WriteLine("Response :" + result);
                    }


                }
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }

        private bool validatemail(string text)
        {
            try
            {
                MailAddress m = new MailAddress(text);

                return true;
            }
            catch (FormatException)
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
