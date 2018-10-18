using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model.DataModel
{
    class TimingPlan
    {
        public Schedule TimingSchedule;
        public float CycleTime;
        public float Offset;
        public int SplitNumber;
        public int SequenceNumber;
    }
}
