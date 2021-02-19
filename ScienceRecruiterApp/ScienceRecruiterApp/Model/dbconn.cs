using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model
{
    class dbconn
    {

        public static string connectionString = "server=recruiter.database.windows.net;port=1433;user=andrasp;password=0915854947Andre;database=Participants_data;";

        public string selectAllQuery { get; set; }

        

        public dbconn(string tableName)
        {
            selectAllQuery = String.Concat("select * from ", tableName);

        }
    }
}
