﻿namespace TimingDataTool
{
    partial class PlanDetailsForm
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
            this.planDetailsDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.PlanCycleLabel = new System.Windows.Forms.Label();
            this.planOffsetLabel = new System.Windows.Forms.Label();
            this.cycleValueLabel = new System.Windows.Forms.Label();
            this.offsetValueLabel = new System.Windows.Forms.Label();
            this.SequenceNoLabel = new System.Windows.Forms.Label();
            this.sequenceValueLabel = new System.Windows.Forms.Label();
            this.scheduleStartLabel = new System.Windows.Forms.Label();
            this.scheduleEndLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.planDetailsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // planDetailsDataGridView
            // 
            this.planDetailsDataGridView.AllowUserToAddRows = false;
            this.planDetailsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.planDetailsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.planDetailsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.planDetailsDataGridView.Location = new System.Drawing.Point(35, 75);
            this.planDetailsDataGridView.Name = "planDetailsDataGridView";
            this.planDetailsDataGridView.Size = new System.Drawing.Size(805, 404);
            this.planDetailsDataGridView.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // PlanCycleLabel
            // 
            this.PlanCycleLabel.AutoSize = true;
            this.PlanCycleLabel.Location = new System.Drawing.Point(369, 17);
            this.PlanCycleLabel.Name = "PlanCycleLabel";
            this.PlanCycleLabel.Size = new System.Drawing.Size(86, 13);
            this.PlanCycleLabel.TabIndex = 2;
            this.PlanCycleLabel.Text = "Plan Cycle Time:";
            // 
            // planOffsetLabel
            // 
            this.planOffsetLabel.AutoSize = true;
            this.planOffsetLabel.Location = new System.Drawing.Point(369, 40);
            this.planOffsetLabel.Name = "planOffsetLabel";
            this.planOffsetLabel.Size = new System.Drawing.Size(88, 13);
            this.planOffsetLabel.TabIndex = 3;
            this.planOffsetLabel.Text = "Plan Offset Time:";
            // 
            // cycleValueLabel
            // 
            this.cycleValueLabel.AutoSize = true;
            this.cycleValueLabel.Location = new System.Drawing.Point(461, 17);
            this.cycleValueLabel.Name = "cycleValueLabel";
            this.cycleValueLabel.Size = new System.Drawing.Size(60, 13);
            this.cycleValueLabel.TabIndex = 4;
            this.cycleValueLabel.Text = "CycleValue";
            // 
            // offsetValueLabel
            // 
            this.offsetValueLabel.AutoSize = true;
            this.offsetValueLabel.Location = new System.Drawing.Point(461, 40);
            this.offsetValueLabel.Name = "offsetValueLabel";
            this.offsetValueLabel.Size = new System.Drawing.Size(62, 13);
            this.offsetValueLabel.TabIndex = 5;
            this.offsetValueLabel.Text = "OffsetValue";
            // 
            // SequenceNoLabel
            // 
            this.SequenceNoLabel.AutoSize = true;
            this.SequenceNoLabel.Location = new System.Drawing.Point(640, 17);
            this.SequenceNoLabel.Name = "SequenceNoLabel";
            this.SequenceNoLabel.Size = new System.Drawing.Size(99, 13);
            this.SequenceNoLabel.TabIndex = 6;
            this.SequenceNoLabel.Text = "Sequence Number:";
            // 
            // sequenceValueLabel
            // 
            this.sequenceValueLabel.AutoSize = true;
            this.sequenceValueLabel.Location = new System.Drawing.Point(736, 17);
            this.sequenceValueLabel.Name = "sequenceValueLabel";
            this.sequenceValueLabel.Size = new System.Drawing.Size(83, 13);
            this.sequenceValueLabel.TabIndex = 7;
            this.sequenceValueLabel.Text = "SequenceValue";
            // 
            // scheduleStartLabel
            // 
            this.scheduleStartLabel.AutoSize = true;
            this.scheduleStartLabel.Location = new System.Drawing.Point(38, 17);
            this.scheduleStartLabel.Name = "scheduleStartLabel";
            this.scheduleStartLabel.Size = new System.Drawing.Size(50, 13);
            this.scheduleStartLabel.TabIndex = 9;
            this.scheduleStartLabel.Text = "schedule";
            // 
            // scheduleEndLabel
            // 
            this.scheduleEndLabel.AutoSize = true;
            this.scheduleEndLabel.Location = new System.Drawing.Point(38, 40);
            this.scheduleEndLabel.Name = "scheduleEndLabel";
            this.scheduleEndLabel.Size = new System.Drawing.Size(50, 13);
            this.scheduleEndLabel.TabIndex = 10;
            this.scheduleEndLabel.Text = "schedule";
            // 
            // PlanDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 506);
            this.Controls.Add(this.scheduleEndLabel);
            this.Controls.Add(this.scheduleStartLabel);
            this.Controls.Add(this.sequenceValueLabel);
            this.Controls.Add(this.SequenceNoLabel);
            this.Controls.Add(this.offsetValueLabel);
            this.Controls.Add(this.cycleValueLabel);
            this.Controls.Add(this.planOffsetLabel);
            this.Controls.Add(this.PlanCycleLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.planDetailsDataGridView);
            this.Name = "PlanDetailsForm";
            this.Text = "Plan Details";
            this.Load += new System.EventHandler(this.PlanDetailsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.planDetailsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView planDetailsDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label PlanCycleLabel;
        private System.Windows.Forms.Label planOffsetLabel;
        private System.Windows.Forms.Label cycleValueLabel;
        private System.Windows.Forms.Label offsetValueLabel;
        private System.Windows.Forms.Label SequenceNoLabel;
        private System.Windows.Forms.Label sequenceValueLabel;
        private System.Windows.Forms.Label scheduleStartLabel;
        private System.Windows.Forms.Label scheduleEndLabel;
    }
}