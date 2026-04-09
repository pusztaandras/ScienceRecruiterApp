using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Newtonsoft.Json;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace ScienceRecruiterApp.Model
{
    public class UserSpec
    {
        [PrimaryKey]
        public string id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string pass { get; set; }

        public int age { get; set; }

        public string gender { get; set; }

        public string hand { get; set; }

        public string isDrug{ get; set; }

        public string Drug { get; set; }

        public string isDisorder { get; set; }

        public string Disorder { get; set; }

        public static bool CheckforLocal()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation))
            {
                conn.CreateTable<Model.UserSpec>();
                List<Model.UserSpec> list = conn.Table<Model.UserSpec>().ToList();
                return list.Count() > 0;
                

            }
        }
        public UserSpec()
        {
            Random x = new Random();
            id = String.Concat("SR", x.Next(100, 999).ToString(), "Pi", x.Next(1000, 9999).ToString()); 
        }
        //public static async Task<bool> RetriveID()
        //{
        //    UserSpec b;

        //    using (SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation))
        //    {
        //        conn.CreateTable<Model.UserSpec>();
        //        List<Model.UserSpec> a = conn.Table<Model.UserSpec>().ToList();
        //        b = a.FirstOrDefault();

        //    }
        //    List<UserSpec> list=await App.client.GetTable<UserSpec>().Where(a => a.email == b.email && a.pass == b.pass).ToListAsync();
        //    if (list.Count != 0)
        //    {
        //        App.user = list.FirstOrDefault();
        //        return true;
        //    }
        //    else
        //    {
        //        await App.client.GetTable<UserSpec>().InsertAsync(b);
        //        list = await App.client.GetTable<UserSpec>().Where(a => a.email == b.email && a.pass == b.pass).ToListAsync();
        //        App.user = list.FirstOrDefault();
        //        return true;
        //    }
                
            
        //}
        

        
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
