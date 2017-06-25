namespace gen_alg
{
    partial class Employee_change
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_change = new System.Windows.Forms.Button();
            this.btn_info = new System.Windows.Forms.Button();
            this.dgv_emp = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_emp)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btn_delete);
            this.panel1.Controls.Add(this.btn_change);
            this.panel1.Controls.Add(this.btn_info);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 322);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(657, 64);
            this.panel1.TabIndex = 2;
            // 
            // btn_delete
            // 
            this.btn_delete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_delete.Location = new System.Drawing.Point(484, 21);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(106, 31);
            this.btn_delete.TabIndex = 2;
            this.btn_delete.Text = "Удалить";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_change
            // 
            this.btn_change.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_change.Location = new System.Drawing.Point(270, 21);
            this.btn_change.Name = "btn_change";
            this.btn_change.Size = new System.Drawing.Size(106, 31);
            this.btn_change.TabIndex = 1;
            this.btn_change.Text = "Изменить";
            this.btn_change.UseVisualStyleBackColor = true;
            this.btn_change.Click += new System.EventHandler(this.btn_change_Click);
            // 
            // btn_info
            // 
            this.btn_info.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_info.Location = new System.Drawing.Point(69, 21);
            this.btn_info.Name = "btn_info";
            this.btn_info.Size = new System.Drawing.Size(106, 31);
            this.btn_info.TabIndex = 0;
            this.btn_info.Text = "Подробнее";
            this.btn_info.UseVisualStyleBackColor = true;
            this.btn_info.Click += new System.EventHandler(this.btn_info_Click);
            // 
            // dgv_emp
            // 
            this.dgv_emp.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_emp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_emp.Location = new System.Drawing.Point(0, 0);
            this.dgv_emp.Name = "dgv_emp";
            this.dgv_emp.Size = new System.Drawing.Size(657, 325);
            this.dgv_emp.TabIndex = 3;
            // 
            // Employee_change
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 386);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgv_emp);
            this.Name = "Employee_change";
            this.Text = "Редактирование сотрудников";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Employee_change_FormClosing);
            this.Load += new System.EventHandler(this.Employe_change_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_emp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_change;
        private System.Windows.Forms.Button btn_info;
        private System.Windows.Forms.DataGridView dgv_emp;
    }
}