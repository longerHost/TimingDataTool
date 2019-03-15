using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using TimingDataTool.Model;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public class IntersectionDataFormViewModel : IIntersectionDataFormViewModel
    {
        public IList<Intersection> Intersections { set; get; }

        private IList<DataSet> filesDataSet;

        private Microsoft.Office.Interop.Excel.Application xlApp;
        private Workbook xlWorkBook;

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
            System.Data.DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            ds = new DataSet();
            for (int i = 0; i < dtSheetName.Rows.Count; i++)
            {
                System.Data.DataTable dt = new System.Data.DataTable();
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
            CheckIntersectionsValidation();
            xlWorkBook = CreateExcelWorkBook();
            Worksheet intersectionSheet = CreateIntersectionsWorkSheet();
            CreateIntersectionsTimingWorkSheet(Intersections);



            /*
            int intersecionSheetOffset = 10; //The starting index of intersecion details in each intersection sheet
            for(int k = 0; k < Intersections.Count; k++)
            {
                SchedulesFrom sf = new SchedulesFrom(Intersections[0]);
                Intersection isc = Intersections[k];
                System.Data.DataTable scheduleDt = sf.getSheduleTableWithIntersection(isc);

                // Creating timing pattern details for each intersection
                Worksheet intersectionTimingSheet;
                intersectionTimingSheet = (Worksheet)xlWorkBook.Worksheets.Add();

                string originalName = Intersections[k].Name;
                string officialName = originalName;
                if (originalName.Length > 31)
                {
                    officialName = originalName.Substring(0, 31);
                }

                intersectionTimingSheet.Name = officialName;

                // Add schedule information at each intersection sheet
                intersectionTimingSheet.Cells[1, 1] = isc.Name;
                // Add schedule form header(Weekday | plan 1 | plan 2 ... | plan 8 )
                for (int i = 0; i < scheduleDt.Columns.Count; i++)
                {
                    intersectionTimingSheet.Cells[2, i + 1] = scheduleDt.Columns[i].ColumnName;
                }

                // Add content
                for (int i = 0; i < scheduleDt.Rows.Count; i++)
                {
                    for (int j = 0; j < scheduleDt.Columns.Count; j++)
                    {
                        intersectionTimingSheet.Cells[i + 3, j + 1] = scheduleDt.Rows[i][j];
                    }
                }

                int frameHeight = 9; // set Frame height of each pattern

                // Timing details of current intersection
                int planNumber = 0;
                for(int dayIndex = 1; dayIndex <= isc.WholeWeeksDayPlan.Values.Count; dayIndex++)
                {
                    IList<DayPlan> plans = isc.WholeWeeksDayPlan.Values.ToList()[dayIndex-1];
                    for(int planIndex = 1; planIndex <= plans.Count; planIndex++)
                    {
                        DayPlan plan = plans[planIndex - 1];
                        PlanDetailsForm pf = new PlanDetailsForm(isc, isc.WholeWeeksDayPlan.Values.ToList()[dayIndex - 1][planIndex - 1]);
                        System.Data.DataTable detailsTable = pf.getPlanTableWithIntersection(isc, plan);

                        //Fill the sheet one by one
                        
                        //Add header
                        for (int i = 0; i < detailsTable.Columns.Count; i++)
                        {
                            //plan information
                            intersectionTimingSheet.Cells[intersecionSheetOffset + frameHeight * planNumber + 1, 1] = "Pattern Number: " + plan.DayPlanActionId.ToString();

                            //Plan header
                            intersectionTimingSheet.Cells[intersecionSheetOffset + frameHeight * planNumber + 2, i + 1] = detailsTable.Columns[i].ColumnName;
                        }

                        // Add content
                        for (int i = 0; i < detailsTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < detailsTable.Columns.Count; j++)
                            {
                                intersectionTimingSheet.Cells[intersecionSheetOffset + frameHeight * planNumber + 3 + i, j + 1] = detailsTable.Rows[i][j];
                            }
                        }

                        planNumber++;
                    }
                }
            }
            */

            object misValue = System.Reflection.Missing.Value;
            xlWorkBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            //Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            MessageBox.Show("Excel file created , you can find the file on: " + filePath);
        }

        private void CreateIntersectionsTimingWorkSheet(IList<Intersection> intersections)
        {
            foreach(Intersection i in intersections)
            {
                Worksheet intersectionTimingSheet;
                intersectionTimingSheet = (Worksheet)xlWorkBook.Worksheets.Add();
                intersectionTimingSheet.Name = FixNameToOfficialFormat(i.Name) ;
            }
        }

        private string FixNameToOfficialFormat(string originalName)
        {
            string officialName = originalName;
            if (originalName.Length > 31)
            {
                officialName = originalName.Substring(0, 31);
            }
            return officialName;
        }

        private Worksheet CreateIntersectionsWorkSheet()
        {
            Worksheet intersectionSheet;
            intersectionSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            intersectionSheet.Name = "Intersections";

            intersectionSheet.Cells[1, 1] = "Intersections";
            intersectionSheet.Cells[1, 2] = "ID";
            intersectionSheet.Cells[1, 3] = "Configuration";
            for (int i = 0; i < Intersections.Count; i++)
            {
                intersectionSheet.Cells[i + 2, 1] = Intersections[i].Name;
                intersectionSheet.Cells[i + 2, 2] = Intersections[i].Id;
                intersectionSheet.Cells[i + 2, 3] = Intersections[i].Config;
            }
            return intersectionSheet;
        }

        private Workbook CreateExcelWorkBook()
        {
            xlApp = CreateExcelApplicaitonInstance();
            object misValue = System.Reflection.Missing.Value;
            Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            return xlWorkBook;
        }

        private Microsoft.Office.Interop.Excel.Application CreateExcelApplicaitonInstance()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
            }
            return xlApp;
        }
        

        private void CheckIntersectionsValidation()
        {
            if (Intersections == null || Intersections.Count <= 0)
            {
                MessageBox.Show("Please import proper files");
                return;
            }
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
