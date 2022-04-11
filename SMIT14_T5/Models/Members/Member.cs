using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMIT14_T5.Models.Members
{
    public class SettingData
    {
        public int DataID { get; set; }
        public System.Guid U_ID { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string UserLastName { get; set; }
        public string UserFirstName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public int SettingID { get; set; }
        public int AlarmTypeID { get; set; }
        public bool AlarmEnable { get; set; }
        public int AlarmCondition { get; set; }
        public int AlarmAreaID { get; set; }
        public bool AlarmAreaEnable { get; set; }
        public virtual AlarmType AlarmType { get; set; }
        public virtual Area Area { get; set; }
        public virtual UserData UserData { get; set; }

        public string AlarmName { get; set; }
        public int SensorID { get; set; }

        public int AreaID { get; set; }
        public string AreaName { get; set; }

    }
} 