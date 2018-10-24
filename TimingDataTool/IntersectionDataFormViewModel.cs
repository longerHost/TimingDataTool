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
        private Intersection intersection;

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
            displayTable = ds.Tables[0];
            IDictionary<string, string> headerDic = GetIntersectionInfo(ds.Tables[0]);
            intersection.Id = Convert.ToInt32(headerDic["ID"]);
            intersection.Name = headerDic["Name"];
            intersection.Config = headerDic["Configuration"];
            intersection.wholeWeeksDayPlan = GetWholeWeekDayPlan(ds);
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

        private IDictionary<int, IList<DayPlan>> GetWholeWeekDayPlan(DataSet ds)
        {
            DataTable dayPlanTable = ds.Tables[0];
            DataTable phaseTimesTable = ds.Tables[1];
            DataTable PatternsTable = ds.Tables[2];
            DataTable splitsExpandedTable = ds.Tables[3];

            IDictionary<int, List<DataRow>> dayPlanData = GetValidDayPlanTableData(dayPlanTable);
            IList<DataRow> phaseTimeOptionsData = GetValidPhaseTimeOptionsData(phaseTimesTable);
            IList<DataRow> patternsData = GetValidPatternsTableData(PatternsTable);
            IDictionary<int, List<DataRow>> splitsExpandedData = GetSplitsExpandedTableData(splitsExpandedTable);

        }

        private IDictionary<int, List<DataRow>> GetSplitsExpandedTableData(DataTable splitsExpandedTable)
        {
            throw new NotImplementedException();
        }

        private IList<DataRow> GetValidPatternsTableData(DataTable patternsTable)
        {
            throw new NotImplementedException();
        }

        private IList<DataRow> GetValidPhaseTimeOptionsData(DataTable phaseTimesTable)
        {
            throw new NotImplementedException();
        }

        private IDictionary<int, List<DataRow>> GetValidDayPlanTableData(DataTable dt)
        {
            List<DataRow> validDataRows = new List<DataRow>();
            List<List<DataRow>> dayRows = new List<List<DataRow>>();
            List<int> dayIndexs = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                int index = dt.Rows.IndexOf(row);
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
