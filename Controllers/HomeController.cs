using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OldOneWinDB.Models;
using OldOneWinDB.Models.ViewModels;

namespace OldOneWinDB.Controllers
{
    public class HomeController : Controller
    {
        int pageSize = 50;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _cache;
        List<ConnectionStrings> connStrs;

        public HomeController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            connStrs = new List<ConnectionStrings>();
            connStrs.Add(new ConnectionStrings("Центральный", "Server=tcp:192.168.209.208, 1433; Database=base_cen;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Фрунзенский", "Server=tcp:192.168.209.208, 1433; Database=base_frun;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Лененский", "Server=tcp:192.168.209.208, 1433; Database=base_len;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Мингорисполком", "Server=tcp:192.168.209.208, 1433; Database=base_mingor;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Мингорисполком 2", "Server=tcp:192.168.209.208, 1433; Database=base_mingor_ag;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Московский", "Server=tcp:192.168.209.208, 1433; Database=base_mos;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Октябрьский", "Server=tcp:192.168.209.208, 1433; Database=base_okt;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Партизанский", "Server=tcp:192.168.209.208, 1433; Database=base_par;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Первомайский", "Server=tcp:192.168.209.208, 1433; Database=base_per;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Советский", "Server=tcp:192.168.209.208, 1433; Database=base_sov;User Id=sa; Password=jlyjjryj;"));
            connStrs.Add(new ConnectionStrings("Заводской", "Server=tcp:192.168.209.208, 1433; Database=base_zav;User Id=sa; Password=jlyjjryj;"));
            _hostingEnvironment = hostingEnvironment;
        }
      
        public IActionResult Index()
        {

            _cache.Remove("ConnectionString");
            _cache.Remove("RegList");
            _cache.Remove("AllRegList");
            HttpContext.Session.Remove("Year");
            ViewBag.connStrs = connStrs;
            return View();
        }

        public IActionResult SelectYear(ConnectionStrings model)
        {
            if (model.AreaName == null && model.ConnectionString == null && _cache.Get("ConnectionString") == null)
                return RedirectToAction("Index");
            _cache.Remove("SelectedPeripod");
            if (_cache.Get("ConnectionString")== null)
            {
                model = connStrs.FirstOrDefault(x => x.AreaName == model.AreaName);
                _cache.Set("ConnectionString", model);
            }
            List<string> years = new List<string>();
            years.Add("2006");
            years.Add("2007");
            years.Add("2008");
            years.Add("2009");
            years.Add("2010");
            years.Add("2011");
            years.Add("2012");
            years.Add("2013");
            years.Add("2014");
            years.Add("2015");         
            ViewBag.Years = years;
            return View();
        }

        public IActionResult FindForm(SelectPeriodModel model)
        {
            if (_cache.Get("ConnectionString") == null)
                return RedirectToAction("Index");
            if (model.BeginYear == null && model.EndYear == null && (SelectPeriodModel)_cache.Get("SelectedPeripod") == null)
                return RedirectToAction("SelectYear", new { model = _cache.Get("ConnectionString") });
                List<TblDocRegistry> parentsList = GetDocRegistries().Where(x => x.ParrentId == null).OrderBy(x => x.RegName).ToList();
            ViewBag.parentsList = parentsList;
            //list = GetRegistrationsList(model);
            //list = list.OrderBy(x => x.GettingDate).ToList();
            if ((SelectPeriodModel)_cache.Get("SelectedPeripod") != null)            
                _cache.Remove("FindParamsModel");           
            else
                _cache.Set("SelectedPeripod", model);
            return View();
        }

