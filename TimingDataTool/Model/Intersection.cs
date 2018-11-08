using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model.DataModel
{
    public class Intersection
    {
        public int Id;
        public string Name;
        public string Config;

        public IntersectionPresetInfo presetInfo;
        public IDictionary<int, IList<DayPlan>> wholeWeeksDayPlan; //7 day plans Mon-Sun
    }
}
