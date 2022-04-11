using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMIT14_T5.ViewsModels;

namespace SMIT14_T5.ViewsModels
{
    public class HomecontrollerViewModel
    {
        public IEnumerable<SMIT14_T5.Models.RawData> RawData { get; set; }
        public IEnumerable<SMIT14_T5.Models.IOT_RawData> IOT_RawData { get; set; }
        public IEnumerable<SMIT14_T5.Models.IOTStation> IOTStation { get; set; }
        public IEnumerable<SMIT14_T5.Models.Station> Station { get; set; }
    }
}