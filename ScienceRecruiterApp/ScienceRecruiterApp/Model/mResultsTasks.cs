using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace ScienceRecruiterApp.Model
{
    public class mResultsTasks_SST 
    {
        public ImageSource icon { get; set; }

        public string TaskName { get; set; }

        public string nSamples { get; set; }

        public List<string> Means { get; set; }

        public List<string> SDs { get; set; }

        public List<string> Mins { get; set; }

        public List<string> Maxs { get; set; }

        public List<string> ParameterNames { get; set; }

        public mResultsTasks_SST(string im, List<ResultsSST> type)
        {
            
            
            
            TaskName = "Stop Signal Task";
            icon =  ImageSource.FromResource(im);

            nSamples = String .Concat("Number of samples = ", type.Count().ToString() );

            
            
        }
        
       

    }
}
