using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool
{
    public class IntersectionDataFormViewModel : IIntersectionDataFormViewModel
    {
        private IList<DataSet> filesDataSet;

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
                FillControllerTables(ds);
            }
        }

        private void FillControllerTables(DataSet ds)
        {
            DataTable dayPlanTable;
            DataTable phaseTimesTable;
            DataTable PatternsTable;
            DataTable splitsExpandedTable;

            dayPlanTable = ds.Tables[0];
            phaseTimesTable = ds.Tables[1];
            PatternsTable = ds.Tables[2];
            splitsExpandedTable = ds.Tables[3];
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
