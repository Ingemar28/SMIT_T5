//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMIT14_T5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class IOT_Setting
    {
        public int IotID { get; set; }
        public int DataID { get; set; }
        public int AlarmTypeID { get; set; }
        public string IotName { get; set; }
        public bool IotEnable { get; set; }
        public string Town { get; set; }
        public int AlarmCondition { get; set; }

        public virtual AlarmType AlarmType { get; set; }
        public virtual UserData UserData { get; set; }
    }
}
