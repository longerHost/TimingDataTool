using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TimingDataTool.Model;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public partial class PlanDetailsForm : Form
    {
        public PlanDetailsForm(Intersection intersection, DayPlan dp)
        {
            InitializeComponent();

            cycleValueLabel.Text = dp.TimingPlan.CycleLength.ToString();
            offsetValueLabel.Text = dp.TimingPlan.Offset.ToString();
            sequenceValueLabel.Text = dp.TimingPlan.SequenceNumber.ToString();
            scheduleStartLabel.Text = "From: " + dp.Schedule.StartTime.TimeOfDay.ToString();
            scheduleEndLabel.Text = "To: " + dp.Schedule.EndTime.TimeOfDay.ToString();
            patternNoValueLabel.Text = dp.DayPlanActionId.ToString();

            IList<string> pns = new List<string>(new string[] { "Phase 1", "Phase 2", "Phase 3", "Phase 4", "Phase 5", "Phase 6", "Phase 7", "Phase 8", "Phase 9", "Phase 10", "Phase 11", "Phase 12", "Phase 13", "Phase 14", "Phase 15", "Phase 16" });

            //Add columns
            DataTable dt = new DataTable();
            dt.Columns.Add("Data Type");
            foreach(string p in pns)
            {
                dt.Columns.Add(p);
            }

            Split s = dp.TimingPlan.split;
            IDictionary<string, double> wp = intersection.presetInfo.WalkInfo;
            IDictionary<string, double> pp = intersection.presetInfo.PedClearance;
            IDictionary<string, double> yp = intersection.presetInfo.YellowCtl;
            IDictionary<string, double> rp = intersection.presetInfo.RedCtr;

            //Add Rows
            dt.Rows.Add("Split", s.phase1.Length, s.phase2.Length, s.phase3.Length, s.phase4.Length, s.phase5.Length, s.phase6.Length, s.phase7.Length, s.phase8.Length, s.phase9.Length, s.phase10.Length, s.phase11.Length, s.phase12.Length, s.phase13.Length, s.phase14.Length, s.phase15.Length, s.phase16.Length);
            dt.Rows.Add("Walk", wp[pns[0]], wp[pns[1]], wp[pns[2]], wp[pns[3]], wp[pns[4]], wp[pns[5]], wp[pns[6]], wp[pns[7]], wp[pns[8]], wp[pns[9]], wp[pns[10]], wp[pns[11]], wp[pns[12]], wp[pns[13]], wp[pns[14]], wp[pns[15]]);
            dt.Rows.Add("Ped Clear", pp[pns[0]], pp[pns[1]], pp[pns[2]], pp[pns[3]], pp[pns[4]], pp[pns[5]], pp[pns[6]], pp[pns[7]], pp[pns[8]], pp[pns[9]], pp[pns[10]], pp[pns[11]], pp[pns[12]], pp[pns[13]], pp[pns[14]], pp[pns[15]]);
            dt.Rows.Add("Yellow Ctrl", yp[pns[0]], yp[pns[1]], yp[pns[2]], yp[pns[3]], yp[pns[4]], yp[pns[5]], yp[pns[6]], yp[pns[7]], yp[pns[8]], yp[pns[9]], yp[pns[10]], yp[pns[11]], yp[pns[12]], yp[pns[13]], yp[pns[14]], yp[pns[15]]);
            dt.Rows.Add("Red Ctrl", rp[pns[0]], rp[pns[1]], rp[pns[2]], rp[pns[3]], rp[pns[4]], rp[pns[5]], rp[pns[6]], rp[pns[7]], rp[pns[8]], rp[pns[9]], rp[pns[10]], rp[pns[11]], rp[pns[12]], rp[pns[13]], rp[pns[14]], rp[pns[15]]);

            dt.Columns["Phase 1"].ColumnName = "Phase 1";

            planDetailsDataGridView.DataSource = dt;
        }

        private void PlanDetailsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
