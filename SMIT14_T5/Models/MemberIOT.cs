using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SMIT14_T5.Models
{
    public partial class MemberIOT
    {
        public int IotID { get; set; }
        public int DataID { get; set; }
        public int AlarmTypeID { get; set; }
        public string IotName { get; set; }
        public bool IotEnable { get; set; }
        public string Town { get; set; }
        public int AlarmCondition { get; set; }
        public bool AlarmEnable { get; set; }

        public virtual AlarmType AlarmType { get; set; }
        public virtual UserData UserData { get; set; }
    }
}
