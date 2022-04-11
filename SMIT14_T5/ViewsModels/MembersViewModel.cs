using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMIT14_T5.ViewsModels;

namespace SMIT14_T5.ViewsModels
{
    public class MembersViewModel
    {
        public IEnumerable<SMIT14_T5.Models.UserData> userDatas { get; set; }
        public IEnumerable<SMIT14_T5.Models.UserSetting> userSettings { get; set; }
        public IEnumerable<SMIT14_T5.Models.AlarmType> alarmTypes { get; set; }
        public IEnumerable<SMIT14_T5.Models.Area> areas { get; set; }
        public IEnumerable<SMIT14_T5.Models.IOT_Setting> iot_Settings { get; set; }
    }
}
