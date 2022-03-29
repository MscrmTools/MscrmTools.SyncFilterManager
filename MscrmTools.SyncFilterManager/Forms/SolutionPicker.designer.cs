namespace MscrmTools.SyncFilterManager.Forms
{
    partial class SolutionPicker
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
            this.btnSolutionPickerCancel = new System.Windows.Forms.Button();
            this.btnSolutionPickerValidate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lstSolutions = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSolutionPickerCancel
            // 
            this.btnSolutionPickerCancel.Location = new System.Drawing.Point(626, 11);
            this.btnSolutionPickerCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSolutionPickerCancel.Name = "btnSolutionPickerCancel";
            this.btnSolutionPickerCancel.Size = new System.Drawing.Size(112, 35);
            this.btnSolutionPickerCancel.TabIndex = 4;
            this.btnSolutionPickerCancel.Text = "Cancel";
            this.btnSolutionPickerCancel.UseVisualStyleBackColor = true;
            this.btnSolutionPickerCancel.Click += new System.EventHandler(this.btnSolutionPickerCancel_Click);
            // 
            // btnSolutionPickerValidate
            // 
            this.btnSolutionPickerValidate.Enabled = false;
            this.btnSolutionPickerValidate.Location = new System.Drawing.Point(502, 11);
            this.btnSolutionPickerValidate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSolutionPickerValidate.Name = "btnSolutionPickerValidate";
            this.btnSolutionPickerValidate.Size = new System.Drawing.Size(112, 35);
            this.btnSolutionPickerValidate.TabIndex = 3;
            this.btnSolutionPickerValidate.Text = "OK";
            this.btnSolutionPickerValidate.UseVisualStyleBackColor = true;
            this.btnSolutionPickerValidate.Click += new System.EventHandler(this.btnSolutionPickerValidate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSolutionPickerCancel);
            this.panel2.Controls.Add(this.btnSolutionPickerValidate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 415);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(730, 60);
            this.panel2.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(730, 92);
            this.panel1.TabIndex = 15;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.lblHeader.Location = new System.Drawing.Point(4, 9);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(644, 46);
            this.lblHeader.TabIndex = 11;
            this.lblHeader.Text = "Select solution that contains views to display";
            // 
            // lstSolutions
            // 
            this.lstSolutions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSolutions.Enabled = false;
            this.lstSolutions.FullRowSelect = true;
            this.lstSolutions.GridLines = true;
            this.lstSolutions.HideSelection = false;
            this.lstSolutions.Location = new System.Drawing.Point(0, 92);
            this.lstSolutions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstSolutions.Name = "lstSolutions";
            this.lstSolutions.Size = new System.Drawing.Size(730, 323);
            this.lstSolutions.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstSolutions.TabIndex = 16;
            this.lstSolutions.UseCompatibleStateImageBehavior = false;
            this.lstSolutions.View = System.Windows.Forms.View.Details;
            this.lstSolutions.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstSolutions_ColumnClick);
            this.lstSolutions.DoubleClick += new System.EventHandler(this.lstSolutions_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Display Name";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 125;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Publisher";
            this.columnHeader3.Width = 200;
            // 
            // SolutionPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(730, 475);
            this.ControlBox = false;
            this.Controls.Add(this.lstSolutions);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SolutionPicker";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.SolutionPicker_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSolutionPickerCancel;
        private System.Windows.Forms.Button btnSolutionPickerValidate;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ListView lstSolutions;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}