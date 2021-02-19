using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace ScienceRecruiterApp.Model
{
    public class FilterClass
    {
        List<ResultsSST> list;
        public List<int> idx { get; set; }
        public List<FilterProperties> Properties { get; set; }

        public FilterClass()
        {

            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            list = apiLogic.GetResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl).Result;
            idx = list.Select(x => x.id).ToList();
        }

        private async void GetRes()
        {
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            list = apiLogic.GetResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl).Result;
        }

        public FilterClass(List<FilterProperties> filterProperties)
        {
            idx = new List<int>();
            
            List<FilterProperties> tmpprop = filterProperties;
            GetRes();
            while (tmpprop.Count > 0)
            {
                PropertyInfo prop = typeof(ResultsSST).GetProperty(tmpprop.Last().propertyname);

                Type t = tmpprop.Last().propertyvalue.GetType();
                if (t.Equals(typeof(int)))
                {
                    List<int> parameter = list.Select(l => ((int)prop.GetValue(l, null))).ToList();
                    switch (tmpprop.Last().condition)
                    {
                        case "bigger":
                            idx.AddRange(list.Select(a => a.id).Where((d, idxa) => parameter[idxa] > (int)tmpprop.Last().propertyvalue));
                            break;
                        case "smaller":
                            idx.AddRange(list.Select(a => a.id).Where((d, idxa) => parameter[idxa] < (int)tmpprop.Last().propertyvalue));
                            break;
                        default: break;
                        
                    }
                    
                }
                else
                {
                    
                    List<string> parameter = list.Select(l => ((string)prop.GetValue(l, null))).ToList();
                    idx.AddRange(list.Select(a => a.id).Where((d, idxa) => parameter[idxa] == (string)tmpprop.Last().propertyvalue));
                }
                
                    tmpprop.RemoveAt(tmpprop.Count-1);
                
                
            }


        }
    }
}
