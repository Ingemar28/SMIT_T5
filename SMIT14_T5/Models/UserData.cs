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
    
    public partial class UserData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserData()
        {
            this.UserSettings = new HashSet<UserSetting>();
            this.IOT_Setting = new HashSet<IOT_Setting>();
        }
    
        public int DataID { get; set; }
        public System.Guid U_ID { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string UserLastName { get; set; }
        public string UserFirstName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    
        public virtual UserAccount UserAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IOT_Setting> IOT_Setting { get; set; }
    }
}