using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model
{
    public static class ControllerInfoTool
    {
        public static string SequenceIndexToSequence(int seqIndex)
        {
            string seqString = String.Empty;
            if (seqIndex <= 16 && seqIndex >= 1)
            {
                IDictionary<int, string> seqMapDic = new Dictionary<int, string>();

                seqMapDic.Add(1, "1,2 | 3,4:5,6 | 7,8");
                seqMapDic.Add(2, "2,1 | 3,4:5,6 | 7,8");
                seqMapDic.Add(3, "1,2 | 4,3:5,6 | 7,8");
                seqMapDic.Add(4, "2,1 | 4,3:5,6 | 7,8");

                seqMapDic.Add(5, "1,2 | 3,4:6,5 | 7,8");
                seqMapDic.Add(6, "2,1 | 3,4:6,5 | 7,8");
                seqMapDic.Add(7, "1,2 | 4,3:6,5 | 7,8");
                seqMapDic.Add(8, "2,1 | 4,3:6,5 | 7,8");

                seqMapDic.Add(9, "1,2 | 3,4:5,6 | 8,7");
                seqMapDic.Add(10, "2,1 | 3,4:5,6 | 8,7");
                seqMapDic.Add(11, "1,2 | 4,3:5,6 | 8,7");
                seqMapDic.Add(12, "2,1 | 4,3:5,6 | 8,7");

                seqMapDic.Add(13, "1,2 | 3,4:6,5 | 8,7");
                seqMapDic.Add(14, "2,1 | 3,4:6,5 | 8,7");
                seqMapDic.Add(15, "1,2 | 4,3:6,5 | 8,7");
                seqMapDic.Add(16, "2,1 | 4,3:6,5 | 8,7");

                seqString = seqMapDic[seqIndex];
            }
            return seqString;
        }
    }
}
