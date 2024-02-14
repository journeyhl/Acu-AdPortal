using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;

using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace DotNetCoreSqlDb.Controllers
{
    public class CreateAdController : Controller
    {
        SqlConnection con = new SqlConnection("Server=tcp:jhl-dbcentral.database.windows.net,1433;Initial Catalog=db_uccxWarehouse;Persist Security Info=False;User ID=sqlboss;Password=0%8SAB9b1QCQ3R2g;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //SqlConnection con = new SqlConnection("Data Source=msdocs-core-sql-jake-server.database.windows.net,1433;Initial Catalog=msdocs-core-sql-jake-database;User ID=msdocs-core-sql-jake-server-admin;Password=5IGSJ22LN4704MQ0$");
        SqlConnection conVK = new SqlConnection("Server=192.168.3.144,1433; Database=PHONE_TRACE; User ID=dbu_svcBOOMI; Password=t5RG#Q39!3!@2e;Encrypt=False; Packet Size = 4096; Trusted_Connection = False"); //  TrustServerCertificate =False
        int SourceID;
        public static CreateAd Create = new CreateAd();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult BindPhone()
        {
            return View();
        }


        [HttpPost]
        public ActionResult BindPhoneSubmit(string PhoneAdCode, string TFN, DateTime Date)
        {
            SqlConnection con = new SqlConnection("Server=tcp:jhl-dbcentral.database.windows.net,1433;Initial Catalog=db_uccxWarehouse;Persist Security Info=False;User ID=sqlboss;Password=0%8SAB9b1QCQ3R2g;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            using (con)
            {
                using (SqlCommand cmd = new SqlCommand("PostPhone"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@AdCode", PhoneAdCode);
                    cmd.Parameters.AddWithValue("@OnSale", Date.ToShortDateString());
                    cmd.Parameters.AddWithValue("@Phone", TFN);
                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        con.Close();
                        ViewBag.ProducthoneBind = "Success!";
                        return View("BindPhone");
                    }
                    catch
                    {
                        con.Close();
                        ViewBag.ProducthoneBind = "Error!";
                        return View("BindPhone");
                    }
                }
            }
        }

        static void InfoMessageHandler(object sender, SqlInfoMessageEventArgs e)
        {
            string myMsg = e.Message;
        }

        [HttpPost]
        public ActionResult Internal(string AdCodeInternal)
        {
            
            Create.AdCodeInternal = null;
            Create.AdCodeExternal = null;
            using (con)
            {
                using (SqlCommand AdCodeSearch = new SqlCommand("generateInternalAdCode"))
                {
                    AdCodeSearch.CommandType = CommandType.StoredProcedure;
                    AdCodeSearch.Connection = con;
                    con.Open();
                    SqlDataReader readersql = AdCodeSearch.ExecuteReader(); // create a reader
                    if (readersql.Read())
                    {
                        ViewBag.AdCodeInternal = readersql["adCode"].ToString();
                        AdCodeInternal = ViewBag.AdCodeInternal;
                        Create.AdCodeInternal = AdCodeInternal;
                        return View("Index");
                    }
                    else //Put a label next to AdCode to put Error Message
                    {
                        ViewBag.AdCodeError = "This AdCode already exists.";
                        return View("Index");
                    }
                }
            }
        }
   

        [HttpPost]
        public ActionResult External(string AdCodeExternal)
        {
            Create.AdCodeInternal = null;
            Create.AdCodeExternal = null;
            using (con)
            {
                using (SqlCommand AdCodeSearch = new SqlCommand("generateExternalAdCode"))
                {
                    AdCodeSearch.CommandType = CommandType.StoredProcedure;
                    AdCodeSearch.Connection = con;
                    con.Open();
                    SqlDataReader readersql = AdCodeSearch.ExecuteReader(); // create a reader
                    if (readersql.Read())
                    {
                        ViewBag.AdCodeExternal = readersql["adCode"].ToString();
                        AdCodeExternal = ViewBag.AdCodeExternal;
                        Create.AdCodeExternal = AdCodeExternal;
                        return View("Index");
                    }
                    else //Put a label next to AdCode to put Error Message
                    {
                        ViewBag.AdCodeError = "This AdCode already exists.";
                        return View("Index");
                    }
                }

            }
        }
      

       [HttpPost]
        public ActionResult Submit(string AdCodeInternal, string AdCodeExternal, string AdCode, DateTime Date, DateTime SpendDate,    
                                   string myInput, string AdVersionID, string CheckPhone, string mySecondary,
                                   string AdVersionProduct, string? Region, string? Circulation,  string AdCategory,
                                   string AdSpendAmount,  string? PageNumber, string CustomAdCode, string TFN
                                   )
        {
            




            
            string test = TFN;
            ViewBag.AdCodeError = "";
            ViewBag.SourceError = "";
            ViewBag.AdVersionIDError = "";
            ViewBag.InsertError = "";
            ViewBag.AdVersionProductError = "";
            ViewBag.AdSpendpendAmountError = "";
            AdCode = null;
            using (con)
            {
                if (Create.AdCodeExternal != null && (Create.CustomAdCode == "" || Create.CustomAdCode == null))
                {
                    AdCode = Create.AdCodeExternal;
                }
                if (Create.AdCodeInternal != null && (Create.CustomAdCode == "" || Create.CustomAdCode == null))
                {
                    AdCode = Create.AdCodeInternal;
                }
                if(CustomAdCode != null && CustomAdCode != "")
                {
                    AdCode = CustomAdCode;
                }
                if(AdCode == null)
                {
                    ViewBag.AdCodeError = "Please select whether this ad was created Internally, by an Ad Agency or enter a custom AdCode.";
                    return View("Index");
                }    
                using (SqlCommand AdCodeSearch = new SqlCommand("acSearch"))
                {
                    AdCodeSearch.CommandType = CommandType.StoredProcedure;
                    AdCodeSearch.Parameters.AddWithValue("@AdCode", AdCode);
                    AdCodeSearch.Connection = con;
                    con.Open();
                    SqlDataReader readersql = AdCodeSearch.ExecuteReader(); // create a reader
                    if (readersql.HasRows) // if the AdCode exists, it will continue
                    {  
                        con.Close();
                        ViewBag.AdCodeError = "This AdCode already exists.";
                        return View("Index");
                    }
                    else
                    {
                        con.Close();
                        //Code to pull pubcode for PrimaryAd
                            //using (SqlCommand SourceIDSearch = new SqlCommand("findSourceID"))
                            //{
                            //    SourceIDSearch.CommandType = CommandType.StoredProcedure;
                            //    SourceIDSearch.Connection = con;
                            //    if (myInput == null)
                            //    {
                            //        ViewBag.SourceError = "Please select a Primary Ad";
                            //        if (Create.AdCodeExternal != null)
                            //        {
                            //            ViewBag.AdCodeExternal = Create.AdCodeExternal;
                            //        }
                            //        if (Create.AdCodeInternal != null)
                            //        {
                            //            ViewBag.AdCodeInternal = Create.AdCodeInternal;
                            //        }
                            //        return View("Index");
                            //    }
                            //    ViewBag.SourceError = "";
                            //    SourceIDSearch.Parameters.AddWithValue("@PrimaryName", myInput);
                            //    con.Open();
                            //    SqlDataReader readersqlSourceID = SourceIDSearch.ExecuteReader(); // create a reader
                            //    if (readersqlSourceID.Read())
                            //    {
                            //        SourceID = Int32.Parse(readersqlSourceID["SourceID"].ToString());
                            //        con.Close();
                            //    }
                            //    else //Put a label next to PubCode to put Error Message
                            //    {
                            //        ViewBag.SourceError = "Invalid Primary Ad, please try again.";
                            //        if (Create.AdCodeExternal != null)
                            //        {
                            //            ViewBag.AdCodeExternal = Create.AdCodeExternal;
                            //        }
                            //        if (Create.AdCodeInternal != null)
                            //        {
                            //            ViewBag.AdCodeInternal = Create.AdCodeInternal;
                            //        }
                            //        con.Close();
                            //        return View("Index");
                                
                            //    }
                            //}
                        if (AdSpendAmount == null)
                        {
                            ViewBag.InsertError = "AdSpendAmount field must contain only numbers or decimal points and is a required field.";
                            if (Create.AdCodeExternal != null)
                            {
                                ViewBag.AdCodeExternal = Create.AdCodeExternal;
                            }
                            if (Create.AdCodeInternal != null)
                            {
                                ViewBag.AdCodeInternal = Create.AdCodeInternal;
                            }
                            return View("Index");
                        }
                        //if (AdVersionProduct == null)
                        //{
                        //    ViewBag.AdVersionProductError = "AdVersionProduct field is required.";
                        //    if (Create.AdCodeExternal != null)
                        //    {
                        //        ViewBag.AdCodeExternal = Create.AdCodeExternal;
                        //    }
                        //    if (Create.AdCodeInternal != null)
                        //    {
                        //        ViewBag.AdCodeInternal = Create.AdCodeInternal;
                        //    }
                        //    return View("Index");
                        //}
                        //else
                        //{
                        //    using (SqlCommand AdVersionProductCheck = new SqlCommand("AdVersionProductCheck"))
                        //    {
                        //        AdVersionProductCheck.CommandType = CommandType.StoredProcedure;
                        //        AdVersionProductCheck.Parameters.AddWithValue("@Product", AdVersionProduct);
                        //        AdVersionProductCheck.Connection = con;
                        //        con.Open();
                        //        SqlDataReader AdVersionProductReader = AdVersionProductCheck.ExecuteReader(); // create a reader
                        //        if (!AdVersionProductReader.HasRows) // if the AdVersionProduct does not match our definitions, it enters loop
                        //        {
                        //            con.Close();
                        //            AdVersionProduct = "Other";
                        //        }
                        //        else
                        //        {
                        //            con.Close();
                        //        }
                        //    }
                        //}
                        if (AdVersionID == null)
                        {
                            ViewBag.AdVersionIDError = "AdVersionID field is required.";
                            if (Create.AdCodeExternal != null)
                            {
                                ViewBag.AdCodeExternal = Create.AdCodeExternal;
                            }
                            if (Create.AdCodeInternal != null)
                            {
                                ViewBag.AdCodeInternal = Create.AdCodeInternal;
                            }
                            return View("Index");
                        }
                        else
                        {
                            using (SqlCommand AdVersionProductCheck = new SqlCommand("AdVersionProduct"))
                            {
                                AdVersionProductCheck.CommandType = CommandType.StoredProcedure;
                                AdVersionProductCheck.Parameters.AddWithValue("@AdVersionID", AdVersionID);
                                AdVersionProductCheck.Connection = con;
                                con.Open();
                                SqlDataReader AdVersionProductReader = AdVersionProductCheck.ExecuteReader(); // create a reader
                                if (AdVersionProductReader.HasRows) // if the AdVersionProduct does not match our definitions, it enters loop
                                {
                                    AdVersionProductReader.Read();
                                    AdVersionProduct = AdVersionProductReader.GetString(0);
                                    con.Close();
                                }
                                else
                                {
                                    con.Close();
                                    AdVersionProduct = "Other";
                                }
                            }
                        }
                        try
                        {
                            SqlCommand cmd = new SqlCommand("insertAdvertisement", con);
                           
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@AdCode", AdCode);
                            cmd.Parameters.AddWithValue("@AdVersionID", AdVersionID);
                            cmd.Parameters.AddWithValue("@PrimaryAdName", myInput);
                            cmd.Parameters.AddWithValue("@Category", AdCategory);
                            cmd.Parameters.AddWithValue("@StartDate", Date);
                            cmd.Parameters.AddWithValue("@SpendDate", SpendDate);
                            cmd.Parameters.AddWithValue("@SpendAmount", AdSpendAmount);
                            cmd.Parameters.AddWithValue("@Circulation", Circulation);
                            cmd.Parameters.AddWithValue("@SecondaryAdName", mySecondary);
                            if (mySecondary == null)
                            {
                                cmd.Parameters["@SecondaryAdName"].Value = SqlString.Null;
                            }
                            else
                            {
                                cmd.Parameters["@SecondaryAdName"].Value = mySecondary;
                            }
                            if (Circulation == null)
                            {
                                cmd.Parameters["@Circulation"].Value = SqlInt32.Null;
                            }
                            else
                            {
                                cmd.Parameters["@Circulation"].Value = Int32.Parse(Circulation);
                            }
                            if (AdSpendAmount == null)
                            {
                                ViewBag.AdSpendpendAmountError = "Ad Spend field is required. Please only use numbers or decimals.";
                                if (Create.AdCodeExternal != null)
                                {
                                    ViewBag.AdCodeExternal = Create.AdCodeExternal;
                                }
                                if (Create.AdCodeInternal != null)
                                {
                                    ViewBag.AdCodeInternal = Create.AdCodeInternal;
                                }
                                return View("Index");
                            }
                            else
                            {
                                cmd.Parameters["@SpendAmount"].Value = SqlMoney.Parse(AdSpendAmount);
                            }
                            if (CheckPhone == "1")
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    MediaTypeHeaderValue m = new MediaTypeHeaderValue("application/vnd.microsoft.card.adaptive");
                                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://journeyhl.webhook.office.com/webhookb2/af51916a-b4a6-49f3-a42b-07b1a3a1d8c6@474a556a-e9ae-49fd-9360-588b21aee670/IncomingWebhook/d39edada0ee74dff861e2ea2e1868800/dc440e51-2976-4d54-b6bd-1a80eb2507d6"))
                                    {
                                        string message = "{'text':'<at>jjordan@journeyhl.com</at> A new Advertisement has been created in the AdPortal that requires a TFN. AdCode: " + AdCode + "'}";
                   
                                        string message2 = "{\"type\":\"message\",\"attachments\":[{\"contentType\":\"application/vnd.microsoft.card.adaptive\",\"content\":{\"type\":\"AdaptiveCard\",\"body\":[{\"type\":\"TextBlock\",\"size\":\"Large\",\"weight\":\"Bolder\",\"text\":\"New Advertisement needs TFN\"},{\"type\":\"TextBlock\", \"wrap\": \"true\",\"text\":\"Hi <at>Jake UPN</at>, a new Ad created through the AdPortal has been created and needs a Phone Number bound.\\n\\nAdCode: " + AdCode + "\\n\\nDisplay: " + (AdVersionProduct.ToUpper().Replace(" ", "") + " " + AdCategory.ToUpper() + " " + AdCode) + "\\n\\nChannel: " + AdCategory.ToUpper() + "\\n\\nDrop Date: " + Date.ToShortDateString() + "\\n\\nCisco Product: " + AdVersionProduct.ToUpper() + "\"}],\"$schema\":\"http://adaptivecards.io/schemas/adaptive-card.json\",\"version\":\"1.0\",\"msteams\":{\"entities\":[{\"type\":\"mention\",\"text\":\"<at>Jake UPN</at>\",\"mentioned\":{\"id\":\"jjordan@journeyhl.com\",\"name\":\"Jake Jordan\"}}]}}}]}\r\n";

                                        // request.Content = new StringContent(message);
                                       request.Content = new StringContent(message2);

                                        //string message value
                                        //request.Content = new StringContent("{'text':'A new Advertisement has been created in the AdPortal that requires a TFN.'}");



                                        //request.Content = new StringContent("{'$schema':'https://adaptivecards.io/schemas/adaptive-card.json','type':'AdaptiveCard','version':'1.0','body':[{'type':'ColumnSet','columns':[{'width':'stretch','spacing':'padding','items':[{'type':'Container','height':'stretch','items':[{'type':'TextBlock','size':'large','text':'**New Ad Created: Bind Telephone Number**'},{'type':'TextBlock','spacing':'default','color':'attention','text':'AdCode: AdCode'}]},{'type':'ActionSet','actions':[{'type':'Action.Http','method':'POST','body':'{}','title':'Email Laura','url':'mailto:lpierce@journeyhl.com'}]}]}]}]}");
                                        request.Content.Headers.ContentType = m;

                                        var response = httpClient.Send(request);
                                    }
                                }
                            }






                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            ModelState.Clear();
                            if (Create.AdCodeExternal != null)
                            {
                                ViewBag.AdCode = "AdCode: " + Create.AdCodeExternal;
                            }
                            if (Create.AdCodeInternal != null)
                            {
                                ViewBag.AdCode = "AdCode: " + Create.AdCodeInternal;
                            }
                            Create.AdCodeInternal = null;
                            Create.AdCodeExternal = null;
                            ViewBag.InsertSuccess = "Advertisement submitted successfully. Thank you!";
                            ViewBag.SubSum = "Submission Summary";
                            ViewBag.AdCode = "AdCode: " + AdCode;
                            ViewBag.AdVersionID = "AdVersionID: " + AdVersionID;
                            ViewBag.Product = "Ad Product: " + AdVersionProduct;
                            ViewBag.PrimaryName = "Primary Name: " + myInput;
                            ViewBag.SecondaryName = "Secondary Name: " + mySecondary;
                            ViewBag.StartDate = "Ad Start Date: " + Date.ToString().Split(' ')[0];
                            ViewBag.SpendDate = "Ad Spend Date: " + SpendDate.ToString().Split(' ')[0];
                            ViewBag.AdSpend = "Ad Spend: $" + AdSpendAmount;
                            ViewBag.InCirculation = "In Circulation: " + Circulation;
                            ViewBag.AdCategory = "Ad Category:" + AdCategory;
                            return View("Index");
                        }
                        catch (Exception ex)
                        { //Put a label under Submit Button to put Error Message
                            
                            if (con.State == ConnectionState.Open)
                            {
                                con.Close();
                            }
                            if (Create.AdCodeExternal != null)
                            {
                                ViewBag.AdCodeExternal = Create.AdCodeExternal;
                            }
                            if (Create.AdCodeInternal != null)
                            {
                                ViewBag.AdCodeInternal = Create.AdCodeInternal;
                            }
                            ViewBag.InsertError = "There was an error submitting your Ad. If there are the other error labels are not descrptive enough, please reach out to Jake Jordan for assistance/troubleshooting.";
                            return View("Index");
                        }
                    }
                }       
            }
            
        }
    }
}
