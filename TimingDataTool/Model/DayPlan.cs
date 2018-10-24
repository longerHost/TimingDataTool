using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool.Model
{
    internal class DayPlan
    {
        public string DayPlanName;
        public int DayPlanActionId;
        public Schedule Schedule;
        public TimingPlan TimingPlan;
    }
}
