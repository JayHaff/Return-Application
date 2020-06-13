using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NsExcel = Microsoft.Office.Interop.Excel;

namespace SNNReturn
{
    public class SavetoExcel
    {
       
        public SavetoExcel()
        {
         
        }


        public void Save_to_Excel(ObservableCollection<CalData> ReturnList)
        {
            try { 
            List<CalData>  OldList = ReturnList.ToList();
            List<CalData> NewList;
            
                NewList = Bubble_Sort(OldList);

                NsExcel.Application excapp = new Microsoft.Office.Interop.Excel.Application
                {

                    //if you want to make excel visible           
                    Visible = false
                };

                //create a blank workbook
                var workbook = excapp.Workbooks.Add(NsExcel.XlWBATemplate.xlWBATWorksheet);



                //Not done yet. You have to work on a specific sheet - note the cast
                //You may not have any sheets at all. Then you have to add one with NsExcel.Worksheet.Add()
                var sheet = (NsExcel.Worksheet)workbook.Sheets[1]; //indexing starts from 
               
                //do something usefull: you select now an individual cell
                //var range = sheet.get_Range("A1", "A1");
                //range.Value2 = "test"; //Value2 is not a typo
                int counter = 1;
                string cellName;

                cellName = "A" + counter.ToString();
                
                var range1 = sheet.get_Range(cellName, cellName);
                range1.NumberFormat = "@";
                range1.Value2 = "Ge Serial No";
                
                


                cellName = "B" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Ge Cat NO";
                


                cellName = "C" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "AMR Serial No";


                cellName = "D" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "AMR Assembly";

                cellName = "E" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Error Code";

           

                cellName = "F" + counter.ToString();
               range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Physical Defect";
                

                cellName = "G" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Description";
                


                cellName = "H" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Return to Manufacturer";
                


                cellName = "I" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Return to Productuion";

                cellName = "J" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Scrap";

                cellName = "K" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Further Analysis";

                cellName = "L" + counter.ToString();
                range1 = sheet.get_Range(cellName, cellName);
                range1.Value2 = "Date Processed";
                





                //now the list

                counter = 2;
                foreach (var item in NewList)
                {
                    
                    
                    cellName = "A" + counter.ToString();
                    var range = sheet.get_Range(cellName, cellName);
                    range.NumberFormat = "@";
                    range.Value2 = item.ge_serial_no.Trim();
                    


                    cellName = "B" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.assembly_catalog_no.Trim();
                    



                    cellName = "C" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.amr_serial_no.Trim();


                    cellName = "D" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.amr_assembly_no.Trim();

                    cellName = "E" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.reject_reason_1.Trim();
                    


                    cellName = "F" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Physical_defect.ToString();
                    

                    cellName = "G" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Description.Trim();
                    


                    cellName = "H" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Return_Manufacturer.ToString();
                    
                

                    cellName = "I" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Return_Production.ToString();

                    cellName = "J" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Scrap.ToString();

                    cellName = "K" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Further_Analysis.ToString();

                    cellName = "L" + counter.ToString();
                    range = sheet.get_Range(cellName, cellName);
                    range.Value2 = item.Date_Documented.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                    



                    ++counter;
                }
                sheet.Columns.AutoFit();

                //you've probably got the point by now, so a detailed explanation about workbook.SaveAs and workbook.Close is not necessary
                //important: if you did not make excel visible terminating your application will terminate excel as well - I tested it
                //but if you did it - to be honest - I don't kno
                workbook.SaveAs(@"\\som-fs02\I210\Jbl_SSN_Return\Jabil_SSN_" + DateTime.Now.ToString("dddd, dd MMMM yyyy ") + NewList.First().ge_serial_no.Trim() +"-" + NewList.Last().ge_serial_no.Trim()+ ".xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
                Missing.Value, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
        Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlUserResolution, true,
        Missing.Value, Missing.Value, Missing.Value);
               
                
                workbook.Close();
             
                excapp.Quit();
              
                Marshal.ReleaseComObject(workbook);
    
                Marshal.ReleaseComObject(excapp);
            }

             catch(Exception ex)
            {

            }
        }

        private List<CalData> Bubble_Sort(List<CalData> unsorted_list)
        {
            int n = unsorted_list.Count;
            List<CalData> sorted_list = new List<CalData>();
            int x;
            int y;

           for (int j = 0;j < n; j++ )
            {
                sorted_list.Add(unsorted_list.ElementAt(j));
                
            }

            try
            {


                for (int i = 0; i < n - 1; i++)
                {


                    for (int j = 0; j < n - i - 1; j++)
                    {
                        CalData index1 = new CalData();
                        CalData index2= new CalData();

                        if (unsorted_list[j].ge_serial_no.Trim().Contains('X') || unsorted_list[j].ge_serial_no.Trim().Contains('R') ||unsorted_list[j +1].ge_serial_no.Trim().Contains('X') || unsorted_list[j].ge_serial_no.Trim().Contains('R'))
                        {
                           
                             index1.ge_serial_no= unsorted_list.ElementAt(j).ge_serial_no.Replace('X', ' ').Trim();

                            index2.ge_serial_no = unsorted_list.ElementAt(j+1).ge_serial_no.Replace('X', ' ').Trim();

                            x = Convert.ToInt32(index1.ge_serial_no.Replace('R', ' ').Trim());
                            y = Convert.ToInt32(index2.ge_serial_no.Replace('R', ' ').Trim());


                        }
                        else
                        {
                            x = Convert.ToInt32(index1.ge_serial_no.Trim());
                            y = Convert.ToInt32(index2.ge_serial_no.Trim());
                        }

                        //Console.WriteLine(x);
                        //Console.WriteLine(y);


                        if (x > y)
                        {
                            // swap temp and arr[i] 
                            
                            CalData temp = sorted_list[j];
                            sorted_list[j] = sorted_list[j + 1];
                            sorted_list[j + 1] = temp;
                        }

                        //Console.WriteLine("index j: " + j + "  ");
                        //Console.WriteLine(sorted_list[j].ge_serial_no);
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            
            return sorted_list;
        }

    }
}
