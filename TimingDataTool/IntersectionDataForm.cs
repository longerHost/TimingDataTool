using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimingDataTool.Model.DataModel;

namespace TimingDataTool
{
    public partial class IntersectionForm : Form
    {
        private IIntersectionDataFormViewModel viewModel;

        private IList<Intersection> intersections;

        public IntersectionForm()
        {
            InitializeComponent();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            viewModel = new IntersectionDataFormViewModel();
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ChooseFilesAndImport();
                displayToDataGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void displayToDataGrid()
        {
            if(viewModel.Intersections == null)
            {
                throw new Exception("Please import valid data files");
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("ID");
                dt.Columns.Add("Configuration");

                foreach(Intersection isc in viewModel.Intersections)
                {
                    dt.Rows.Add(isc.Name, isc.Id, isc.Config);
                }

                intersectionGridView.DataSource = dt;
            }
        }

        private void ChooseFilesAndImport()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            fileDialog.Title = "Select Intersection Excels Files";
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                viewModel.ImportExcelFilesAndLoad(fileDialog.FileNames);
            }
        }

        private void intersectionGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                Intersection isc = viewModel.Intersections[e.RowIndex];
                SchedulesFrom pf = new SchedulesFrom(isc);
                pf.ShowDialog();
            }
        }

        private void ExprotBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                    sfd.Title = "Please name your file";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        viewModel.ExportDataToExcel(intersectionGridView, sfd.FileName);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
