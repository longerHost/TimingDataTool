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
            intersection = isc;
            intersectionNameLabel.Text = isc.Name;

            // Schedule DataGridView
            DataTable sdt = getSheduleTableWithIntersection(isc);
            PlansListGridView.DataSource = sdt;

            DataTable pdt = getPatternsTableFromIntersection(isc);
            PatternDataGridView.DataSource = pdt;

        }

        private DataTable getPatternsTableFromIntersection(Intersection isc)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");
            dt.Columns.Add("6");
            dt.Columns.Add("7");
            dt.Columns.Add("8");
            dt.Columns.Add("9");

            IList<TimingPlan> timingPlans = isc.AllTimings;
            int columnNo = 9; // 9 columns in total
            double Uppper = Math.Ceiling((double)timingPlans.Count / columnNo);
            IList<string> indexs = new List<string>();

            for(int i = 1; i <= Uppper * columnNo; i++)
            {
                if(i <= timingPlans.Count)
                {
                    indexs.Add("Pattern " + i);
                }
                else
                {
                    indexs.Add("N/A");
                }
            }
            
            for (int j = 0; j < Uppper; j++)
            {
                dt.Rows.Add(indexs[0 + j*9], indexs[1 + j * 9], indexs[2 + j * 9], indexs[3 + j * 9], indexs[4 + j * 9], indexs[5 + j * 9], indexs[6 + j * 9], indexs[7 + j * 9], indexs[8 + j * 9]);
            }

            return dt;
        }
        

        public DataTable getSheduleTableWithIntersection(Intersection isc)
        {
            DataTable dt = new DataTable();
            IList<IList<DayPlan>> daysPlans = isc.WholeWeeksDayPlan.Values.ToList();

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
                IList<DateTime> startDateTimes = plans.Select(e => e.Schedule.StartTime).ToList();
                IList<DateTime> endDateTimes = plans.Select(e => e.Schedule.EndTime).ToList();
                IList<string> startTimes = GetDisplayString(startDateTimes);
                IList<string> endTimes = GetDisplayString(endDateTimes);
                IList<int> actions = plans.Select(e => e.DayPlanActionId).ToList();

                for (int i = startTimes.Count - 1; i < 8; i++)
                {
                    startTimes.Add("N/A");
                    endTimes.Add(" ");
                }

                displaySchedules.Add(index++.ToString());
                for (int j = 0; j < 8; j++)
                {
                    if (startTimes[j] != "N/A")
                    {
                        displaySchedules.Add(startTimes[j] + " - " + endTimes[j] + " (" + actions[j] + ")");
                    }
                    else
                    {
                        displaySchedules.Add(startTimes[j]);
                    }
                }
                dt.Rows.Add(displaySchedules[0], displaySchedules[1], displaySchedules[2], displaySchedules[3], displaySchedules[4], displaySchedules[5], displaySchedules[6], displaySchedules[7], displaySchedules[8]);
            }
            return dt;
        }

        private IList<string> GetDisplayString(IList<DateTime> DateTimes)
        {
            IList<string> timeStrings = new List<string>();
            foreach (DateTime dateTime in DateTimes)
            {
                string timeStr = dateTime.ToString("HH:mm");
                timeStrings.Add(timeStr);
            }
            return timeStrings;
        }

        private void PlansListGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1 && PlansListGridView.CurrentCell.Value.ToString() != "N/A" && e.ColumnIndex != 0)
            {
                IList<DayPlan> dayPlans = intersection.WholeWeeksDayPlan[e.RowIndex + 1];
                DayPlan dp = dayPlans[e.ColumnIndex - 1];

                PlanDetailsForm pf = new PlanDetailsForm(intersection, dp);
                pf.ShowDialog();
            }
        }

        private void PatternDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && PatternDataGridView.CurrentCell.Value.ToString() != "N/A")
            {
                //IList<DayPlan> dayPlans = intersection.WholeWeeksDayPlan[e.RowIndex + 1];
                //DayPlan dp = dayPlans[e.ColumnIndex];

                DayPlan dp = new DayPlan();
                dp.DayPlanName = PatternDataGridView.CurrentCell.Value.ToString();
                dp.DayPlanActionId = e.RowIndex * 9 + e.ColumnIndex + 1;
                //Schedule sc = new Schedule(DateTime.Now, DateTime.Now);
                //sdp.Schedule = sc;
                TimingPlan tp = intersection.AllTimings[e.RowIndex * 9 + e.ColumnIndex];
                dp.TimingPlan = tp;
                
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
