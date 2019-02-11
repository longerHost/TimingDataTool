﻿namespace TimingDataTool
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
            this.label1 = new System.Windows.Forms.Label();
            this.PatternDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.PlansListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatternDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PlansListGridView
            // 
            this.PlansListGridView.AllowUserToAddRows = false;
            this.PlansListGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlansListGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PlansListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PlansListGridView.Location = new System.Drawing.Point(36, 57);
            this.PlansListGridView.Name = "PlansListGridView";
            this.PlansListGridView.Size = new System.Drawing.Size(1141, 175);
            this.PlansListGridView.TabIndex = 0;
            this.PlansListGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PlansListGridView_CellClick);
            this.PlansListGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PlansListGridView_CellContentClick);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Patterns";
            // 
            // PatternDataGridView
            // 
            this.PatternDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PatternDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PatternDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PatternDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9});
            this.PatternDataGridView.Location = new System.Drawing.Point(38, 302);
            this.PatternDataGridView.Name = "PatternDataGridView";
            this.PatternDataGridView.Size = new System.Drawing.Size(1138, 167);
            this.PatternDataGridView.TabIndex = 5;
            this.PatternDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "3";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "4";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "5";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "6";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "7";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "8";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "9";
            this.Column9.Name = "Column9";
            // 
            // SchedulesFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 516);
            this.Controls.Add(this.PatternDataGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.intersectionLabel);
            this.Controls.Add(this.intersectionNameLabel);
            this.Controls.Add(this.ScheduleLabel);
            this.Controls.Add(this.PlansListGridView);
            this.Name = "SchedulesFrom";
            this.Text = "Plan Schedules";
            ((System.ComponentModel.ISupportInitialize)(this.PlansListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PatternDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PlansListGridView;
        private System.Windows.Forms.Label ScheduleLabel;
        private System.Windows.Forms.Label intersectionNameLabel;
        private System.Windows.Forms.Label intersectionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView PatternDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
    }
}