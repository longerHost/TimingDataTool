using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using TimingDataTool.Model;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public class IntersectionDataFormViewModel : IIntersectionDataFormViewModel
    {
        private DataTable DayPlanTable;
        private DataTable PhaseTimesTable;
        private DataTable PatternsTable;
        private DataTable SplitsExpandedTable;

        private IDictionary<int, List<DataRow>> DayPlanData;
        private IList<DataRow> PhaseTimeOptionsData;
        private IList<DataRow> PatternsData;
        private IDictionary<int, List<DataRow>> SplitsExpandedData;

        private IList<DataSet> filesDataSet;

        public DataTable displayTable { get; set; }

        public IList<Intersection> ImportExcelFilesAndLoad(string[] filesPaths)
        {
            filesDataSet = new List<DataSet>();
            foreach (string filePath in filesPaths)
            {
                filesDataSet.Add(ImportExcel(filePath));
            }

            return LoadFilesContent(filesDataSet);
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
                intersections.Add(FillIntersectionModels(ds));
            }
            return intersections;
        }

        /// <summary>
        /// Fill the data to our intersection model
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private Intersection FillIntersectionModels(DataSet ds)
        {
            Intersection intersection = new Intersection();
            //
            displayTable = ds.Tables[2];
            IDictionary<string, string> headerDic = GetIntersectionInfo(ds.Tables[0]);
            intersection.Id = Convert.ToInt32(headerDic["ID"]);
            intersection.Name = headerDic["Name"];
            intersection.Config = headerDic["Configuration"];
            intersection.wholeWeeksDayPlan = GetWholeWeekDayPlans(ds);
            //
            return intersection;
        }

        /// <summary>
        /// Get the information in table header intersection details
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetIntersectionInfo(DataTable dt)
        {
            IDictionary<string, string> headerDic = new Dictionary<string, string>();

            foreach(DataRow r in dt.Rows)
            {
                if (r[0].ToString().Contains("Table")) break;
                string[] temp = r[0].ToString().Split(new[] { ": " }, StringSplitOptions.None);
                headerDic.Add(temp[0], temp[1]);
            }
            return headerDic;
        }

        /// <summary>
        /// Get all dayPlan information in Day Plan table
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private IDictionary<int, IList<DayPlan>> GetWholeWeekDayPlans(DataSet ds)
        {
            //Should move to initialization
            DayPlanTable = ds.Tables[0];
            PhaseTimesTable = ds.Tables[1];
            SplitsExpandedTable = ds.Tables[2];
            PatternsTable = ds.Tables[3];
            
            DayPlanData = GetValidDayPlanTableData(DayPlanTable);
            PhaseTimeOptionsData = GetValidPhaseTimeOptionsData(PhaseTimesTable);
            PatternsData = GetValidPatternsTableData(PatternsTable);
            SplitsExpandedData = GetSplitsExpandedTableData(SplitsExpandedTable);
            //
            Dictionary<int, IList<DayPlan>> wholeWeekDic = new Dictionary<int, IList<DayPlan>>();
            //Each day in a week
            foreach (int dayIndex in DayPlanData.Keys)
            {
                IList<DayPlan> oneDayPlans = GetOneDayDayPlans(dayIndex);
                wholeWeekDic.Add(dayIndex, oneDayPlans);
            }

            return wholeWeekDic;
        }

        /// <summary>
        /// Get all plan in one day
        /// </summary>
        /// <param name="dayIndex">Index of the day</param>
        /// <param name="dayPlanData"></param>
        /// <param name="phaseTimeOptionsData"></param>
        /// <param name="patternsData"></param>
        /// <param name="splitsExpandedData"></param>
        /// <returns></returns>
        private IList<DayPlan> GetOneDayDayPlans(int dayIndex)
        {
            IDictionary<string, List<string>> dayPlanDic = GetDayPlanDetailsInfo(dayIndex, DayPlanData);

            IList<string> hours = dayPlanDic["hours"];
            IList<string> minutes = dayPlanDic["minutes"];
            IList<string> actions = dayPlanDic["actions"];

            IList<DayPlan> dayPlans = new List<DayPlan>();

            //Each dayplan
            for (int planIndex = 0; planIndex < hours.Count(); planIndex++)
            {
                DayPlan sdp = GetSinglePlanInOneDay(dayIndex, planIndex, hours, minutes, actions);
                dayPlans.Add(sdp);
            }
            return dayPlans;
        }

        private DayPlan GetSinglePlanInOneDay(int dayIndex, int planIndex, IList<string> hours, IList<string> minutes, IList<string> actions)
        {
            DayPlan dp = new DayPlan();
            int actionNo = Convert.ToInt32(actions[planIndex]);
            dp.DayPlanActionId = actionNo;
            dp.DayPlanName = DayPlanTable.Rows[3][planIndex + 2].ToString();
            dp.Schedule = GetDayPlanSchedule(planIndex, hours, minutes);
            dp.TimingPlan = GetDayPlanTiming(actionNo);
            return dp;

        }

        private TimingPlan GetDayPlanTiming(int actionNo)
        {
            //get timing plan information
            TimingPlan tp = new TimingPlan();
            foreach (DataRow patternRow in PatternsData)
            {
                int patternIndex = Convert.ToInt32(patternRow.ItemArray[0].ToString().Split(' ').Last());
                bool mappingFounded = false;
                if(actionNo <= 16)
                {
                    if (patternIndex == actionNo + 8)
                    {
                        mappingFounded = true;
                    }
                }
                else
                {
                    if(patternIndex == actionNo)
                    {
                        mappingFounded = true;
                    }
                }

                if (mappingFounded) // There are problems here
                {
                    tp.CycleLength = Convert.ToInt32(patternRow.ItemArray[1].ToString()); // Cycle time
                    tp.SplitNumber = Convert.ToInt32(patternRow.ItemArray[3].ToString()); // Split number
                    tp.Offset = Convert.ToInt32(patternRow.ItemArray[2].ToString()); // Offset
                    tp.SequenceNumber = Convert.ToInt32(patternRow.ItemArray[4].ToString()); // Seq number
                    break;
                }
            }
            tp.split = GetTimingPlanSplit(tp.SplitNumber);
            return tp;
        }

        private Split GetTimingPlanSplit(int SplitNumber)
        {
            //get split information
            Split sp = new Split();
            IList<Phase> phases = GetSplitPhases(SplitNumber);
            sp.phase1 = phases[0];
            sp.phase2 = phases[1];
            sp.phase3 = phases[2];
            sp.phase4 = phases[3];
            sp.phase5 = phases[4];
            sp.phase6 = phases[5];
            sp.phase7 = phases[6];
            sp.phase8 = phases[7];
            return sp;
        }

        private IList<Phase> GetSplitPhases(int SplitNumber)
        {
            int mappingAction = SplitNumber;
            if(SplitNumber <= 16)
            {
                mappingAction = SplitNumber + 8;
            }

            IList<DataRow> splitInfo = SplitsExpandedData[mappingAction];

            DataRow timeRow = splitInfo[0];
            DataRow modeRow = splitInfo[1];
            DataRow coordinateRow = splitInfo[2];

            IList<Phase> phases = new List<Phase>();
            for (int l = 2; l <= 9; l++)
            {
                int cycleLength = Convert.ToInt32(timeRow[l].ToString());

                bool coor = false;
                if (coordinateRow[l].ToString() == "ON")
                {
                    coor = true;
                }
                string mode = modeRow[l].ToString();
                Phase p = new Phase(l - 1, cycleLength, coor, mode);
                phases.Add(p);
            }
            return phases;
        }

        private Schedule GetDayPlanSchedule(int planIndex, IList<string> hours, IList<string> minutes)
        {
            DateTime sdt = new DateTime(2000, 1, 1, Convert.ToInt32(hours[planIndex]), Convert.ToInt32(minutes[planIndex]), 0);
            DateTime edt = new DateTime();
            if (planIndex == hours.Count -1)
            {
                edt = new DateTime(2000, 1, 2, 0, 0, 0);
            }
            else
            {
                edt = new DateTime(2000, 1, 1, Convert.ToInt32(hours[planIndex + 1]), Convert.ToInt32(minutes[planIndex + 1]), 0);
            }

            return new Schedule(sdt, edt);
        }

        private IDictionary<string, List<string>> GetDayPlanDetailsInfo(int dayIndex, IDictionary<int, List<DataRow>> dayPlanData)
        {
            IDictionary<string, List<string>> dayPlanDic = new Dictionary<string, List<string>>();

            List<DataRow> dayDetails = dayPlanData[dayIndex];
            DataRow hourRow = dayDetails[0];
            DataRow minuteRow = dayDetails[1];
            DataRow actionRow = dayDetails[2];

            List<string> hours = new List<string>();
            List<string> minutes = new List<string>();
            List<string> actions = new List<string>();

            hours.Add("0");
            minutes.Add("0");
            actions.Add(actionRow.ItemArray[2].ToString());

            int i = 3;
            for (; i <= hourRow.ItemArray.Count(); i++)
            {
                string hour = hourRow.ItemArray[i].ToString();
                if (hour == "0")
                {
                    break;
                }
                hours.Add(hour);
            }

            int j = 3;
            for (; j < i; j++)
            {
                string minute = minuteRow.ItemArray[j].ToString();
                string action = actionRow.ItemArray[j].ToString();
                minutes.Add(minute);
                actions.Add(action);
            }
            dayPlanDic.Add("hours", hours);
            dayPlanDic.Add("minutes", minutes);
            dayPlanDic.Add("actions", actions);
            return dayPlanDic;
        }

        private IDictionary<int, List<DataRow>> GetSplitsExpandedTableData(DataTable st)
        {
            List<DataRow> validDataRows = new List<DataRow>();
            List<List<DataRow>> patternRows = new List<List<DataRow>>();

            foreach(DataRow r in st.Rows)
            {
                int patternIndex = 0;
                if(int.TryParse(r[0].ToString(), out patternIndex))
                {
                    if(patternIndex >= 9 && patternIndex <=30)
                    {
                        validDataRows.Add(r);
                    }
                }
            }
            return validDataRows.GroupBy(r => Convert.ToInt32(r[0])).ToDictionary(l => l.Key, l => l.ToList());
        }

        private IList<DataRow> GetValidPatternsTableData(DataTable pt)
        {
            List<DataRow> validDataRow = new List<DataRow>();
            foreach(DataRow r in pt.Rows)
            {
                string[] temp = r[0].ToString().Split(' ');
                if (temp.Length == 2)
                {
                    string pattern = temp[0];
                    if(pattern == "Pattern")
                    {
                        int patternIndex = 0;
                        if(int.TryParse(temp[1], out patternIndex))
                        {
                            if(patternIndex >= 0 && patternIndex <= 32)
                            {
                                validDataRow.Add(r);
                            }
                        }
                    }
                }
            }
            return validDataRow;
        }

        private IList<DataRow> GetValidPhaseTimeOptionsData(DataTable pt)
        {
            List<DataRow> validDataRow = new List<DataRow>();
            foreach(DataRow r in pt.Rows)
            {
                string term = r[0].ToString();
                if(term == "Walk" || term == "Ped Clearance" || term == "Yellow Clr" || term == "Red Clr")
                {
                    validDataRow.Add(r);
                }
            }
            return validDataRow;
        }

        private IDictionary<int, List<DataRow>> GetValidDayPlanTableData(DataTable dt)
        {
            List<DataRow> validDataRows = new List<DataRow>();
            List<List<DataRow>> dayRows = new List<List<DataRow>>();

            foreach (DataRow row in dt.Rows)
            {
                int dayIndex = 0;
                if (int.TryParse(row[0].ToString(), out dayIndex))
                {
                    if (dayIndex >= 1 && dayIndex <= 7)
                    {
                        validDataRows.Add(row);
                    }
                }
            }
            return validDataRows.GroupBy(r => Convert.ToInt32(r[0])).ToDictionary(l => l.Key, l => l.ToList());
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
    }
}
