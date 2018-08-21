using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private IMemoryCache _cache;
        List<TblRegistration> list;
        List<ConnectionStrings> connStrs;
        public HomeController(IMemoryCache memoryCache)
        {
           // db = applicationContext;
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

        }
       // baseContext db;

       
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
            _cache.Remove("RegList");
            _cache.Remove("AllRegList");
            HttpContext.Session.Remove("Year");
            if (_cache.Get("ConnectionString")== null)
            {
                model = connStrs.FirstOrDefault(x => x.AreaName == model.AreaName);
                _cache.Set("ConnectionString", model);
            }

            var Years = new List<SelectPeriodModel>();
            Years.Add(new SelectPeriodModel("2006"));
            Years.Add(new SelectPeriodModel("2007"));
            Years.Add(new SelectPeriodModel("2008"));
            Years.Add(new SelectPeriodModel("2009"));
            Years.Add(new SelectPeriodModel("2010"));
            Years.Add(new SelectPeriodModel("2011"));
            Years.Add(new SelectPeriodModel("2012"));
            Years.Add(new SelectPeriodModel("2013"));
            Years.Add(new SelectPeriodModel("2014"));
            Years.Add(new SelectPeriodModel("2015"));
            ViewBag.Years = Years;
            return View();
        }
        public IActionResult FindForm(SelectPeriodModel model)
        {
            
            _cache.Remove("RegList");
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("Year")))
            {
                _cache.Remove("AllRegList");
                list = GetRegistrations(model.Year);
                list = list.OrderBy(x => x.GettingDate).ToList();
                _cache.Set("AllRegList", list);
                HttpContext.Session.SetString("Year", model.Year);
            }


            return View();
        }
        public List<TblRegistration> GetRegistrations(string Year)
        {
            ConnectionStrings connString = (ConnectionStrings)_cache.Get("ConnectionString");
            string connectionString = connString.ConnectionString;
            List<TblRegistration> result = new List<TblRegistration>();
            string sqlExpression = String.Format("SELECT * FROM tblRegistration WHERE GettingDate > '1.1.{0}' AND GettingDate < '31.12.{0}'", Year);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
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
                        //      tblRegistration.Nprav = reader.GetString(26);
                        tblRegistration.KolB = (reader["KolB"] == DBNull.Value) ? null: (byte?)reader["KolB"];// (byte?)reader.GetValue(29);
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


                        result.Add(tblRegistration);


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
            string sqlExpression = String.Format("SELECT * FROM tblDocRegistry");
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
                        result.Add(item);
                    }
                }
                reader.Close();
            }
            return result;
        }

        public IActionResult getRegListAjax(int? id)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                int page = id ?? 0;
                var itemsToSkip = page * pageSize;
                List<TblRegistration> tempRegistrationsList = (List<TblRegistration>)_cache.Get("RegList");
                tempRegistrationsList = tempRegistrationsList.Skip(itemsToSkip).Take(pageSize).ToList();
                var res = GetRegistrationsViewModelsList(tempRegistrationsList);
                return PartialView("_RegistrationItems", res);
            }
            return PartialView("_RegistrationItems");
        }
        public IActionResult RegList(FindParamsModel model, int? id)
        {
          
            //if ((List<TblRegistration>)_cache.Get("RegList") != null)
            //{
            //    return View(GetRegistrationsViewModelsList((List<TblRegistration>)_cache.Get("RegList")).Take(pageSize));
            //}
            
            var result = (List<TblRegistration>)_cache.Get("AllRegList");
         
            if (model.DocNo != null && model.DocNo != 0)
            {
                result = result.Where(x => x.DocNo == model.DocNo).ToList();
            }
            if (!String.IsNullOrEmpty(model.Fname))
            {
                result = result.Where(x => x.Fname.Contains(model.Fname)).ToList();
            }
            if (!String.IsNullOrEmpty(model.Lname))
            {
                result = result.Where(x => x.Fname.Contains(model.Lname)).ToList();
            }
            if (!String.IsNullOrEmpty(model.Mname))
            {
                result = result.Where(x => x.Fname.Contains(model.Mname)).ToList();
            }
            if (!String.IsNullOrEmpty(model.Adres))
            {
                result = result.Where(x => x.Address.Contains(model.Adres)).ToList();
            }
            if (model.StartOutDeptDate != null && model.StartOutDeptDate != new DateTime(1, 1,1))
            {
                result = result.Where(x => x.OutDeptDate > model.StartOutDeptDate).ToList();
            }
            if (model.EndOutDeptDate != null && model.EndOutDeptDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.OutDeptDate < model.EndOutDeptDate).ToList();
            }

            if (model.StartReturnInDeptDate != null && model.StartReturnInDeptDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.ReturnInDeptDate > model.StartReturnInDeptDate).ToList();
            }
            if (model.EndReturnInDeptDate != null && model.EndReturnInDeptDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.ReturnInDeptDate < model.EndReturnInDeptDate).ToList();
            }

            if (model.StartIssueDate != null && model.StartIssueDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.IssueDate > model.StartIssueDate).ToList();
            }
            if (model.EndIssueDate != null && model.EndIssueDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.IssueDate < model.EndIssueDate).ToList();
            }

            if (model.StartMustBeReadyDate != null && model.StartMustBeReadyDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.MustBeReadyDate > model.StartMustBeReadyDate).ToList();
            }
            if (model.EndMustBeReadyDate != null && model.EndMustBeReadyDate != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.MustBeReadyDate < model.EndMustBeReadyDate).ToList();
            }

            if (model.DateSsolutions != null && model.DateSsolutions != new DateTime(1, 1, 1))
            {
                result = result.Where(x => x.DateSsolutions == model.DateSsolutions).ToList();
            }
            if (!String.IsNullOrEmpty(model.NumberSolutions))
            {
                result = result.Where(x => x.NumberSolutions == model.NumberSolutions).ToList();
            }
            _cache.Set("RegList", result);
            return View(GetRegistrationsViewModelsList(result.Take(pageSize).ToList()));
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    try
            //    {
            //        connection.Open();
            //        string sqlExpression = String.Format("SELECT GettingDate FROM Registration WHERE GettingDate >=CONVERT(date, '{0}-{1}-{2}') and GettingDate <= CONVERT(date, '{3}-{4}-{5}') {6}", beginDate.Year, beginDate.Month, beginDate.Day, endDate.Year, endDate.Month, endDate.Day,
            //            String.IsNullOrEmpty(procedureName) ? "" : String.Format("and Number='{0}'", procedureName));
            //        SqlCommand command = new SqlCommand(sqlExpression, connection);
            //        SqlDataReader reader = command.ExecuteReader();
            //        if (reader.HasRows)
            //        {
            //            while (reader.Read())
            //            {
            //                data.Add(reader.GetDateTime(0));
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        MessageBox.Show(e.Message, "Ошибка");
            //    }
            //    finally
            //    {
            //        connection.Close();
            //    }
            //}








            //if (HttpContext.Session.GetString("isLoad") == null)
            //{
            //    list = db.TblRegistration.Where(x => x.GettingDate > new DateTime(2010, 7, 7) && x.GettingDate < new DateTime(2011, 1, 1)).ToList();
            //    list = list.OrderBy(x => x.DocNo).ToList();
            //    _cache.Set("1", list);

            //    HttpContext.Session.SetString("isLoad", "True");
            //}
            //else
            //{
            //    list = (List<TblRegistration>)_cache.Get("1");
            //}

            //int page = id ?? 0;
            //var itemsToSkip = page * pageSize;


            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //{
            //    try
            //    {

            //        var model = GetRegistrationsViewModelsList(list
            //            .Skip(itemsToSkip)
            //            .Take(pageSize)
            //            .ToList());
            //        return PartialView("_RegistrationItems", model);
            //    }
            //    catch (Exception e)
            //    {

            //    }
            //}
            //return View(GetRegistrationsViewModelsList(
            //   list
            //    .Take(pageSize)
            //    .ToList()));
            //return View();
        }
        public IActionResult RegCard(Guid RegistrationID)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            List<TblRegistration> tblRegistrations = (List<TblRegistration>)_cache.Get("AllRegList"); //...FirstOrDefault(x => x.RegistrationId == RegistrationID);
            var model = tblRegistrations.FirstOrDefault(x => x.RegistrationId == RegistrationID);
            model.TblDocRegistry = GetDocRegistries().FirstOrDefault(x => x.RegId == model.RegId);
            return View(model);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private List<RegistrationsViewModel> GetItemsPage(List<TblRegistration> regLis, int page = 1)
        {
            var itemsToSkip = page * pageSize;

            return GetRegistrationsViewModelsList(regLis).OrderBy(t => t.DocNo).Skip(itemsToSkip).
                Take(pageSize).ToList();
        }
        public List<RegistrationsViewModel> GetRegistrationsViewModelsList(List<TblRegistration> regList)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            List<RegistrationsViewModel> result = new List<RegistrationsViewModel>();
            foreach (var item in regList)
            {
                RegistrationsViewModel registrationsViewModel = item.GetRegistrationsViewModel();
                if (item.RegId != null)
                {
                    var docRegistry = GetDocRegistries().FirstOrDefault(x => x.RegId == item.RegId);
                    if (docRegistry != null)
                        if (docRegistry.RegName != null)
                            registrationsViewModel.RegName = GetDocRegistries().FirstOrDefault(x => x.RegId == item.RegId).RegName;
                  
                }
                result.Add(registrationsViewModel);
            }
            return result;
        }

     
    }
}
