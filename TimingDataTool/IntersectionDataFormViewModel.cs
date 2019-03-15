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
            // Create
            CheckIntersectionsValidation();
            xlWorkBook = CreateExcelWorkBook();
            Worksheet intersectionSheet = CreateIntersectionsWorkSheet();
            CreateIntersectionsTimingWorkSheet(Intersections);

            // Save
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            MessageBox.Show("Excel file created , you can find the file on: " + filePath);
        }

        private void CreateIntersectionsTimingWorkSheet(IList<Intersection> intersections)
        {
            foreach(Intersection i in intersections)
            {
                CreateIntersectionTimingWorkSheet(i);
            }
        }

        private void CreateIntersectionTimingWorkSheet(Intersection intersection)
        {
            Worksheet timingSheet;
            timingSheet = (Worksheet)xlWorkBook.Worksheets.Add();
            timingSheet.Name = FixNameToOfficialFormat(intersection.Name);

            // Create Schedule Frame
            timingSheet.Cells[1, 1] = intersection.Name;

            // Header
            IList<string> scheduleColumnNames = new List<string>();
            scheduleColumnNames.Add("Week days");
            scheduleColumnNames.Add("Plan 1");
            scheduleColumnNames.Add("Plan 2");
            scheduleColumnNames.Add("Plan 3");
            scheduleColumnNames.Add("Plan 4");
            scheduleColumnNames.Add("Plan 5");
            scheduleColumnNames.Add("Plan 6");
            scheduleColumnNames.Add("Plan 7");
            scheduleColumnNames.Add("Plan 8");

            // Index
            for(int i = 1; i <= scheduleColumnNames.Count; i++)
            {
                timingSheet.Cells[2, i] = scheduleColumnNames[i-1];
            }

            // Content 
            for(int i = 1; i <= intersection.WholeWeeksDayPlan.Count; i++)
            {
                IList<DayPlan> plans = intersection.WholeWeeksDayPlan[i];
                timingSheet.Cells[i + 2, 1] = i.ToString();
                for(int j = 1; j <= 8; j++)
                {
                    string displayStr = "N/A";
                    if(j <= plans.Count)
                    {
                        DayPlan p = plans[j - 1];
                        string startTime = GetTimeHourAndMinuteString(p.Schedule.StartTime);
                        string endTime = GetTimeHourAndMinuteString(p.Schedule.EndTime);
                        string patternStr = p.DayPlanActionId.ToString();
                        displayStr = startTime + " - " + endTime + " (" + patternStr + ")";
                    }
                    timingSheet.Cells[i + 2, j + 1] = displayStr;
                }
            }
            //
            //

            // Create intersection common info frame
            int intersectionCommonFrameOffset = 10;
            timingSheet.Cells[intersectionCommonFrameOffset + 1, 1] = "Type/Phases";
            timingSheet.Cells[intersectionCommonFrameOffset + 2, 1] = "walk";
            timingSheet.Cells[intersectionCommonFrameOffset + 3, 1] = "Ped Clear";
            timingSheet.Cells[intersectionCommonFrameOffset + 4, 1] = "Yellow Ctr";
            timingSheet.Cells[intersectionCommonFrameOffset + 5, 1] = "Red Ctr";
            IList<string> planNames = new List<string>(new string[] { "Phase 1", "Phase 2", "Phase 3", "Phase 4", "Phase 5", "Phase 6", "Phase 7", "Phase 8", "Phase 9", "Phase 10", "Phase 11", "Phase 12", "Phase 13", "Phase 14", "Phase 15", "Phase 16" });
            for(int i = 0; i < planNames.Count; i++)
            {
                timingSheet.Cells[intersectionCommonFrameOffset + 1, i + 2] = planNames[i];
                timingSheet.Cells[intersectionCommonFrameOffset + 2, i + 2] = intersection.PresetInfo.WalkInfo[planNames[i]].ToString();
                timingSheet.Cells[intersectionCommonFrameOffset + 3, i + 2] = intersection.PresetInfo.PedClearance[planNames[i]].ToString();
                timingSheet.Cells[intersectionCommonFrameOffset + 4, i + 2] = intersection.PresetInfo.YellowCtl[planNames[i]].ToString();
                timingSheet.Cells[intersectionCommonFrameOffset + 5, i + 2] = intersection.PresetInfo.RedCtr[planNames[i]].ToString();
            }

            // Create Patterns Frames
            int patternsFrameOffset = intersectionCommonFrameOffset + 6;
            int patternFrameHeight = 5;

            // Find patterns need to display
            IList<DayPlan> displayPatterns = FindPatternsNeedToDisplay(intersection);

            for(int i = 0; i < displayPatterns.Count; i++)
            {
                DayPlan p = displayPatterns[i];

                int coordinatePhaseId = GetCoordinatePhaseId(p);
                timingSheet.Cells[patternFrameHeight* i + patternsFrameOffset + 1, 1] = "Pattern: " + p.DayPlanActionId;
                timingSheet.Cells[patternFrameHeight * i + patternsFrameOffset + 2, 1] = "Type/Phases";
                timingSheet.Cells[patternFrameHeight * i + patternsFrameOffset + 3, 1] = "Split";

                for (int j = 0; j < planNames.Count; j++)
                {
                    timingSheet.Cells[patternFrameHeight * i + patternsFrameOffset + 2, j + 2] = planNames[j];
                    if (j + 1 == coordinatePhaseId)
                    {
                        timingSheet.Cells[patternFrameHeight * i + patternsFrameOffset + 2, j + 2] = planNames[j] + "*";
                    }

                    timingSheet.Cells[patternFrameHeight * i + patternsFrameOffset + 3, j + 2] = p.TimingPlan.split.phases[j].Length;
                }
            }
        }

        private int GetCoordinatePhaseId(DayPlan plan)
        {
            foreach(Phase p in plan.TimingPlan.split.phases)
            {
                if(p.CoordinatePhase)
                {
                    return p.PhaseId;
                }
            }
            return 0;
        }

        private IList<DayPlan> FindPatternsNeedToDisplay(Intersection intersection)
        {
            IList<DayPlan> dayplans = new List<DayPlan>();
            foreach (IList<DayPlan> dps in intersection.WholeWeeksDayPlan.Values.ToList())
            {
                foreach (DayPlan dp in dps)
                {
                    dayplans.Add(dp);
                }
            }

            List<int> distinctPatternIds = dayplans.Select(e => e.DayPlanActionId).Distinct().ToList();

            IList<DayPlan> selectedDayPlans = new List<DayPlan>();
            foreach (DayPlan dp in dayplans)
            {
                if (distinctPatternIds.Contains(dp.DayPlanActionId) && distinctPatternIds.Count > 0)
                {
                    distinctPatternIds.Remove(dp.DayPlanActionId);
                    selectedDayPlans.Add(dp);
                }
            }

            IList<DayPlan> sortedSelectedDayplans = selectedDayPlans.OrderBy(p => p.DayPlanActionId).ToList();
            return sortedSelectedDayplans;
        }

        private string GetTimeHourAndMinuteString(DateTime dateTime)
        {
            string timeStr = dateTime.ToString("HH:mm");
            return timeStr;
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
