using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool.Model.DataModel
{
    public class Phase
    {
        private int phaseId;
        private bool coordinatePhase;
        private string mode;
        private int length;

        //public information
        private int walk;
        private int pedClearance;
        private int yellowCtr;
        private int redCtr;

        private float Green;
        private float Yellow;
        private float Red;

        public int PhaseId
        {
            get => phaseId;
            set => phaseId = value;
        }

        public bool CoordinatePhase
        {
            get => coordinatePhase;
            set => coordinatePhase = value;
        }

        public string Mode
        {
            get => mode;
            set => mode = value;
        }

        public int Length
        {
            get => length;
            set => length = value;
        }

        public Phase(int id, int len, bool coordinate, string mode)
        {
            PhaseId = id;
            Length = len;
            CoordinatePhase = coordinate;
            Mode = mode;
        }
    }
}
