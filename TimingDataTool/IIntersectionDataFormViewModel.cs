using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingDataTool
{
    public interface IIntersectionDataFormViewModel
    {
        DataTable displayTable { get; set; }

        /// <summary>
        /// Select mutiple excel files and load them to memory
        /// </summary>
        /// <param name="fileNames"></param>
        void ImportExcelFilesAndLoad(string[] fileNames);
    }
}
