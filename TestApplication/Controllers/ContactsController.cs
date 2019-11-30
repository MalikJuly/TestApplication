using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestApplication.DAL;
using TestApplication.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestApplication.Controllers
{
    public class ContactsController : Controller
    {
        private TeamContext db = new TeamContext();

        public ActionResult Index()
        {
            ViewBag.DateList = DateList();
            return View();
        }

        public List<SelectListItem> DateList()
        {
            List<SelectListItem> dateList = new List<SelectListItem>
            {
              new SelectListItem() { Value = "", Text = "" }
            };
            var dates = Enumerable.Range(0, 1 + new DateTime(2019, 11, 28).Subtract(new DateTime(2019, 11, 25)).Days)
                .Select(offset => new DateTime(2019, 11, 25).AddDays(offset))
                .ToArray();
            foreach (var item in dates) {
                var textDate = String.Format("{0:MM-dd-yyyy}", item);
                dateList.Add(new SelectListItem() { Value = textDate, Text = textDate });           
            }
           
            return dateList;
        }
    
        public List<ContactViewModel> ContactsToContactViewModels(List<Contact> contacts)
        {
            var contactVMs = new List<ContactViewModel>();
            if(contacts != null)
            {
                foreach (var cont in contacts)
                {
                    var contactCompany = db.Companies.FirstOrDefault(x => x.company_id == cont.associated_company_id);
                    if (contactCompany != null)
                    {
                        cont.Company = contactCompany;
                        var recentContactWithCompany = new ContactViewModel
                        {
                            vid = cont.vid,
                            firstname = cont.firstname,
                            lastname = cont.lastname,
                            addedAt = cont.addedAt.ToShortDateString(),
                            lastmodifieddate = cont.lastmodifieddate != null ? ((DateTime)cont.lastmodifieddate).ToShortDateString() : "",
                            lifecyclestage = cont.lifecyclestage,
                            associated_company_id = cont.associated_company_id,
                            companyname = cont.Company.name,
                            companywebsite = cont.Company.website,
                            companycity = cont.Company.city,
                            companystate = cont.Company.state,
                            companyzip = cont.Company.zip,
                            companyphone = cont.Company.phone
                        };
                        contactVMs.Add(recentContactWithCompany);
                    }
                }
            }
            return contactVMs;
        }

        [HttpPost]
        public string GetRecentContactsDate(string modifiedOnOrAfter)
        {
            var modifiedDate = new DateTime();
            CultureInfo provider = CultureInfo.InvariantCulture;
            if(!String.IsNullOrEmpty(modifiedOnOrAfter))
                modifiedDate = DateTime.ParseExact(modifiedOnOrAfter, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, provider, DateTimeStyles.None);
          
            return GetRecentContacts(modifiedDate);
        }

        public string GetRecentContacts(DateTime modifiedOnOrAfter)
        {
            var monthEarlier = modifiedOnOrAfter.AddDays(-30);
            var allContacts = modifiedOnOrAfter != null ? db.Contacts.Where(x => x.addedAt >= monthEarlier && x.lastmodifieddate != null || x.lastmodifieddate >= monthEarlier).ToList() : null;
            var contactVMs = ContactsToContactViewModels(allContacts);

            var jsonSerialiser = new JavaScriptSerializer();
            var data = jsonSerialiser.Serialize(contactVMs);
          
            return data;
        }

        public List<string> GetPropertiesNameOfClass(ContactViewModel pObject)
        {
            List<string> propertyList = new List<string>();
            if (pObject != null)
            {
                foreach (var prop in pObject.GetType().GetProperties(BindingFlags.Public))
                {
                    propertyList.Add(prop.Name);
                }
            }
            return propertyList;
        }

        public List<string> GetPropertiesValueOfClass(ContactViewModel pObject)
        {
            List<string> propertyList = new List<string>();
            if (pObject != null)
            {
                foreach (var prop in pObject.GetType().GetProperties(BindingFlags.Public))
                {
                    var propValue = "";
                    if (prop.Name == "addedAt" || prop.Name == "lastmodifieddate")
                    {
                         propValue = String.Format("{0:MM/dd/yy}", prop.GetValue(pObject, null));
                    }
                    else
                         propValue = prop.GetValue(pObject, null).ToString(); 
                    propertyList.Add(propValue);
                }
            }
            return propertyList;
        }

        [HttpPost]
        public ActionResult GetExcelReport(List<Contact> contacts)
        {
            var contactVMs = ContactsToContactViewModels(contacts);
            if (contactVMs != null && contactVMs.Count > 0)
            {
                try
                { 
                    //Объявляем приложение
                    Excel.Application ex = new Excel.Application();
                    //Отобразить Excel
                    ex.Visible = true;
                    //Количество листов в рабочей книге
                    ex.SheetsInNewWorkbook = 2;
                    //Добавить рабочую книгу
                    Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
                    //Отключить отображение окон с сообщениями
                    ex.DisplayAlerts = false;
                    //Получаем первый лист документа (счет начинается с 1)
                    Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
                    //Название листа (вкладки снизу)
                    var dateNow = String.Format("{0:MM-dd-yy}", DateTime.Now.Date);
                    sheet.Name = "Report for " + dateNow;
                    var rowsN = contactVMs.Count;
                    var cellsN = 0;
                    //Пример заполнения ячеек
                    for (int i = 1; i <= rowsN; i++)
                    {
                        var props = GetPropertiesValueOfClass(contactVMs[i]);
                        cellsN = props.Count;
                        for (int j = 1; j < cellsN; ++j)
                            sheet.Cells[i, j] = String.Format("Boom {0} {1}", contactVMs[i], props[j]);
                    }
                    //Захватываем диапазон ячеек
                    Excel.Range range1 = sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[rowsN, cellsN]);
                    //Шрифт для диапазона
                    range1.Cells.Font.Name = "Tahoma";
                    //Размер шрифта для диапазона
                    range1.Cells.Font.Size = 10;
                    //Захватываем другой диапазон ячеек
                    Excel.Range range2 = sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[rowsN, 2]);
                    range2.Cells.Font.Name = "Tahoma";
                    //Задаем цвет этого диапазона.
                    range2.Cells.Font.Color = ColorTranslator.ToOle(Color.Green);
                    //Фоновый цвет
                    range2.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(0xFF, 0xFF, 0xCC));
                    //Выставляем рамки со всех сторон
                    range1.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range1.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range1.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range1.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range1.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                    
                    range2.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range2.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range2.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range2.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range2.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                    //Авто ширина и высота
                    range1.EntireColumn.AutoFit(); 
                    range1.EntireRow.AutoFit();
                    
                    range2.EntireColumn.AutoFit(); 
                    range2.EntireRow.AutoFit();
                    //Сохраняем документ
                    ex.Application.ActiveWorkbook.SaveAs("report_"+ dateNow + ".xlsx", Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //================================================
                    //Получаем документ
                    var excel = ex.Workbooks.Open(@"C:\Users\Malik July\Documents\report_" + dateNow + ".xlsx",
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing);
                    
                    
                    //Из документа в DataTable
                    
                    // Create a new DataTable for 1 Worksheet
                    DataTable dt = new DataTable();
                    
                    var ws = (Excel.Worksheet)excel.Worksheets.get_Item(1);
                    
                    //textBox1.Text = count.ToString();
                    
                    // Get range of the worksheet
                    var range = ws.UsedRange;
                    object[,] data = range.Value2;
                    
                    // Create new Column in DataTable
                    for (int cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                    {
                        //textBox3.Text = cCnt.ToString();
                        
                        var Column = new DataColumn
                        {
                            DataType = System.Type.GetType("System.String"),
                            ColumnName = cCnt.ToString()
                        };
                        dt.Columns.Add(Column);
                    
                        // Create row for Data Table
                        for (int rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                        {
                            //textBox2.Text = rCnt.ToString();
                    
                            string cellVal = String.Empty;
                            try
                            {
                                cellVal = (string)(data[rCnt, cCnt]);
                            }
                            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                            {
                            }
                    
                            DataRow Row;
                    
                            // Add to the DataTable
                            if (cCnt == 1)
                            {                    
                                Row = dt.NewRow();
                                Row[cCnt.ToString()] = cellVal;
                                dt.Rows.Add(Row);
                            }
                            else
                            {                    
                                Row = dt.Rows[rCnt + 1];
                                Row[cCnt.ToString()] = cellVal;                    
                            }
                        }
                    }
                    return PartialView("_ExcelDataTable", dt);
                }
                catch(Exception ex)
                {
                }
            }
            return PartialView("_ExcelDataTable", new DataTable());
        }
    }
}
