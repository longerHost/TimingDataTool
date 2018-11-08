namespace TimingDataTool
{
    partial class IntersectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.importBtn = new System.Windows.Forms.Button();
            this.intersectionGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.intersectionGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // importBtn
            // 
            this.importBtn.Location = new System.Drawing.Point(12, 12);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(75, 23);
            this.importBtn.TabIndex = 0;
            this.importBtn.Text = "import";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // intersectionGridView
            // 
            this.intersectionGridView.AllowUserToAddRows = false;
            this.intersectionGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.intersectionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.intersectionGridView.Location = new System.Drawing.Point(12, 41);
            this.intersectionGridView.Name = "intersectionGridView";
            this.intersectionGridView.Size = new System.Drawing.Size(711, 434);
            this.intersectionGridView.TabIndex = 1;
            this.intersectionGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.intersectionGridView_CellClick);
            // 
            // IntersectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 493);
            this.Controls.Add(this.intersectionGridView);
            this.Controls.Add(this.importBtn);
            this.Name = "IntersectionForm";
            this.Text = "Intersections list";
            ((System.ComponentModel.ISupportInitialize)(this.intersectionGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.DataGridView intersectionGridView;
    }
}

