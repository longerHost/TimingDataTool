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

namespace TimingDataTool
{
    public partial class IntersectionForm : Form
    {
        private IIntersectionDataFormViewModel viewModel;

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
            intersectionGridView.DataSource = viewModel.displayTable;
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

        private void intersectionGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