        public IActionResult getRegListAjax(int? id)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var res = GetRegistrationsViewModelsList(GetRegistrationsList(id.Value));
                return PartialView("_RegistrationItems", res);
            }
            return PartialView("_RegistrationItems");
        }

        public IActionResult RegList(FindParamsModel model, int? id)
        {
            if (model == null)
            {
                if (_cache.Get("ConnectionString") == null)
                    return RedirectToAction("Index");
            }
            _cache.Set("FindParamsModel", model);
            ViewBag.RegCount = GetRegistrationListCount();
            return View(GetRegistrationsViewModelsList(GetRegistrationsList()));
        }

        public IActionResult RegCard(Guid RegistrationID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            return View(GetRegistrationItem(RegistrationID));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetRegCard(Guid RegistrationID)
        {
            var model = GetRegistrationItem(RegistrationID);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "temp", CreateRegCard(model));
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        string getRegistrationListSQLQuery(string QueryType = "*")
        {
            FindParamsModel findParamsModel = (FindParamsModel)_cache.Get("FindParamsModel");
            SelectPeriodModel selectPeriodModel = (SelectPeriodModel)_cache.Get("SelectedPeripod");
            string sqlExpression = String.Format("SELECT {0} FROM tblRegistration WHERE GettingDate > '1.1.{1}' AND GettingDate < '31.12.{2}'", QueryType, selectPeriodModel.BeginYear, selectPeriodModel.EndYear);

            if (findParamsModel.DocNo != null && findParamsModel.DocNo != 0)
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = {1} ", "DocNo", findParamsModel.DocNo);
            }
            if (!String.IsNullOrEmpty(findParamsModel.Fname))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "Fname", findParamsModel.Fname);
            }
            if (!String.IsNullOrEmpty(findParamsModel.Lname))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "Lname", findParamsModel.Lname);
            }
            if (!String.IsNullOrEmpty(findParamsModel.Mname))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "Mname", findParamsModel.Mname);
            }
            if (!String.IsNullOrEmpty(findParamsModel.Adres))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "Adres", findParamsModel.Adres);
            }
            if (findParamsModel.StartOutDeptDate != null && findParamsModel.StartOutDeptDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} >= '{1}' ", "OutDeptDate", findParamsModel.StartOutDeptDate.ToShortDateString());
            }
            if (findParamsModel.EndOutDeptDate != null && findParamsModel.EndOutDeptDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} <= '{1}' ", "OutDeptDate", findParamsModel.EndOutDeptDate.ToShortDateString());
            }
            if (findParamsModel.StartReturnInDeptDate != null && findParamsModel.StartReturnInDeptDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} >= '{1}' ", "ReturnInDeptDate", findParamsModel.StartReturnInDeptDate.ToShortDateString());
            }
            if (findParamsModel.EndReturnInDeptDate != null && findParamsModel.EndReturnInDeptDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} <= '{1}' ", "ReturnInDeptDate", findParamsModel.EndReturnInDeptDate.ToShortDateString());
            }
            if (findParamsModel.StartIssueDate != null && findParamsModel.StartIssueDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} >= '{1}' ", "IssueDate", findParamsModel.StartIssueDate.ToShortDateString());
            }
            if (findParamsModel.EndIssueDate != null && findParamsModel.EndIssueDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} <= '{1}' ", "IssueDate", findParamsModel.EndIssueDate.ToShortDateString());
            }
            if (findParamsModel.StartMustBeReadyDate != null && findParamsModel.StartMustBeReadyDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} >= '{1}' ", "MustBeReadyDate", findParamsModel.StartMustBeReadyDate.ToShortDateString());
            }
            if (findParamsModel.EndMustBeReadyDate != null && findParamsModel.EndMustBeReadyDate != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} <= '{1}' ", "MustBeReadyDate", findParamsModel.EndMustBeReadyDate.ToShortDateString());
            }
            if (findParamsModel.DateSsolutions != null && findParamsModel.DateSsolutions != new DateTime(1, 1, 1))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "DateSsolutions", findParamsModel.DateSsolutions.Value.ToShortDateString());
            }
            if (!String.IsNullOrEmpty(findParamsModel.NumberSolutions))
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "NumberSolutions", findParamsModel.NumberSolutions);
            }
            if (findParamsModel.ResultType != null)
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = {1} ", "ResultType", Convert.ToInt32(findParamsModel.ResultType.Value));
            }
            if (findParamsModel.ProcedureId != null && findParamsModel.ProcedureId != Guid.Empty)
            {
                sqlExpression = sqlExpression + String.Format("AND {0} = '{1}' ", "RegID", findParamsModel.ProcedureId.ToString());
            }
            if (QueryType == "*")
                sqlExpression = sqlExpression + " order by GettingDate";

            return sqlExpression;
        }

        List<TblRegistration> getRegistrationListExecuteSqlQuery(string query)
        {
            List<TblRegistration> result = new List<TblRegistration>();
            List<TblDocRegistry> tblDocRegistries = GetDocRegistries();
            ConnectionStrings connString = (ConnectionStrings)_cache.Get("ConnectionString");
            string connectionString = connString.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        TblRegistration tblRegistration = new TblRegistration();
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                        tblRegistration.RegistrationId = reader.GetGuid(0);
                        tblRegistration.Fname = (reader["Fname"] == DBNull.Value) ? null : reader["Fname"].ToString();
                        tblRegistration.Mname = (reader["Mname"] == DBNull.Value) ? null : reader["Mname"].ToString();
                        tblRegistration.Lname = (reader["Lname"] == DBNull.Value) ? null : reader["Lname"].ToString();
                        tblRegistration.Address = (reader["Address"] == DBNull.Value) ? null : reader["Address"].ToString();
                        tblRegistration.Home = (reader["Home"] == DBNull.Value) ? null : (int?)reader["Home"];
                        tblRegistration.Flat = (reader["Flat"] == DBNull.Value) ? null : reader["Flat"].ToString();
                        tblRegistration.PhoneNo = (reader["PhoneNo"] == DBNull.Value) ? null : reader["PhoneNo"].ToString();
                        tblRegistration.PassportNo = (reader["PassportNo"] == DBNull.Value) ? null : reader["PassportNo"].ToString();
                        tblRegistration.PassIssueDate = (reader["PassIssueDate"] == DBNull.Value) ? null : (DateTime?)reader["PassIssueDate"];
                        tblRegistration.PassIssuer = (reader["PassIssuer"] == DBNull.Value) ? null : reader["PassIssuer"].ToString();
                        tblRegistration.PersonalNo = (reader["PersonalNo"] == DBNull.Value) ? null : reader["PersonalNo"].ToString();
                        tblRegistration.RegId = (reader["RegId"] == DBNull.Value) ? Guid.Empty : Guid.Parse(reader["RegId"].ToString());
                        tblRegistration.Registrator = (reader["Registrator"] == DBNull.Value) ? null : (byte?)reader["Registrator"];
                        tblRegistration.GettingDate = (reader["GettingDate"] == DBNull.Value) ? null : (DateTime?)reader["GettingDate"];
                        tblRegistration.OutDeptDate = (reader["OutDeptDate"] == DBNull.Value) ? null : (DateTime?)reader["OutDeptDate"];
                        tblRegistration.ReturnInDeptDate = (reader["ReturnInDeptDate"] == DBNull.Value) ? null : (DateTime?)reader["ReturnInDeptDate"];
                        tblRegistration.IssueDate = (reader["IssueDate"] == DBNull.Value) ? null : (DateTime?)reader["IssueDate"];
                        tblRegistration.MustBeReadyDate = (reader["MustBeReadyDate"] == DBNull.Value) ? null : (DateTime?)reader["MustBeReadyDate"];
                        tblRegistration.ResultType = (reader["ResultType"] == DBNull.Value) ? null : (bool?)reader["ResultType"];
                        tblRegistration.Deleted = reader.GetBoolean(20);
                        tblRegistration.State = (reader["State"] == DBNull.Value) ? null : (byte?)reader["State"];
                        tblRegistration.Notes = reader.GetString(22);
                        tblRegistration.OrderNo = reader.GetInt32(24);
                        tblRegistration.DocNo = (reader["DocNo"] == DBNull.Value) ? null : (int?)reader["DocNo"];
                        tblRegistration.KolB = (reader["KolB"] == DBNull.Value) ? null : (byte?)reader["KolB"];
                        tblRegistration.Organiz = (reader["Organiz"] == DBNull.Value) ? null : reader["Organiz"].ToString();
                        tblRegistration.Vid = (reader["Vid"] == DBNull.Value) ? null : (byte?)reader["Vid"];
                        tblRegistration.Room = (reader["Room"] == DBNull.Value) ? null : (byte?)reader["Room"];
                        tblRegistration.KolList = (reader["KolList"] == DBNull.Value) ? null : (byte?)reader["KolList"];
                        tblRegistration.KolListPril = (reader["KolListPril"] == DBNull.Value) ? null : (byte?)reader["KolListPril"];
                        tblRegistration.StatementForm = (reader["StatementForm"] == DBNull.Value) ? null : reader["StatementForm"].ToString();
                        tblRegistration.Proceedings = (reader["Organiz"] == DBNull.Value) ? null : reader["Organiz"].ToString();
                        tblRegistration.DateSsolutions = (reader["DateSsolutions"] == DBNull.Value) ? null : (DateTime?)reader["DateSsolutions"];
                        tblRegistration.NumberSolutions = (reader["NumberSolutions"] == DBNull.Value) ? null : reader["NumberSolutions"].ToString();
                        tblRegistration.EvaluationNotification = (reader["EvaluationNotification"] == DBNull.Value) ? null : reader["EvaluationNotification"].ToString();
                        tblRegistration.EvaluationControl = (reader["EvaluationControl"] == DBNull.Value) ? null : reader["EvaluationControl"].ToString();
                        tblRegistration.CaseNumber = (reader["CaseNumber"] == DBNull.Value) ? null : reader["CaseNumber"].ToString();
                        tblRegistration.KolListCase = (reader["KolListCase"] == DBNull.Value) ? null : (byte?)reader["KolListCase"];

                        tblRegistration.TblDocRegistry = tblDocRegistries.Where(x => x.RegId == tblRegistration.RegId).FirstOrDefault();
                        if (tblRegistration.TblDocRegistry != null)
                            tblRegistration.TblDocRegistry.TblOrganization = GetTblOrganizations().FirstOrDefault(x => x.DeptId == tblRegistration.TblDocRegistry.DeptId);
                        result.Add(tblRegistration);
                    }
                }
                reader.Close();
            }
            return result;
        }

        List<TblRegistration> GetRegistrationsList(int page = 0)
        {
            string sqlExpression = getRegistrationListSQLQuery();
            sqlExpression = sqlExpression + String.Format(" OFFSET {0} row Fetch First 50 rows only", page == 0 ? 0 : page * 50);
            return getRegistrationListExecuteSqlQuery(sqlExpression);
        }

        int GetRegistrationListCount()
        {
            int result = 0;
            ConnectionStrings connectionString = (ConnectionStrings)_cache.Get("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(getRegistrationListSQLQuery("COUNT (*)"), connection);
                result = (int)command.ExecuteScalar();            
            }
            return result ;
        }

        List<TblOrganization> GetTblOrganizations()
        {
            ConnectionStrings connString = (ConnectionStrings)_cache.Get("ConnectionString");
            string connectionString = connString.ConnectionString;
            List<TblOrganization> result = new List<TblOrganization>();
            string sqlExpression = String.Format("SELECT * FROM TblOrganization");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        TblOrganization item = new TblOrganization();
                        item.DeptId = reader.GetGuid(0);
                        item.DeptName = reader.GetString(1);
                        item.Address = reader.GetString(2);
                        item.Cabinet = reader.GetString(3);
                        item.PhoneNo = reader.GetString(4);
                        item.Notes = reader.GetString(5);
                        result.Add(item);
                    }
                }
                reader.Close();
            }
            return result;
        }

        List<TblDocRegistry> GetDocRegistries()
        {
            ConnectionStrings connString = (ConnectionStrings)_cache.Get("ConnectionString");
            string connectionString = connString.ConnectionString;
            List<TblDocRegistry> result = new List<TblDocRegistry>();
            string sqlExpression = String.Format("SELECT * FROM tblDocRegistry order by RegName");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        TblDocRegistry item = new TblDocRegistry();
                        item.RegId = reader.GetGuid(0);
                        item.RegName = reader.GetString(1);

                        if (reader.IsDBNull(2))
                            item.ParrentId = null;
                        else
                            item.ParrentId = reader.GetGuid(2);
                        item.IssueTerms = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            item.DeptId = reader.GetGuid(4);
                        else
                            item.DeptId = Guid.Empty;
                        result.Add(item);
                    }
                }
                reader.Close();
            }
            return result;
        }

        public List<TblDocRegistry> GetDocRegistries(string parentID)
        {
            return GetDocRegistries().Where(x => x.ParrentId.ToString() == parentID).ToList();
        }      

        TblRegistration GetRegistrationItem(Guid RegistrationID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");           
            return getRegistrationListExecuteSqlQuery(String.Format("SELECT * FROM tblRegistration WHERE registrationID='{0}' ", RegistrationID.ToString())).First();
        }

        List<RegistrationsViewModel> GetRegistrationsViewModelsList(List<TblRegistration> regList)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            List<RegistrationsViewModel> result = new List<RegistrationsViewModel>();
            foreach (var item in regList)
            {
                RegistrationsViewModel registrationsViewModel = item.GetRegistrationsViewModel();              
                result.Add(registrationsViewModel);
            }
            return result;
        }



        protected string CreateRegCard(TblRegistration registration)
        {
            //   string initialPath= _hostingEnvironment.ContentRootPath + @"\Template\regcard.docx";
            // string targetfile = _hostingEnvironment.ContentRootPath + @"\temp\" + registration.RegistrationId.ToString() + "_rk.docx";
            
            string initialPath= Path.GetFullPath(_hostingEnvironment.ContentRootPath + "/Template/regcard.docx");
            string targetfile = Path.GetFullPath(_hostingEnvironment.ContentRootPath + @"\temp\" + registration.RegistrationId.ToString() + "_rk.docx"); 
            string resultFileName = registration.RegistrationId.ToString() + "_rk.docx";
            System.IO.File.Copy(initialPath, targetfile, true);
            using (WordprocessingDocument document = WordprocessingDocument.Open(targetfile, true))
            {
                MainDocumentPart mainPart = document.MainDocumentPart;
                var fields = mainPart.Document.Body.Descendants<FormFieldData>();
                var a = getDictionaryOfRegistration(registration);
                foreach (var item in getDictionaryOfRegistration(registration))
                {
                    foreach (var field in fields)
                    {
                        if (((FormFieldName)field.FirstChild).Val.InnerText.Equals(item.Key) ||
                            ((FormFieldName)field.FirstChild).Val.InnerText.Equals(item.Key + "1"))
                        {
                            TextInput text = field.Descendants<TextInput>().First();

                            SetFormFieldValue(text, String.IsNullOrEmpty(item.Value) ? "" : item.Value);
                        }
                    }
                }
                string docText = null;
                using (StreamReader sr = new StreamReader(document.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                var DictionaryOfRegistration = getDictionaryOfRegistration(registration);
                foreach (var item in DictionaryOfRegistration)
                {
                    docText = docText.Replace(item.Key, item.Value);
                }
                using (StreamWriter sw = new StreamWriter(document.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
            return resultFileName;

        }

        Dictionary<string, string> getDictionaryOfRegistration(TblRegistration registration)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {"DocNo", registration.DocNo.ToString()},
                {"LName", registration.Lname},
                {"MName", registration.Mname},
                {"FName", registration.Fname},
               
              
                {"Address", registration.Address + ", д." + registration.Home + ", кв." + registration.Flat},

                {"PhoneNo", registration.PhoneNo},
                {"PhoneNO", registration.PhoneNo},
                {"StatementForm", registration.StatementForm},
                {"GettingDate", registration.GettingDate.Value.ToShortDateString()},
                {"kollist", registration.KolList != null ? registration.KolList.ToString() : ""}, //?
                {"kolListPril", registration.KolListPril != null ? registration.KolListPril.ToString() : ""}, //?
                {
                    "RegName",registration.TblDocRegistry.RegName
                 },
                {"DeptName",
                    String.IsNullOrEmpty(registration.TblDocRegistry.TblOrganization.DeptName)? " ":
                    registration.TblDocRegistry.TblOrganization.DeptName
                },
                {"DeptPhoneNo",
                    String.IsNullOrEmpty(registration.TblDocRegistry.TblOrganization.PhoneNo)? " ":
                    registration.TblDocRegistry.TblOrganization.PhoneNo
                },
                {"Isp_address",
                    String.IsNullOrEmpty(registration.TblDocRegistry.TblOrganization.Address)? " ":
                    registration.TblDocRegistry.TblOrganization.Address
                },
                {"Isp_kab",
                    String.IsNullOrEmpty(registration.TblDocRegistry.TblOrganization.Cabinet)? " ":
                    registration.TblDocRegistry.TblOrganization.Cabinet
                },
                { "Isp_prim",
                    String.IsNullOrEmpty(registration.TblDocRegistry.TblOrganization.Notes)? " ":
                    registration.TblDocRegistry.TblOrganization.Notes
                },
                {"Proceedings", registration.Proceedings},
                {"LongReadyDate", registration.MustBeReadyDate.Value.ToShortDateString()},
                {"MustBeReadyDate", registration.MustBeReadyDate.Value.ToShortDateString()},
                {
                    "DateSsolutions",
                    registration.DateSsolutions != null ? registration.DateSsolutions.Value.ToShortDateString() : ""
                },
                {"NamberSolutions", registration.NumberSolutions},
                {"ResultType", registration.ResultType != null ? registration.ResultType == true ? "Положительно" : "Отрицательно" : ""},

                {
                    "OutDeptDate",
                    registration.OutDeptDate != null ? registration.OutDeptDate.Value.ToShortDateString() : ""
                },
                {"IssueDate", registration.IssueDate != null ? registration.IssueDate.Value.ToShortDateString() : ""},
                {"EvaluationNotificati", registration.EvaluationNotification}, // Имя поля в ворде имеет ограничение по количеству символов 
                {"CaseNamber", registration.CaseNumber},
                {"LoListCase", registration.KolListCase != null ? registration.KolListCase.ToString() : ""},
               

              
            };
            return dictionary;
        }

        private static void SetFormFieldValue(TextInput textInput, string value)
        {  // Code for http://stackoverflow.com/a/40081925/3103123

            if (value == null) // Reset formfield using default if set.
            {
                if (textInput.DefaultTextBoxFormFieldString != null && textInput.DefaultTextBoxFormFieldString.Val.HasValue)
                    value = textInput.DefaultTextBoxFormFieldString.Val.Value;
            }

            // Enforce max length.
            short maxLength = 0; // Unlimited
            if (textInput.MaxLength != null && textInput.MaxLength.Val.HasValue)
                maxLength = textInput.MaxLength.Val.Value;
            if (value != null && maxLength > 0 && value.Length > maxLength)
                value = value.Substring(0, maxLength);

            // Not enforcing TextBoxFormFieldType (read documentation...).
            // Just note that the Word instance may modify the value of a formfield when user leave it based on TextBoxFormFieldType and Format.
            // A curious example:
            // Type Number, format "# ##0,00".
            // Set value to "2016 was the warmest year ever, at least since 1999.".
            // Open the document and select the field then tab out of it.
            // Value now is "2 016 tht,tt" (the logic behind this escapes me).

            // Format value. (Only able to handle formfields with textboxformfieldtype regular.)
            if (textInput.TextBoxFormFieldType != null
            && textInput.TextBoxFormFieldType.Val.HasValue
            && textInput.TextBoxFormFieldType.Val.Value != TextBoxFormFieldValues.Regular)
                throw new ApplicationException("SetFormField: Unsupported textboxformfieldtype, only regular is handled.\r\n" + textInput.Parent.OuterXml);
            if (!string.IsNullOrWhiteSpace(value)
            && textInput.Format != null
            && textInput.Format.Val.HasValue)
            {
                switch (textInput.Format.Val.Value)
                {
                    case "Uppercase":
                        value = value.ToUpperInvariant();
                        break;
                    case "Lowercase":
                        value = value.ToLowerInvariant();
                        break;
                    case "First capital":
                        value = value[0].ToString().ToUpperInvariant() + value.Substring(1);
                        break;
                    case "Title case":
                        value = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value);
                        break;
                    default: // ignoring any other values (not supposed to be any)
                        break;
                }
            }

            // Find run containing "separate" fieldchar.
            Run rTextInput = textInput.Ancestors<Run>().FirstOrDefault();
            if (rTextInput == null) throw new ApplicationException("SetFormField: Did not find run containing textinput.\r\n" + textInput.Parent.OuterXml);
            Run rSeparate = rTextInput.ElementsAfter().FirstOrDefault(ru =>
               ru.GetType() == typeof(Run)
               && ru.Elements<FieldChar>().FirstOrDefault(fc =>
                  fc.FieldCharType == FieldCharValues.Separate)
                  != null) as Run;
            if (rSeparate == null) throw new ApplicationException("SetFormField: Did not find run containing separate.\r\n" + textInput.Parent.OuterXml);

            // Find run containg "end" fieldchar.
            Run rEnd = rTextInput.ElementsAfter().FirstOrDefault(ru =>
               ru.GetType() == typeof(Run)
               && ru.Elements<FieldChar>().FirstOrDefault(fc =>
                  fc.FieldCharType == FieldCharValues.End)
                  != null) as Run;
            if (rEnd == null) // Formfield value contains paragraph(s)
            {
                Paragraph p = rSeparate.Parent as Paragraph;
                Paragraph pEnd = p.ElementsAfter().FirstOrDefault(pa =>
                pa.GetType() == typeof(Paragraph)
                && pa.Elements<Run>().FirstOrDefault(ru =>
                   ru.Elements<FieldChar>().FirstOrDefault(fc =>
                      fc.FieldCharType == FieldCharValues.End)
                      != null)
                   != null) as Paragraph;
                if (pEnd == null) throw new ApplicationException("SetFormField: Did not find paragraph containing end.\r\n" + textInput.Parent.OuterXml);
                rEnd = pEnd.Elements<Run>().FirstOrDefault(ru =>
                   ru.Elements<FieldChar>().FirstOrDefault(fc =>
                      fc.FieldCharType == FieldCharValues.End)
                      != null);
            }

            // Remove any existing value.

            Run rFirst = rSeparate.NextSibling<Run>();
            if (rFirst == null || rFirst == rEnd)
            {
                RunProperties rPr = rTextInput.GetFirstChild<RunProperties>();
                if (rPr != null) rPr = rPr.CloneNode(true) as RunProperties;
                rFirst = rSeparate.InsertAfterSelf<Run>(new Run(new[] { rPr }));
            }
            rFirst.RemoveAllChildren<Text>();

            Run r = rFirst.NextSibling<Run>();
            while (r != rEnd)
            {
                if (r != null)
                {
                    r.Remove();
                    r = rFirst.NextSibling<Run>();
                }
                else // next paragraph
                {
                    Paragraph p = rFirst.Parent.NextSibling<Paragraph>();
                    if (p == null) throw new ApplicationException("SetFormField: Did not find next paragraph prior to or containing end.\r\n" + textInput.Parent.OuterXml);
                    r = p.GetFirstChild<Run>();
                    if (r == null)
                    {
                        // No runs left in paragraph, move other content to end of paragraph containing "separate" fieldchar.
                        p.Remove();
                        while (p.FirstChild != null)
                        {
                            OpenXmlElement oxe = p.FirstChild;
                            oxe.Remove();
                            if (oxe.GetType() == typeof(ParagraphProperties)) continue;
                            rSeparate.Parent.AppendChild(oxe);
                        }
                    }
                }
            }
            if (rEnd.Parent != rSeparate.Parent)
            {
                // Merge paragraph containing "end" fieldchar with paragraph containing "separate" fieldchar.
                Paragraph p = rEnd.Parent as Paragraph;
                p.Remove();
                while (p.FirstChild != null)
                {
                    OpenXmlElement oxe = p.FirstChild;
                    oxe.Remove();
                    if (oxe.GetType() == typeof(ParagraphProperties)) continue;
                    rSeparate.Parent.AppendChild(oxe);
                }
            }

            // Set new value.

            if (value != null)
            {
                // Word API use \v internally for newline and \r for para. We treat \v, \r\n, and \n as newline (Break).
                string[] lines = value.Replace("\r\n", "\n").Split(new char[] { '\v', '\n', '\r' });
                string line = lines[0];
                Text text = rFirst.AppendChild<Text>(new Text(line));
                if (line.StartsWith(" ") || line.EndsWith(" ")) text.SetAttribute(new OpenXmlAttribute("xml:space", null, "preserve"));
                for (int i = 1; i < lines.Length; i++)
                {
                    rFirst.AppendChild<Break>(new Break());
                    line = lines[i];
                    text = rFirst.AppendChild<Text>(new Text(lines[i]));
                    if (line.StartsWith(" ") || line.EndsWith(" ")) text.SetAttribute(new OpenXmlAttribute("xml:space", null, "preserve"));
                }
            }
            else
            { // An empty formfield of type textinput got char 8194 times 5 or maxlength if maxlength is in the range 1 to 4.
                short length = maxLength;
                if (length == 0 || length > 5) length = 5;
                rFirst.AppendChild(new Text(((char)8194).ToString()));
                r = rFirst;
                for (int i = 1; i < length; i++) r = r.InsertAfterSelf<Run>(r.CloneNode(true) as Run);
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsfficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

    }
}
