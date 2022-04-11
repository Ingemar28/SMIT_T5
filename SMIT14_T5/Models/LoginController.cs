using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using SMIT14_T5.ViewsModels;
using SMIT14_T5.Models;

namespace SMIT14_T5.Models
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        static public string sqlstr;
        static public SqlConnection cn;
        static public SqlDataAdapter da;
        public void SQLConn()
        {
            switch (Environment.MachineName)
            {
                case "DESKTOP-T1OG40G":
                    sqlstr = "Data Source=DESKTOP-T1OG40G\\SQLEXPRESS;Initial Catalog=TEST;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                case "DESKTOP-1507IVQ":
                    sqlstr = "Data Source=DESKTOP-1507IVQ\\SQLEXPRESS;Initial Catalog=TEST;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                case "踏水無痕-PC":
                    //踏水無痕-PC\SQLEXPRESS
                    sqlstr = "Data Source=踏水無痕-PC\\SQLEXPRESS;Initial Catalog=TEST;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                case "DESKTOP-I0C8M1U":
                    sqlstr = "Data Source=DESKTOP-I0C8M1U\\SQLEXPRESS;Initial Catalog=TEST;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                default:
                    break;
            }
        }


        public bool check(Register data)
        {
            SQLConn();
            using (SqlConnection sqlConnectionCheck = new SqlConnection(sqlstr))
            {
                SqlCommand sqlCommandCheck = new SqlCommand("select * from dbo.UserAccount where UserAccount = @UserAccount and UserPWD = @UserPWD;");
                sqlCommandCheck.Parameters.AddWithValue("@UserAccount", data.UserAccount1);
                sqlCommandCheck.Parameters.AddWithValue("@UserPWD", data.UserPWD);
                using (sqlCommandCheck.Connection = sqlConnectionCheck)
                {
                    sqlConnectionCheck.Open();
                    using (SqlDataReader reader = sqlCommandCheck.ExecuteReader())
                    {
                        return (!reader.HasRows);
                    }
                }
            }
            //and 改 or ??
        }

        public int NewUser(Register data)
        {
            //判斷是否已經註冊過
            if (check(data))
            {
                SQLConn();
                //新增帳號密碼
                using (SqlConnection sqlConnectionAccount = new SqlConnection(sqlstr))
                {
                    using (SqlCommand sqlCommandAccount = new SqlCommand())
                    {
                        sqlCommandAccount.CommandText = "insert into dbo.UserAccount(UserAccount, UserPWD) values(@UserAccount, @UserPWD)";
                        sqlCommandAccount.Parameters.AddWithValue("@UserAccount", data.UserAccount1);
                        sqlCommandAccount.Parameters.AddWithValue("@UserPWD", data.UserPWD);
                        using (sqlCommandAccount.Connection = sqlConnectionAccount)
                        {
                            sqlConnectionAccount.Open();
                            int num = sqlCommandAccount.ExecuteNonQuery();
                            sqlConnectionAccount.Close();
                        }
                    }
                }
                //取得系統生成的U_ID
                SQLConn();
                SqlConnection sqlConnectionGetID = new SqlConnection(sqlstr);
                SqlCommand sqlCommandGetID = new SqlCommand("select * from dbo.UserAccount where UserAccount = @UserAccount and UserPWD = @UserPWD;");
                sqlCommandGetID.Parameters.AddWithValue("@UserAccount", data.UserAccount1);
                sqlCommandGetID.Parameters.AddWithValue("@UserPWD", data.UserPWD);
                sqlCommandGetID.Connection = sqlConnectionGetID;
                sqlConnectionGetID.Open();
                SqlDataReader reader = sqlCommandGetID.ExecuteReader();
                Guid U_ID = new Guid();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        U_ID = reader.GetGuid(reader.GetOrdinal("U_ID"));
                    }
                }
                sqlConnectionGetID.Close();

                //將註冊資料寫入資料庫
                SQLConn();
                using (SqlConnection sqlConnectionData = new SqlConnection(sqlstr))
                {
                    using (SqlCommand sqlCommandData = new SqlCommand())
                    {
                        sqlCommandData.CommandText = @"insert into UserData(U_ID, BirthDate, UserFirstName, UserLastName, EmailAddress, AddressLine1, AddressLine2) 
                        values(@U_ID, @BirthDate, @UserFirstName, @UserLastName, @EmailAddress, @AddressLine1, @AddressLine2)";
                        sqlCommandData.Parameters.AddWithValue("@U_ID", U_ID);
                        sqlCommandData.Parameters.AddWithValue("@BirthDate", data.BirthDate);
                        sqlCommandData.Parameters.AddWithValue("@UserFirstName", data.UserFirstName);
                        sqlCommandData.Parameters.AddWithValue("@UserLastName", data.UserLastName);
                        sqlCommandData.Parameters.AddWithValue("@EmailAddress", data.EmailAddress);
                        if (String.IsNullOrEmpty(data.AddressLine1))
                        {
                            data.AddressLine1 = "";
                        }
                        if (String.IsNullOrEmpty(data.AddressLine2))
                        {
                            data.AddressLine2 = "";
                        }
                        sqlCommandData.Parameters.AddWithValue("@AddressLine1", data.AddressLine1);
                        sqlCommandData.Parameters.AddWithValue("@AddressLine2", data.AddressLine2);
                        using (sqlCommandData.Connection = sqlConnectionData)
                        {
                            sqlConnectionData.Open();
                            int num2 = sqlCommandData.ExecuteNonQuery();
                            sqlConnectionData.Close();
                            //return num2;
                        }
                    }
                }

                //將Iot預設設定寫入資料庫
                int DataID = GetDataID(U_ID);
                SQLConn();
                using (SqlConnection sqlConnectionSetting = new SqlConnection(sqlstr))
                {
                    using (SqlCommand sqlCommandSetting = new SqlCommand())
                    {
                        string[] AlarmCondition = { "'30'", "'20'", "'70'", "'20'" };
                        string[] AlarmAreaID = { "'3001'", "'3002'", "'3003'", "'3004'" };
                        string Comm1 = "";
                        for (int p = 1; p < 5; p++)
                        {
                            Comm1 += "insert into UserSetting(AlarmTypeID, AlarmEnable, AlarmCondition, AlarmAreaID, AlarmAreaEnable, DataID) values(" +
                            p + ",'1'," + AlarmCondition[p - 1] + "," + AlarmAreaID[p - 1] + ",'1'," + DataID + ")";
                        }
                        sqlCommandSetting.CommandText = Comm1;
                        using (sqlCommandSetting.Connection = sqlConnectionSetting)
                        {
                            sqlConnectionSetting.Open();
                            int num4 = sqlCommandSetting.ExecuteNonQuery();
                            sqlConnectionSetting.Close();
                        }
                    }
                }

                SQLConn();
                using (SqlConnection sqlConnectionIot = new SqlConnection(sqlstr))
                {
                    using (SqlCommand sqlCommandIot = new SqlCommand())
                    {
                        string[] IotNames = { "'wr'", "'macpi'", "'djpi'", "'flesh'" };
                        string[] Town = { "'南屯區(站點一_惠文國小)'", "'西  區(站點一_中教大)'", "'西屯區(站點一_文心櫻花站)'", "'北屯區(站點一_仁美國小)'" };
                        string Comm2 = "";
                        for (int i = 0; i < IotNames.Length; i++)
                        {
                            for (int p = 1; p < 5; p++)
                            {
                                Comm2 += "insert into IOT_Setting(DataID, AlarmTypeID, IotName, IotEnable, Town) values(" + DataID +
                                    "," + p + "," + IotNames[i] + ",'1'," + Town[i] + ")";
                            }
                        }
                        sqlCommandIot.CommandText = Comm2;
                        using (sqlCommandIot.Connection = sqlConnectionIot)
                        {
                            sqlConnectionIot.Open();
                            int num3 = sqlCommandIot.ExecuteNonQuery();
                            sqlConnectionIot.Close();
                            return num3;
                        }
                    }
                }
            }
            return 0;
        }

        public Guid Verify(string GetAccount, string GetPwd)
        {
            //List<UserAccount> users= new List<UserAccount>();
            SQLConn();
            SqlConnection sqlConnectionU_ID = new SqlConnection(sqlstr);
            SqlCommand sqlCommandU_ID = new SqlCommand("select * from dbo.UserAccount where UserAccount = @UserAccount and UserPWD = @UserPWD;");
            sqlCommandU_ID.Parameters.AddWithValue("@UserAccount", GetAccount);
            sqlCommandU_ID.Parameters.AddWithValue("@UserPWD", GetPwd);
            Guid U_ID = Guid.NewGuid();
            using (sqlCommandU_ID.Connection = sqlConnectionU_ID)
            {
                sqlConnectionU_ID.Open();
                using (SqlDataReader reader = sqlCommandU_ID.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            return U_ID = reader.GetGuid(reader.GetOrdinal("U_ID"));
                    }
                    else
                    {
                        Console.WriteLine("資料庫為空！");
                        return U_ID = Guid.Empty;
                    }
                }
                sqlConnectionU_ID.Close();
                return U_ID = Guid.Empty;
            }
        }

        public int GetDataID(Guid U_ID)
        {
            SQLConn();
            int DataID = 0;
            SqlConnection sqlConnectionDataID = new SqlConnection(sqlstr);
            SqlCommand sqlCommandDataID = new SqlCommand("select * from UserData where U_ID = @U_ID;");
            sqlCommandDataID.Parameters.AddWithValue("@U_ID", U_ID);
            using (sqlCommandDataID.Connection = sqlConnectionDataID)
            {
                sqlConnectionDataID.Open();
                using (SqlDataReader reader = sqlCommandDataID.ExecuteReader())
                {
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                                return DataID = reader.GetInt32(reader.GetOrdinal("DataID"));
                        }
                        else
                        {
                            Console.WriteLine("資料庫為空！");
                            return DataID = 0;
                        }
                    }
                    sqlConnectionDataID.Close();
                    return DataID = 0;
                }
            }
        }


        public int Connect(string Comm)
        {
            SQLConn();
            using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = Comm;
                    using (sqlCommand.Connection = sqlConnection)
                    {
                        sqlConnection.Open();
                        int num = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        return num;
                    }
                }
            }
        }

        //public int Connect(SqlCommand Comm)
        //{
        //    using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
        //    {
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.CommandText = Comm;
        //            using (sqlCommand.Connection = sqlConnection)
        //            {
        //                sqlConnection.Open();
        //                int num = sqlCommand.ExecuteNonQuery();
        //                sqlConnection.Close();
        //                return num;
        //            }
        //        }
        //    }
        //}



        //Update UserData
        public int UpdateUserData(string DataID, UserData userData)
        {
            SQLConn();
            using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"update UserData set UserLastName = @UserLastName ,
                    UserFirstName = @UserFirstName,BirthDate = @BirthDate, EmailAddress = @EmailAddress , AddressLine1 = @AddressLine1, 
                    AddressLine2= @AddressLine2 where DataID = @DataID ;";
                    sqlCommand.Parameters.AddWithValue("@UserLastName", userData.UserLastName);
                    sqlCommand.Parameters.AddWithValue("@UserFirstName", userData.UserFirstName);
                    sqlCommand.Parameters.AddWithValue("@BirthDate", userData.BirthDate);
                    sqlCommand.Parameters.AddWithValue("@EmailAddress", userData.EmailAddress);
                    if (String.IsNullOrEmpty(userData.AddressLine1))
                    {
                        userData.AddressLine1 = "";
                    }
                    if (String.IsNullOrEmpty(userData.AddressLine2))
                    {
                        userData.AddressLine2 = "";
                    }
                    sqlCommand.Parameters.AddWithValue("@AddressLine1", userData.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@AddressLine2", userData.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@DataID", DataID);
                    using (sqlCommand.Connection = sqlConnection)
                    {
                        sqlConnection.Open();
                        int num = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        return num;
                    }
                }
            }

        }

        //Update UserAccount
        public int UpdatePWD(string U_ID, string OldPWD, string NewPWD)
        {
            SQLConn();
            // 1. get old pwd => verify 
            if (OldPWD != null && NewPWD != null)
            {
                string UserPWD = "";
                using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
                {
                    SqlCommand sqlCommand = new SqlCommand("select * from dbo.UserAccount where U_ID = @U_ID ");
                    sqlCommand.Parameters.AddWithValue("@U_ID", U_ID);
                    using (sqlCommand.Connection = sqlConnection)
                    {
                        sqlConnection.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                    UserPWD = reader.GetString(reader.GetOrdinal("UserPWD"));
                            }
                        }
                    }
                }
                if (UserPWD == OldPWD)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand())
                        {
                            sqlCommand.CommandText = @"update dbo.UserAccount set UserPWD = @NewPWD
                        where U_ID = @U_ID;";
                            sqlCommand.Parameters.AddWithValue("@NewPWD", NewPWD);
                            sqlCommand.Parameters.AddWithValue("@U_ID", U_ID);
                            using (sqlCommand.Connection = sqlConnection)
                            {
                                sqlConnection.Open();
                                int num = sqlCommand.ExecuteNonQuery();
                                sqlConnection.Close();
                                return num;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        //Update UserSetting
        public int UpdateUserSetting(string DataID, int AlarmTypeID, bool AlarmEnable, int AlarmCondition)
        {
            SQLConn();
            using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"update UserSetting set AlarmTypeID = @AlarmTypeID, AlarmEnable = @AlarmEnable, AlarmCondition = @AlarmCondition
                    where DataID = @DataID and AlarmTypeID = @AlarmTypeID;";
                    sqlCommand.Parameters.AddWithValue("@AlarmTypeID", AlarmTypeID);
                    sqlCommand.Parameters.AddWithValue("@AlarmEnable", AlarmEnable);
                    sqlCommand.Parameters.AddWithValue("@AlarmCondition", AlarmCondition);
                    sqlCommand.Parameters.AddWithValue("@DataID", DataID);
                    using (sqlCommand.Connection = sqlConnection)
                    {
                        sqlConnection.Open();
                        int num = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        return num;
                    }
                }
            }


        }

        //Update IOT_Setting
        public int UpdateIot_Setting(string DataID, int AlarmTypeID, bool IotEnable, int k)
        {
            SQLConn();
            // for(4) commed += update iot setting
            string[] IotName = { "wr", "macpi", "djpi", "flesh" };
            using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"update IOT_Setting set IotEnable = @IotEnable
                    where DataID = @DataID and AlarmTypeID = @AlarmTypeID and IotName = @IotName";
                    sqlCommand.Parameters.AddWithValue("@IotEnable", IotEnable);
                    sqlCommand.Parameters.AddWithValue("@DataID", DataID);
                    sqlCommand.Parameters.AddWithValue("@AlarmTypeID", AlarmTypeID);
                    sqlCommand.Parameters.AddWithValue("@IotName", IotName[k - 1]);
                    using (sqlCommand.Connection = sqlConnection)
                    {
                        sqlConnection.Open();
                        int num = sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                        return num;
                    }
                }
            }


        }

        public List<MemberIOT> GetIotSetting(string DataID)
        {
            SQLConn();
            using (SqlConnection sqlConnection = new SqlConnection(sqlstr))
            {
                SqlCommand sqlCommand = new SqlCommand(@"select us.DataID,us.AlarmTypeID,AlarmEnable, AlarmCondition,IotName, IotEnable, Town from Usersetting as us
                inner join IOT_Setting as iot on us.DataID = iot.DataID and us.AlarmTypeID = iot.AlarmTypeID 
                where us.DataID = @DataID
                order by AlarmTypeID");
                sqlCommand.Parameters.AddWithValue("@DataID", DataID);
                using (sqlCommand.Connection = sqlConnection)
                {
                    sqlConnection.Open();
                    var iotsetting = new List<MemberIOT>();
                    using (SqlDataReader dd = sqlCommand.ExecuteReader())
                    {
                        while (dd.Read())
                        {
                            var iotelement = new MemberIOT();
                            iotelement.AlarmTypeID = Int32.Parse(dd["AlarmTypeID"].ToString());
                            iotelement.AlarmCondition = Int32.Parse(dd["AlarmCondition"].ToString());
                            iotelement.AlarmEnable = bool.Parse(dd["AlarmEnable"].ToString());
                            iotelement.IotName = dd["IotName"].ToString();
                            iotelement.IotEnable = bool.Parse(dd["IotEnable"].ToString());
                            iotelement.Town = dd["Town"].ToString();
                            iotsetting.Add(iotelement);
                        }
                    }
                    sqlConnection.Close();
                    return iotsetting;
                }
            }

        }
    }


}

