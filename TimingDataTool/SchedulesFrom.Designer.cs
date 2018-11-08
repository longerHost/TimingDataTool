namespace TimingDataTool
{
    partial class SchedulesFrom
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
            this.PlansListGridView = new System.Windows.Forms.DataGridView();
            this.ScheduleLabel = new System.Windows.Forms.Label();
            this.intersectionNameLabel = new System.Windows.Forms.Label();
            this.intersectionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PlansListGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PlansListGridView
            // 
            this.PlansListGridView.AllowUserToAddRows = false;
            this.PlansListGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlansListGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PlansListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PlansListGridView.Location = new System.Drawing.Point(36, 57);
            this.PlansListGridView.Name = "PlansListGridView";
            this.PlansListGridView.Size = new System.Drawing.Size(1325, 415);
            this.PlansListGridView.TabIndex = 0;
            this.PlansListGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PlansListGridView_CellClick);
            // 
            // ScheduleLabel
            // 
            this.ScheduleLabel.AutoSize = true;
            this.ScheduleLabel.Location = new System.Drawing.Point(36, 27);
            this.ScheduleLabel.Name = "ScheduleLabel";
            this.ScheduleLabel.Size = new System.Drawing.Size(0, 13);
            this.ScheduleLabel.TabIndex = 1;
            // 
            // intersectionNameLabel
            // 
            this.intersectionNameLabel.AutoSize = true;
            this.intersectionNameLabel.Location = new System.Drawing.Point(106, 28);
            this.intersectionNameLabel.Name = "intersectionNameLabel";
            this.intersectionNameLabel.Size = new System.Drawing.Size(62, 13);
            this.intersectionNameLabel.TabIndex = 2;
            this.intersectionNameLabel.Text = "Intersection";
            // 
            // intersectionLabel
            // 
            this.intersectionLabel.AutoSize = true;
            this.intersectionLabel.Location = new System.Drawing.Point(43, 27);
            this.intersectionLabel.Name = "intersectionLabel";
            this.intersectionLabel.Size = new System.Drawing.Size(65, 13);
            this.intersectionLabel.TabIndex = 3;
            this.intersectionLabel.Text = "Intersection:";
            // 
            // SchedulesFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 498);
            this.Controls.Add(this.intersectionLabel);
            this.Controls.Add(this.intersectionNameLabel);
            this.Controls.Add(this.ScheduleLabel);
            this.Controls.Add(this.PlansListGridView);
            this.Name = "SchedulesFrom";
            this.Text = "Plan Schedules";
            ((System.ComponentModel.ISupportInitialize)(this.PlansListGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PlansListGridView;
        private System.Windows.Forms.Label ScheduleLabel;
        private System.Windows.Forms.Label intersectionNameLabel;
        private System.Windows.Forms.Label intersectionLabel;
    }
}