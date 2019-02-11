using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TimingDataTool.Model;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public class IntersectionDataFormViewModel : IIntersectionDataFormViewModel
    {
        public IList<Intersection> Intersections { set; get; }

        private IList<DataSet> filesDataSet;

        public IList<Intersection> ImportExcelFilesAndLoad(string[] filesPaths)
        {
            filesDataSet = new List<DataSet>();
            foreach (string filePath in filesPaths)
            {
                filesDataSet.Add(ImportExcel(filePath));
            }
            Intersections = LoadFilesContent(filesDataSet);
            return Intersections;
        }

        /// <summary>
        /// Load files to memory
        /// </summary>
        /// <param name="formsDataSet"></param>
        private IList<Intersection> LoadFilesContent(IList<DataSet> formsDataSet)
        {
            IList<Intersection> intersections = new List<Intersection>();
            foreach(DataSet ds in filesDataSet)
            {
                intersections.Add(new Intersection(ds));
            }
            return intersections;
        }

        private DataSet ImportExcel(string filePath)
        {
            DataSet ds = null;
            OleDbConnection conn;

            string strConn = string.Empty;
            string sheetName = string.Empty;

            try
            {
                // Excel 2003
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1;'";
                conn = new OleDbConnection(strConn);
                conn.Open();
            }
            catch
            {
                // Excel 2007
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                conn = new OleDbConnection(strConn);
                conn.Open();
            }

            //Get all sheets and push to dataset
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            ds = new DataSet();
            for (int i = 0; i < dtSheetName.Rows.Count; i++)
            {
                DataTable dt = new DataTable();
                dt.TableName = "table" + i.ToString();
                //get sheet name
                sheetName = dtSheetName.Rows[i]["TABLE_NAME"].ToString();
                OleDbDataAdapter oleda = new OleDbDataAdapter("select * from [" + sheetName + "]", conn);
                oleda.Fill(dt);
                ds.Tables.Add(dt);
            }

            //close connection and dispose it
            conn.Close();
            conn.Dispose();
            return ds;
        }

        /// <summary>
        /// Export data to excel file
        /// </summary>
        /// <param name="intersectionGridView"></param>
        public void ExportDataToExcel(DataGridView intersectionGridView, string filePath)
        {
            //Check intersection loaded
            if (Intersections == null || Intersections.Count <= 0)
            {
                MessageBox.Show("Please import proper files");
                return;
            }

            // creating Excel Application
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            // Creating new workbook
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            
            // Creating intial worksheet
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            CopyAllToClipboard(intersectionGridView);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.Name = "intersections";
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.get_Range("A1", Type.Missing);
            range.EntireColumn.Delete(Type.Missing);

            // Creating schedules worksheet
            Microsoft.Office.Interop.Excel.Worksheet scheduleSheet;
            scheduleSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.Add();
            scheduleSheet.Name = "Schedules";
            for(int k = 0; k < Intersections.Count; k++)
            {
                SchedulesFrom sf = new SchedulesFrom(Intersections[0]);
                Intersection isc = Intersections[k];
                DataTable scheduleDt = sf.getSheduleTableWithIntersection(isc);
                int multiplier = 9;

                // Add header
                for (int i = 0; i < scheduleDt.Columns.Count; i++)
                {
                    scheduleSheet.Cells[k * multiplier + 1, i + 1] = scheduleDt.Columns[i].ColumnName;
                }

                // Add content
                for (int i = 0; i < scheduleDt.Rows.Count; i++)
                {
                    for (int j = 0; j < scheduleDt.Columns.Count; j++)
                    {
                        scheduleSheet.Cells[k * multiplier + i + 2, j + 1] = scheduleDt.Rows[i][j];
                    }
                }

                // Creating timing details for each intersection
                Microsoft.Office.Interop.Excel.Worksheet intersectionTimingSheet;
                intersectionTimingSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.Add();

                string originalName = Intersections[k].Name;
                string officialName = originalName;
                if (originalName.Length > 31)
                {
                    officialName = originalName.Substring(0, 31);
                }

                intersectionTimingSheet.Name = officialName;

                int planNumber = 0;
                for(int dayIndex = 1; dayIndex <= isc.WholeWeeksDayPlan.Values.Count; dayIndex++)
                {
                    IList<DayPlan> plans = isc.WholeWeeksDayPlan.Values.ToList()[dayIndex-1];
                    for(int planIndex = 1; planIndex <= plans.Count; planIndex++)
                    {
                        DayPlan plan = plans[planIndex - 1];
                        PlanDetailsForm pf = new PlanDetailsForm(isc, isc.WholeWeeksDayPlan.Values.ToList()[dayIndex - 1][planIndex - 1]);
                        DataTable detailsTable = pf.getPlanTableWithIntersection(isc, plan);

                        //Fill the sheet one by one

                        //Add header
                        for (int i = 0; i < detailsTable.Columns.Count; i++)
                        {
                            //plan information
                            intersectionTimingSheet.Cells[multiplier * planNumber + 1, 1] = "Plan Index: " + (planNumber + 1).ToString();
                            intersectionTimingSheet.Cells[multiplier * planNumber + 1, 2] = "Day Index: " + dayIndex.ToString();
                            intersectionTimingSheet.Cells[multiplier * planNumber + 1, 3] = "Plan Start: " + plan.Schedule.StartTime.TimeOfDay.ToString();

                            //Plan header
                            intersectionTimingSheet.Cells[multiplier * planNumber + 2, i + 1] = detailsTable.Columns[i].ColumnName;
                        }

                        // Add content
                        for (int i = 0; i < detailsTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < detailsTable.Columns.Count; j++)
                            {
                                intersectionTimingSheet.Cells[multiplier * planNumber + 3 + i, j + 1] = detailsTable.Rows[i][j];
                            }
                        }

                        planNumber++;
                    }
                }
            }

            //Further to do  
            //1. load intersection with dataTable   ** Done
            //2. load intersection timing details to each sheet   ** Done
            //3. debug could not copy column 7 and 8 on schedule ** Done
            //4. put intersection and schedule tab front  
            //5. delete the empty column in intersection list ** Done
            //6. Mark intersection name for schedules  
            //7. Copy intersection list cell by cell instead of from UI  **ignore

            //8. Fix issue when encounter N/A problem

            //C. Export to files
            xlWorkBook.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            MessageBox.Show("Excel file created , you can find the file c:\\Users\\xiaolongm\\Desktop\\csharp-Excel.xls");
        }

        private static void CopyAllToClipboard(DataGridView dgv)
        {
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgv.SelectAll();
            DataObject dataObj = dgv.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
    }
}
