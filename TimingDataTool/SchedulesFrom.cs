using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimingDataTool.Model;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public partial class SchedulesFrom : Form
    {
        private Intersection intersection;

        public SchedulesFrom(Intersection isc)
        {
            InitializeComponent();

            intersectionNameLabel.Text = isc.Name;
            intersection = isc;

            DataTable dt = new DataTable();
            IList<IList<DayPlan>> daysPlans = isc.wholeWeeksDayPlan.Values.ToList();

            dt.Columns.Add("Week days");
            dt.Columns.Add("Plan 1");
            dt.Columns.Add("Plan 2");
            dt.Columns.Add("Plan 3");
            dt.Columns.Add("Plan 4");
            dt.Columns.Add("Plan 5");
            dt.Columns.Add("Plan 6");
            dt.Columns.Add("Plan 7");
            dt.Columns.Add("Plan 8");

            int index = 1;
            //plan for each day
            foreach (IList<DayPlan> plans in daysPlans)
            {
                IList<string> displaySchedules = new List<string>();

                IList<string> startDateTimes = plans.Select(e => e.Schedule.StartTime.ToString()).ToList();
                IList<string> endDateTimes = plans.Select(e => e.Schedule.EndTime.ToString()).ToList();

                IList<string> startTimes = RemoveDateFromDateTimeStrings(startDateTimes);
                IList<string> endTimes = RemoveDateFromDateTimeStrings(endDateTimes);

                for (int i = startTimes.Count - 1; i < 8; i++)
                {
                    startTimes.Add("N/A");
                    endTimes.Add(" ");
                }

                displaySchedules.Add(index++.ToString());
                for(int j = 0; j < 8; j++)
                {
                    if(startTimes[j] != "N/A")
                    {
                        displaySchedules.Add(startTimes[j] + " - " + endTimes[j]);
                    }
                    else
                    {
                        displaySchedules.Add(startTimes[j]);
                    }
                }

                dt.Rows.Add(displaySchedules[0], displaySchedules[1], displaySchedules[2], displaySchedules[3], displaySchedules[4], displaySchedules[5], displaySchedules[6], displaySchedules[7], displaySchedules[8]);

            }

            PlansListGridView.DataSource = dt;
        }

        private IList<string> RemoveDateFromDateTimeStrings(IList<string> DateTimeStrings)
        {
            IList<string> timeStrings = new List<string>();
            foreach (string dateTimeStr in DateTimeStrings)
            {
                string startTimeStr = RemoveDateFromDateAndSecondsTimeString(dateTimeStr);
                timeStrings.Add(startTimeStr);
            }
            return timeStrings;
        }

        private string RemoveDateFromDateAndSecondsTimeString(string startStr)
        {
            string timeStr = "";
            if(startStr != null)
            {
                string[] dateItems = startStr.Split(' ');
                string time = dateItems[1];
                string[] timeItems = time.Split(':');
                time.Replace(":" + timeItems[2],"");

                timeStr = time + " " + dateItems[2];
            }

            return timeStr;
        }

        private void PlansListGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1 && PlansListGridView.CurrentCell.Value.ToString() != "N/A")
            {
                IList<DayPlan> dayPlans = intersection.wholeWeeksDayPlan[e.RowIndex + 1];
                DayPlan dp = dayPlans[e.ColumnIndex - 1];

                PlanDetailsForm pf = new PlanDetailsForm(intersection, dp);
                pf.ShowDialog();
            }
        }

        /*
        private void PlansListGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            PlansListGridView.Rows[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            PlansListGridView.Rows[1].HeaderCell.Value = @"Tus";
        }
        */
    }
}
