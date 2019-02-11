using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model.DataModel
{
    public class Intersection
    {
        private int _id;
        private string  _name;
        private string _config;
        private IntersectionPresetInfo _presetInfo;
        private IDictionary<int, IList<DayPlan>> _wholeWeeksDayPlan;
        private IList<TimingPlan> _allTimings;

        private DataTable DayPlanTable;
        private DataTable PhaseTimesTable;
        private DataTable PatternsTable;
        private DataTable SplitsExpandedTable;

        private IDictionary<int, List<DataRow>> DayPlanData;
        private IList<DataRow> PhaseTimeOptionsData;
        private IList<DataRow> PatternsData;
        private IDictionary<int, List<DataRow>> SplitsExpandedData;

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Config
        {
            get
            {
                return _config;
            }
        }

        public IntersectionPresetInfo PresetInfo
        {
            get
            {
                return _presetInfo;
            }
        }

        public IDictionary<int, IList<DayPlan>> WholeWeeksDayPlan
        {
            get
            {
                return _wholeWeeksDayPlan;
            }
        }

        public IList<TimingPlan> AllTimings
        {
            get
            {
                return _allTimings;
            }
        }

        public Intersection(DataSet ds)
        {
            IDictionary<string, string> headerDic = GetIntersectionInfo(ds.Tables[0]);
            _id = Convert.ToInt32(headerDic["ID"]);
            _name = replaceIllegalCharInString(headerDic["Name"]);
            _config = headerDic["Configuration"];
            _wholeWeeksDayPlan = GetWholeWeekDayPlans(ds);
            _presetInfo = GetIntersectionPresetInformation(PhaseTimeOptionsData);
            _allTimings = GetAllTimingInformationOfIntersection();
        }

        private IList<TimingPlan> GetAllTimingInformationOfIntersection()
        {
            IList<TimingPlan> timings = new List<TimingPlan>();
            foreach(DataRow r in PatternsData)
            {
                timings.Add(GetTimingByPatternRow(r));
            }
            return timings;
        }

        /// <summary>
        /// Get the information in table header intersection details
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private IDictionary<string, string> GetIntersectionInfo(DataTable dt)
        {
            IDictionary<string, string> headerDic = new Dictionary<string, string>();

            foreach (DataRow r in dt.Rows)
            {
                if (r[0].ToString().Contains("Table")) break;
                string[] temp = r[0].ToString().Split(new[] { ": " }, StringSplitOptions.None);
                headerDic.Add(temp[0], temp[1]);
            }
            return headerDic;
        }

        private string replaceIllegalCharInString(string str)
        {
            if (str.Contains("*") || str.Contains("/") || str.Contains("]") || str.Contains("["))
            {
                str = str.Replace("*", "");
                str = str.Replace("/", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
            }
            return str;
        }

        /// <summary>
        /// Get all dayPlan information in Day Plan table
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private IDictionary<int, IList<DayPlan>> GetWholeWeekDayPlans(DataSet ds)
        {
            // Assign tables here
            // Note: the table order will change after enable editing on excel.
            // So here we use columns to identify the tables
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                if (i == 0)
                {
                    DayPlanTable = dt;
                    continue;
                }

                if (dt.Columns.Count == 5)
                {
                    PatternsTable = dt;
                    continue;
                }
                else if (dt.Columns.Count == 17)
                {
                    PhaseTimesTable = dt;
                    continue;
                }
                else
                {
                    SplitsExpandedTable = dt;
                    continue;
                }
            }

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

        private Schedule GetDayPlanSchedule(int planIndex, IList<string> hours, IList<string> minutes)
        {
            DateTime sdt = new DateTime(2000, 1, 1, Convert.ToInt32(hours[planIndex]), Convert.ToInt32(minutes[planIndex]), 0);
            DateTime edt = new DateTime();
            if (planIndex == hours.Count - 1)
            {
                edt = new DateTime(2000, 1, 2, 0, 0, 0);
            }
            else
            {
                edt = new DateTime(2000, 1, 1, Convert.ToInt32(hours[planIndex + 1]), Convert.ToInt32(minutes[planIndex + 1]), 0);
            }

            return new Schedule(sdt, edt);
        }

        // Get single timing plan
        private TimingPlan GetDayPlanTiming(int actionNo)
        {
            if (actionNo <= 0) actionNo = 1;  // if action invalid then let it run free(1) as default
            return GetTimingByPatternRow(PatternsData[actionNo - 1]);
        }

        private TimingPlan GetTimingByPatternRow(DataRow patternRow)
        {
            TimingPlan tp = new TimingPlan();
            tp.CycleLength = Convert.ToInt32(patternRow.ItemArray[1].ToString()); // Cycle time
            tp.SplitNumber = Convert.ToInt32(patternRow.ItemArray[3].ToString()); // Split number
            tp.Offset = Convert.ToInt32(patternRow.ItemArray[2].ToString()); // Offset
            tp.SequenceNumber = Convert.ToInt32(patternRow.ItemArray[4].ToString()); // Seq number
            tp.split = GetTimingPlanSplit(tp.SplitNumber);
            return tp;
        }

        private Split GetTimingPlanSplit(int SplitNumber)
        {
            if (SplitNumber <= 0) SplitNumber = 1;  //if invalid split number let it as default 1

            //get split information
            Split sp = new Split();
            sp.phases = GetSplitPhases(SplitNumber);
            return sp;
        }

        private IList<Phase> GetSplitPhases(int SplitNumber)
        {
            IList<DataRow> splitInfo = SplitsExpandedData[SplitNumber];

            DataRow timeRow = splitInfo[0];
            DataRow modeRow = splitInfo[1];
            DataRow coordinateRow = splitInfo[2];

            IList<Phase> phases = new List<Phase>();
            for (int l = 2; l <= timeRow.Table.Columns.Count - 1; l++)
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

        private IDictionary<int, List<DataRow>> GetSplitsExpandedTableData(DataTable st)
        {
            List<DataRow> validDataRows = new List<DataRow>();
            List<List<DataRow>> patternRows = new List<List<DataRow>>();

            foreach (DataRow r in st.Rows)
            {
                int patternIndex = 0;
                if (int.TryParse(r[0].ToString(), out patternIndex))
                {
                    validDataRows.Add(r);
                }
            }

            return validDataRows.GroupBy(r => Convert.ToInt32(r[0])).ToDictionary(l => l.Key, l => l.ToList());
        }

        private IList<DataRow> GetValidPatternsTableData(DataTable pt)
        {
            List<DataRow> validDataRow = new List<DataRow>();
            foreach (DataRow r in pt.Rows)
            {
                string[] temp = r[0].ToString().Split(' ');
                if (temp.Length == 2)
                {
                    string pattern = temp[0];
                    if (pattern == "Pattern")  // filter out invalid data
                    {
                        int patternIndex = 0;
                        if (int.TryParse(temp[1], out patternIndex))
                        {
                            validDataRow.Add(r);
                        }
                    }
                }
            }
            return validDataRow;
        }

        private IList<DataRow> GetValidPhaseTimeOptionsData(DataTable pt)
        {
            List<DataRow> validDataRow = new List<DataRow>();
            foreach (DataRow r in pt.Rows)
            {
                string term = r[0].ToString();
                if (term == "Walk" || term == "Ped Clearance" || term == "Yellow Clr" || term == "Red Clr")
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

        private IntersectionPresetInfo GetIntersectionPresetInformation(IList<DataRow> PhaseData)
        {
            IntersectionPresetInfo info = new IntersectionPresetInfo();
            IList<string> phaseNameStrings = new List<string>(new string[] { "Phase 1", "Phase 2", "Phase 3", "Phase 4", "Phase 5", "Phase 6", "Phase 7", "Phase 8", "Phase 9", "Phase 10", "Phase 11", "Phase 12", "Phase 13", "Phase 14", "Phase 15", "Phase 16" });

            IDictionary<string, double> walkDic = new Dictionary<string, double>();
            IDictionary<string, double> pedClearDic = new Dictionary<string, double>();
            IDictionary<string, double> yellowCtrDic = new Dictionary<string, double>();
            IDictionary<string, double> redCtrDic = new Dictionary<string, double>();

            for (int i = 0; i < 16; i++)
            {
                double walkValue = Convert.ToDouble(PhaseData[0][i + 1].ToString());
                double pedValue = Convert.ToDouble(PhaseData[1][i + 1].ToString());
                double yellowCtrValue = Convert.ToDouble(PhaseData[2][i + 1].ToString());
                double redCtrValue = Convert.ToDouble(PhaseData[3][i + 1].ToString());

                walkDic.Add(phaseNameStrings[i], walkValue);
                pedClearDic.Add(phaseNameStrings[i], pedValue);
                yellowCtrDic.Add(phaseNameStrings[i], yellowCtrValue);
                redCtrDic.Add(phaseNameStrings[i], redCtrValue);
            }

            info.WalkInfo = walkDic;
            info.PedClearance = pedClearDic;
            info.YellowCtl = yellowCtrDic;
            info.RedCtr = redCtrDic;

            return info;
        }
    }
}
