using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model.DataModel
{
    public class TimingPlan
    {
        public Schedule TimingSchedule;
        public float CycleLength;
        public float Offset;
        public int SplitNumber;
        public int SequenceNumber;
        public IList<Phase> phases;
        public Split split;
    }
}
