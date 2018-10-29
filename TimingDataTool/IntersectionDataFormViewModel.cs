using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimingDataTool.Model;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public class IntersectionDataFormViewModel : IIntersectionDataFormViewModel
    {
        private IList<DataSet> filesDataSet;

        public DataTable displayTable { get; set; }

        public void ImportExcelFilesAndLoad(string[] filesPaths)
        {
            filesDataSet = new List<DataSet>();
            foreach (string filePath in filesPaths)
            {
                filesDataSet.Add(ImportExcel(filePath));
            }

            LoadFilesContent(filesDataSet);
        }

        /// <summary>
        /// Load files to memory
        /// </summary>
        /// <param name="formsDataSet"></param>
        private void LoadFilesContent(IList<DataSet> formsDataSet)
        {
            foreach(DataSet ds in filesDataSet)
            {
                FillIntersectionModels(ds);
            }
        }

        private Intersection FillIntersectionModels(DataSet ds)
        {
            Intersection intersection = new Intersection();
            //
            displayTable = ds.Tables[2];
            IDictionary<string, string> headerDic = GetIntersectionInfo(ds.Tables[0]);
            intersection.Id = Convert.ToInt32(headerDic["ID"]);
            intersection.Name = headerDic["Name"];
            intersection.Config = headerDic["Configuration"];
            intersection.wholeWeeksDayPlan = GetValidIntersectionData(ds);
            //
            return intersection;
        }

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

        //IDictionary<int, IList<DayPlan>>
        private IDictionary<int, IList<DayPlan>> GetValidIntersectionData(DataSet ds) //Need to be changed
        {
            DataTable dayPlanTable = ds.Tables[0];
            DataTable phaseTimesTable = ds.Tables[2];
            DataTable PatternsTable = ds.Tables[1];
            DataTable splitsExpandedTable = ds.Tables[3];

            IDictionary<int, List<DataRow>> dayPlanData = GetValidDayPlanTableData(dayPlanTable);
            IList<DataRow> phaseTimeOptionsData = GetValidPhaseTimeOptionsData(phaseTimesTable);
            IList<DataRow> patternsData = GetValidPatternsTableData(PatternsTable);
            IDictionary<int, List<DataRow>> splitsExpandedData = GetSplitsExpandedTableData(splitsExpandedTable);

            return GetWholeWeekDayPlans(dayPlanData, phaseTimeOptionsData, patternsData, splitsExpandedData);
        }

        private IDictionary<int, IList<DayPlan>> GetWholeWeekDayPlans(IDictionary<int, List<DataRow>> dayPlanData, IList<DataRow> phaseTimeOptionsData, IList<DataRow> patternsData, IDictionary<int, List<DataRow>> splitsExpandedData)
        {
            Dictionary<int, IList<DayPlan>> wholeWeekDic = new Dictionary<int, IList<DayPlan>>();

            //Each day in a week
            foreach (int dayIndex in dayPlanData.Keys)
            {
                //For each day find out all its schedules and corresponding time plan(7 days in total)
                List<DataRow> dayDetails = dayPlanData[dayIndex];
                DataRow hourRow = dayDetails[0];
                DataRow minuteRow = dayDetails[1];
                DataRow actionRow = dayDetails[2];

                List<string> hours = new List<string>();
                List<string> minutes = new List<string>();
                List<string> actions = new List<string>();

                //time starting from 0:00
                hours.Add("0");
                minutes.Add("0");
                actions.Add(actionRow.ItemArray[2].ToString());

                int i = 3;
                for(; i <= hourRow.ItemArray.Count(); i++)
                {
                    string hour = hourRow.ItemArray[i].ToString();
                    if(hour == "0")
                    {
                        break;
                    }
                    hours.Add(hour);
                }

                int j = 3;
                for(; j <= i ; j++)
                {
                    string minute = minuteRow.ItemArray[j].ToString();
                    string action = actionRow.ItemArray[j].ToString();
                    minutes.Add(minute);
                    actions.Add(action);
                }

                IList<DayPlan> dayPlans = new List<DayPlan>();

                //Each dayplan
                for (int k = 0; k < hours.Count() - 1; k++)
                {
                    DateTime sdt = new DateTime(2000, 1, 1, Convert.ToInt32(hours[k]), Convert.ToInt32(minutes[k]), 0);
                    DateTime edt = new DateTime(2000, 1, 1, Convert.ToInt32(hours[k+1]), Convert.ToInt32(minutes[k+1]), 0);
                    int actionNo = Convert.ToInt32(actions[k]);
                    Schedule sche = new Schedule(sdt, edt);

                    DayPlan dp = new DayPlan();
                    dp.Schedule = sche;
                    dp.DayPlanActionId = actionNo;
                    dp.DayPlanName = hourRow.Table.Columns[k+2].ColumnName;

                    //get timing plan information
                    TimingPlan tp = new TimingPlan();
                    foreach(DataRow patternRow in patternsData)
                    {
                        if(patternRow.ItemArray[3].ToString() == (actionNo + 9).ToString()) // There are problems here
                        {
                            tp.CycleLength = Convert.ToInt32(patternRow.ItemArray[1].ToString()); // Cycle time
                            tp.SplitNumber = Convert.ToInt32(patternRow.ItemArray[3].ToString()); // Split number
                            tp.Offset = Convert.ToInt32(patternRow.ItemArray[2].ToString()); // Offset
                            tp.SequenceNumber = Convert.ToInt32(patternRow.ItemArray[4].ToString()); // Seq number
                            break;
                        }
                    }

                    //get split information
                    IList<DataRow> splitInfo = splitsExpandedData[actionNo];
                    Split sp = new Split();
                    DataRow timeRow = splitInfo[0];
                    DataRow modeRow = splitInfo[1];
                    DataRow coordinateRow = splitInfo[2];

                    IList<Phase> phases = new List<Phase>();
                    for(int l = 1; l <= 8; l++)
                    {
                        string labelString = String.Format("Split Expanded {0}", l);
                        bool coor = false;
                        if(coordinateRow[labelString].ToString() == "ON")
                        {
                            coor = true;
                        }

                        Phase p = new Phase(l , Convert.ToInt32(timeRow[labelString].ToString()), coor, modeRow[labelString].ToString());
                        phases.Add(p);
                    }

                    sp.phase1 = phases[1];
                    sp.phase1 = phases[2];
                    sp.phase1 = phases[3];
                    sp.phase1 = phases[4];
                    sp.phase1 = phases[5];
                    sp.phase1 = phases[6];
                    sp.phase1 = phases[7];
                    sp.phase1 = phases[8];

                    tp.split = sp;
                    dp.TimingPlan = tp;
                    dayPlans.Add(dp);
                }

                wholeWeekDic.Add(dayIndex, dayPlans);
            }

            return wholeWeekDic;
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
                    if((patternIndex >= 9 && patternIndex <=21) || (patternIndex>= 26 && patternIndex <= 30))
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
                            if((patternIndex >= 9 && patternIndex <= 21) || (patternIndex >= 26 && patternIndex <= 30))
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
