using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SMIT14_T5.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using SMIT14_T5.ViewsModels;
using System.Data.SqlClient;


namespace SMIT14_T5.Controllers
{

    public class HomeController : Controller
    {
        int complementcount = 0, complementcountIOT = 0;
        bool state = false;

        static public string sqlstr;
        static public SqlConnection cn;
        static public SqlDataAdapter da;


        public class ChartJSON
        {
            public string time { get; set; }
            public string temperature { get; set; }
            public string humidity { get; set; }
            public string timeiot { get; set; }
            public string temperatureiot { get; set; }
            public string humidityiot { get; set; }

        }

        // GET: Home
        static public JArray getJson(string uri)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri); //request請求
            req.Timeout = 10000; //request逾時時間
            req.Method = "GET"; //request方式
            HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
            StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
            string result = streamReader.ReadToEnd(); //讀取到最後一行
            respone.Close();
            streamReader.Close();
            JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //將資料轉為json物件
            return (JArray)jsondata["records"]["location"]; //回傳json陣列

        }

        public ActionResult Index()
        {
            JArray jsondata = getJson("https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-F507CE22-D1EB-4740-A1D1-DEF8363B0EB9&locationName=%E8%87%BA%E4%B8%AD%E5%B8%82");

            foreach (JObject data in jsondata)
            {
                string loactionname = (string)data["locationName"]; //地名
                string weathdescrible = (string)data["weatherElement"][0]["time"][0]["parameter"]["parameterName"]; //天氣狀況
                string pop = (string)data["weatherElement"][1]["time"][0]["parameter"]["parameterName"];  //降雨機率
                string mintemperature = (string)data["weatherElement"][2]["time"][0]["parameter"]["parameterName"]; //最低溫度
                string maxtemperature = (string)data["weatherElement"][4]["time"][0]["parameter"]["parameterName"]; //最高溫度
                Session["weather"] = (loactionname + " 天氣:" + weathdescrible + " 溫度:" + mintemperature + "°c-" + maxtemperature + "°c 降雨機率:" + pop + "%");
            }
            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();

            if (Session["Logout"] != null && Session["Logout"] is false)
            {
                Session["Logout"] = true;
                Session["DataID"] = null;
            }
            return View(viewModel);

        }
        public ActionResult FormUpdate()
        {
            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.RawData = db1.RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();
            viewModel.Station = db1.Station.ToList();
            return PartialView("FormUpdate", viewModel);
        }

        private TEST_weatherEntities db1 = new TEST_weatherEntities();

        public ActionResult Data()
        {
            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.RawData = db1.RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();
            viewModel.Station = db1.Station.ToList();
            string time = "[";
            string temperature = "[";
            string humidity = "[";

            for (int i = db1.RawData.ToList().Count - 20; i < db1.RawData.ToList().Count; i++)
            {
                time += "'" + db1.RawData.ToList()[i].DataTime.ToString() + "',";
                temperature += "'" + db1.RawData.ToList()[i].Temperature.ToString() + "',";
                humidity += "'" + db1.RawData.ToList()[i].Humidity.ToString() + "',";
            }

            ViewBag.time = time + "]";
            ViewBag.temperature = temperature + "]";
            ViewBag.humidity = humidity + "]";

            ViewBag.Message = "Your application description page.";

            string timeiot = "[";
            string temperatureiot = "[";
            string humidityiot = "[";
            for (int i = db1.IOT_RawData.ToList().Count - 20; i < db1.IOT_RawData.ToList().Count; i++)
            {
                timeiot += "'" + db1.IOT_RawData.ToList()[i].DataTime.ToString() + "',";
                temperatureiot += "'" + db1.IOT_RawData.ToList()[i].Temperature.ToString() + "',";
                humidityiot += "'" + db1.IOT_RawData.ToList()[i].Humidity.ToString() + "',";
            }
            ViewBag.timeiot = timeiot + "]";
            ViewBag.temperatureiot = temperatureiot + "]";
            ViewBag.humidityiot = humidityiot + "]";

            ViewBag.Message = "Your application description page.";
            //return View(db1.RawData.ToList());
            return View(viewModel);
        }


        private TESTEntities1 db = new TESTEntities1();
        LoginController logincontroller = new LoginController();
        public Guid U_ID = new Guid();
        public int DataID = 0;

        public ActionResult Member2()
        {
            if (Session["DataID"] == null)
            {
                Session["Alert"] = 2;
                return View("Login");
            }
            else
            {
                ViewsModels.MembersViewModel viewModel1 = new ViewsModels.MembersViewModel();
                viewModel1.userDatas = db.UserDatas.ToList();
                viewModel1.userSettings = db.UserSettings.ToList();
                viewModel1.alarmTypes = db.AlarmTypes.ToList();
                viewModel1.areas = db.Areas.ToList();
                viewModel1.iot_Settings = db.IOT_Setting.ToList();

                return View(viewModel1);
                //logincontroller.GetSettingData(Session["DataID"].ToString());
                //UserData userData = db.UserDatas.Find(Session["DataID"]);
                //return View(userData);
            }
        }


        [HttpPost]
        public ActionResult Member2(UserData userData, FormCollection post)
        {
            //db.UserDatas.Add(userData);
            LoginController logincontroller = new LoginController();
            string BirthDate = post["BirthDate"];
            int temp1 = logincontroller.UpdateUserData(Session["DataID"].ToString(), userData);

            string OldPWD = post["OldPWD"];
            string NewPWD = post["NewPWD"];

            Session["PWDhint"] = logincontroller.UpdatePWD(Session["U_ID"].ToString(), OldPWD, NewPWD);

            bool Enable = true;
            int Condition = 0;
            for (int i = 1; i < 5; i++)
            {
                if (post["AlarmEnable" + i] != "false")
                {
                    post["AlarmEnable" + i] = "true";
                }
                Enable = bool.Parse(post["AlarmEnable" + i]);
                Condition = Int32.Parse(post["AlarmCondition" + i]);
                logincontroller.UpdateUserSetting(Session["DataID"].ToString(), i, Enable, Condition);
                for (int k = 1; k < 5; k++)
                {
                    if (post["IotEnable_" + i + "_" + k] != "false")
                    {
                        post["IotEnable_" + i + "_" + k] = "true";
                    }
                    Enable = bool.Parse(post["IotEnable_" + i + "_" + k]);
                    logincontroller.UpdateIot_Setting(Session["DataID"].ToString(), i, Enable, k);
                }
            }
            return RedirectToAction("Member2");
        }


        public ActionResult AlertCheck()
        {
            Session["Flag"] = false;
            Session["Temp"] = false;
            Session["Hum"] = false;

            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();

            if (Session["DataID"] == null)
            {
                Session["Flag"] = true;
                return PartialView("AlertCheck", viewModel);
            }

            List<MemberIOT> iotsetting = logincontroller.GetIotSetting(Session["DataID"].ToString());
            MemberIOT[,] iotarray = new MemberIOT[4, 4];
            string[] namearray = { "wr", "djpi", "macpi", "flesh" };

            for (int p = 0; p < 4; p++)
            {
                for (int i = 0; i < 4; i++)
                {
                    iotarray[p, i] = iotsetting.Find(x => (x.IotName == namearray[p]) && (x.AlarmTypeID == i + 1));
                }
            }
            IOT_RawData wrdata = db1.IOT_RawData.ToList().FindLast(x => x.IOT_name == "wr");
            IOT_RawData djpidata = db1.IOT_RawData.ToList().FindLast(x => x.IOT_name == "djpi");
            IOT_RawData macpidata = db1.IOT_RawData.ToList().FindLast(x => x.IOT_name == "macpi");
            IOT_RawData fleshdata = db1.IOT_RawData.ToList().FindLast(x => x.IOT_name == "flesh");
            IOT_RawData[] Rawdataarray = { wrdata, djpidata, macpidata, fleshdata };

            List<IOT_RawData> TempAlert = new List<IOT_RawData>();
            List<IOT_RawData> HumAlert = new List<IOT_RawData>();


            for (int k = 0; k < 4; k++)
            {
                if ((iotarray[k, 0].AlarmEnable == true && float.Parse(Rawdataarray[k].Temperature) >= iotarray[k, 0].AlarmCondition) || (iotarray[k, 1].AlarmEnable == true && float.Parse(Rawdataarray[k].Temperature) <= iotarray[k, 1].AlarmCondition))
                {
                    Session["Temp"] = true;
                    var element = new IOT_RawData();
                    element.IOT_name = Rawdataarray[k].IOT_name;
                    element.Temperature = Rawdataarray[k].Temperature;
                    TempAlert.Add(element);
                    ViewBag.TempAlert = TempAlert;
                }
                if ((iotarray[k, 2].AlarmEnable == true && float.Parse(Rawdataarray[k].Humidity) >= iotarray[k, 2].AlarmCondition) || (iotarray[k, 3].AlarmEnable == true && float.Parse(Rawdataarray[k].Humidity) <= iotarray[k, 3].AlarmCondition))
                {
                    Session["Hum"] = true;
                    var element2 = new IOT_RawData();
                    element2.IOT_name = Rawdataarray[k].IOT_name;
                    element2.Temperature = Rawdataarray[k].Humidity;
                    HumAlert.Add(element2);
                    ViewBag.HumAlert = HumAlert;
                }
            }
            ViewBag.TempLength = TempAlert.Count();
            ViewBag.HumLength = HumAlert.Count();
            return PartialView("AlertCheck", viewModel);
        }


        //return View("Index", viewModel);




        [HttpPost]
        public ActionResult Logout(int Logout)
        {
            if (Logout == 0)
            {
                Session["DataID"] = null;
            }
            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();
            return View("Index", viewModel);
        }


        public ActionResult Login()
        {
            Session["Alert"] = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection post)
        {
            string GetAccount = post["UserAccount1"];
            string GetPwd = post["UserPWD"];
            Guid U_ID = logincontroller.Verify(GetAccount, GetPwd);
            Session["U_ID"] = U_ID;
            DataID = logincontroller.GetDataID(U_ID);

            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();


            if (DataID == 0)
            {
                Session["Alert"] = 1;
                return View("Login");
            }
            else
            {
                Session["DataID"] = DataID;
                return View("Index", viewModel);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register data)
        {
            LoginController logincontroller = new LoginController();
            Session["flag"] = logincontroller.NewUser(data);
            if (Session["flag"].ToString() == "0")
            {
                return View("Register");
            }
            else
            {
                Session["Alert"] = 0;
                return View("Login");
            }
        }


        [HttpPost]
        public ActionResult RawDataUpdate(string count, string rawstation, string iotstation)
        {
            ViewBag.count = Int32.Parse(count);
            ViewBag.iotstation = iotstation;
            ViewBag.rawstation = rawstation;



            ViewsModels.HomecontrollerViewModel viewModel = new ViewsModels.HomecontrollerViewModel();
            viewModel.IOT_RawData = db1.IOT_RawData.ToList();
            viewModel.RawData = db1.RawData.ToList();
            viewModel.IOTStation = db1.IOTStation.ToList();
            viewModel.Station = db1.Station.ToList();

            foreach (var item in db1.Station)
            {
                if (item.StationID == rawstation)
                {
                    if (item.St_ID < 29)
                    {
                        ViewBag.countfix = ViewBag.count * 6;
                    }
                    else
                    {
                        ViewBag.countfix = ViewBag.count;
                    }
                }
            }

            for (int i = db1.RawData.ToList().Count; i > 0; i--)
            {
                if (db1.RawData.ToList()[i - 1].StationID == ViewBag.rawstation)
                {
                    ViewBag.checkbase = db1.RawData.ToList()[i - 1].DataTime;
                    break;
                }
            }

            for (int i = db1.IOT_RawData.ToList().Count; i > 0; i--)
            {

                if (db1.IOT_RawData.ToList()[i - 1].IOT_name == ViewBag.iotstation)
                {
                    if (complementcount == ViewBag.count)
                    {
                        ViewBag.complementcount = complementcount;
                        state = true;
                        break;
                    }
                    ViewBag.checkbaseIOT = db1.IOT_RawData.ToList()[i - 1].DataTime;
                    if (db1.IOT_RawData.ToList()[i - 1].DataTime == ViewBag.checkbase)
                    {
                        ViewBag.complementcount = complementcount;
                        state = true;
                        break;
                    }
                    else if (db1.IOT_RawData.ToList()[i - 1].DataTime < ViewBag.checkbase) { break; }

                    complementcount++;
                }
                if (state) { break; }
            }

            state = false;

            if (complementcount == 0)
            {
                for (int i = db1.RawData.ToList().Count; i > 0; i--)
                {

                    if (db1.RawData.ToList()[i - 1].StationID == ViewBag.rawstation)
                    {
                        if (complementcountIOT == ViewBag.count)
                        {
                            ViewBag.complementcountIOT = complementcountIOT;
                            state = true;
                            break;
                        }
                        if (db1.RawData.ToList()[i - 1].DataTime == ViewBag.checkbaseIOT)
                        {
                            ViewBag.complementcountIOT = complementcountIOT;
                            state = true;
                            break;
                        }
                        else if (db1.RawData.ToList()[i - 1].DataTime < ViewBag.checkbaseIOT) { break; }
                        complementcountIOT++;
                    }
                    if (state) { break; }
                }
            }
            return PartialView("RawDataUpdate", viewModel);
        }

        //[HttpPost]
        public ActionResult SortData(string count, string rawstation, string iotstation)
        {
            SQLConn();
            ViewBag.count = Int32.Parse(count);
            da = new SqlDataAdapter();
            bool HRCheck = false;

            foreach (var item in db1.Station)
            {
                if (item.StationID == rawstation)
                {
                    if (item.St_ID < 29)
                    { HRCheck = true; }
                    break;
                }
            }
            string sql;
            if (HRCheck)
            {
                sql = "select TOP (@P1) * from IOT_RawData  where DataTime like '%:00%'and IOT_name = @P2 order by DataTime DESC ";
            }
            else
            {
                sql = "select TOP (@P1) * from IOT_RawData  where IOT_name = @P2 order by DataTime DESC";
            }



            da.UpdateCommand = new SqlCommand(sql, cn);
            da.UpdateCommand.Parameters.AddWithValue("@P1", ViewBag.count);
            da.UpdateCommand.Parameters.AddWithValue("@P2", iotstation);


            cn.Open();


            var iotdata = new List<IOT_RawData>();
            SqlDataReader tt = da.UpdateCommand.ExecuteReader();
            while (tt.Read())
            {
                var element = new IOT_RawData();
                element.DataTime = DateTime.Parse(tt["DataTime"].ToString());
                element.Humidity = tt["Humidity"].ToString();
                element.IOT_name = tt["IOT_name"].ToString();
                element.Temperature = tt["Temperature"].ToString();

                iotdata.Add(element);
            }
            tt.Close();
            sql = "select TOP (@P1) * from RawData  where StationID = @P2 order by DataTime DESC ";

            da.UpdateCommand = new SqlCommand(sql, cn);
            da.UpdateCommand.Parameters.AddWithValue("@P1", ViewBag.count);
            da.UpdateCommand.Parameters.AddWithValue("@P2", rawstation);

            var dataraw = new List<RawData>();
            tt = da.UpdateCommand.ExecuteReader();
            while (tt.Read())
            {
                var element = new RawData();
                element.DataTime = DateTime.Parse(tt["DataTime"].ToString());
                element.Data_ID = Int32.Parse(tt["Data_ID"].ToString());
                element.DateTempHigh = tt["DateTempHigh"].ToString();
                element.DateTempLow = tt["DateTempLow"].ToString();
                element.H24R = tt["H24R"].ToString();
                element.Humidity = tt["Humidity"].ToString();
                element.Pressure = tt["Pressure"].ToString();
                element.StationID = tt["StationID"].ToString();
                element.Temperature = tt["Temperature"].ToString();

                dataraw.Add(element);
            }


            cn.Close();


            foreach (var item in dataraw)
            {
                if (item.StationID == rawstation)
                {
                    ViewBag.checkbase = item.DataTime;
                    break;
                }
            }

            foreach (var item in iotdata)
            {

                if (item.IOT_name == iotstation)
                {
                    if (complementcount == ViewBag.count)
                    {
                        ViewBag.complementcount = complementcount;
                        state = true;
                        break;
                    }
                    ViewBag.checkbaseIOT = item.DataTime;
                    if (item.DataTime == ViewBag.checkbase)
                    {
                        ViewBag.complementcount = complementcount;
                        state = true;
                        break;
                    }
                    else if (item.DataTime < ViewBag.checkbase) { break; }
                    complementcount++;
                }
                if (state) { break; }
            }

            state = false;

            ViewBag.complementcountIOT = 0;

            if (complementcount == 0)
            {
                foreach (var item in dataraw)
                {

                    if (item.StationID == rawstation)
                    {
                        if (complementcountIOT == ViewBag.count)
                        {
                            ViewBag.complementcountIOT = complementcountIOT;
                            state = true;
                            break;
                        }
                        if (item.DataTime == ViewBag.checkbaseIOT)
                        {
                            ViewBag.complementcountIOT = complementcountIOT;
                            state = true;
                            break;
                        }
                        else if (item.DataTime < ViewBag.checkbaseIOT) { break; }
                        complementcountIOT++;
                    }
                    if (state) { break; }
                }
            }

            state = false;


            int j = 0;
            string time = "";
            string temperature = "";
            string humidity = "";
            string temperatureiot = "";
            string timeiot = "";
            string humidityiot = "";
            //for (int i = 0 ; i < db1.RawData.ToList().Count ; i++)
            foreach (var item in dataraw)
            {
                if (item.StationID == rawstation)
                {
                    if (j == ViewBag.count) { state = true; break; }
                    else
                    {
                        for (; j < ViewBag.complementcount; j++)
                        {
                            //temperature = "NULL" + "," + temperature;
                            //time = "'" + "NULL" + "'," + time;
                            //humidity = "NULL" + "," + humidity;
                            if (j == ViewBag.count) { state = true; break; }
                        }
                        if (j == ViewBag.count) { state = true; break; }
                        if (j < ViewBag.complementcountIOT) { j++; continue; }
                        j++;
                        temperature = item.Temperature.ToString() + "," + temperature;
                        time = "'" + item.DataTime.ToString() + "'," + time;
                        humidity = float.Parse(item.Humidity.ToString()) * 100 + "," + humidity;
                    }
                }
                if (state) { break; }
            }
            state = false;
            j = 0;
            foreach (var item in iotdata)
            {
                if (item.IOT_name == iotstation)
                {
                    if (j == ViewBag.count) { state = true; break; }
                    else
                    {
                        for (; j < ViewBag.complementcountIOT; j++)
                        {
                            //temperatureiot = "NULL" + "," + temperatureiot;
                            //timeiot = "'" + "NULL" + "'," + timeiot;
                            //humidityiot = "NULL" + "," + humidityiot;
                            if (j == ViewBag.count) { state = true; break; }
                        }
                        if (j == ViewBag.count) { state = true; break; }
                        if (j < ViewBag.complementcount) { j++; continue; }
                        j++;
                        temperatureiot = item.Temperature.ToString() + "," + temperatureiot;
                        timeiot = "'" + item.DataTime.ToString() + "'," + timeiot;
                        humidityiot = item.Humidity.ToString() + "," + humidityiot;
                    }
                }
                if (state) { break; }
            }



            ChartJSON data = new ChartJSON()
            {
                time = time.Substring(0, time.Length - 1),
                temperature = temperature.Substring(0, temperature.Length - 1),
                humidity = humidity.Substring(0, humidity.Length - 1),
                timeiot = timeiot.Substring(0, timeiot.Length - 1),
                temperatureiot = temperatureiot.Substring(0, temperatureiot.Length - 1),
                humidityiot = humidityiot.Substring(0, humidityiot.Length - 1),
            };



            return Json(JsonConvert.SerializeObject(data));
        }

        public void SQLConn()
        {

            switch (Environment.MachineName)
            {
                case "DESKTOP-T1OG40G":
                    sqlstr = "Data Source=DESKTOP-T1OG40G\\SQLEXPRESS;Initial Catalog=TEST_weather;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                case "DESKTOP-1507IVQ":
                    sqlstr = "Data Source=DESKTOP-1507IVQ\\SQLEXPRESS;Initial Catalog=TEST_weather;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                case "踏水無痕-PC":
                    //踏水無痕-PC\SQLEXPRESS
                    sqlstr = "Data Source=踏水無痕-PC\\SQLEXPRESS;Initial Catalog=TEST_weather;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                case "DESKTOP-I0C8M1U":
                    sqlstr = "Data Source=DESKTOP-I0C8M1U\\SQLEXPRESS;Initial Catalog=TEST_weather;Integrated Security=True";
                    cn = new SqlConnection(sqlstr);
                    cn.Open();
                    cn.Close();
                    break;
                default:
                    break;
            }
        }


    }
}