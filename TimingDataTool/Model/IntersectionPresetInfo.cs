using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model
{
    public class IntersectionPresetInfo
    {
        public IDictionary<string, double> WalkInfo;
        public IDictionary<string, double> PedClearance;
        public IDictionary<string, double> YellowCtl;
        public IDictionary<string, double> RedCtr;
    }
}
