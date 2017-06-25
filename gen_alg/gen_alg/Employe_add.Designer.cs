namespace gen_alg
{
    partial class Employe_add
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
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_del_str = new System.Windows.Forms.Button();
            this.btn_add_str = new System.Windows.Forms.Button();
            this.dgv_requ_attr = new System.Windows.Forms.DataGridView();
            this.panel_attr = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_requ_attr)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btn_cancel);
            this.panel1.Controls.Add(this.btn_add);
            this.panel1.Controls.Add(this.btn_del_str);
            this.panel1.Controls.Add(this.btn_add_str);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(494, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(108, 368);
            this.panel1.TabIndex = 3;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(7, 259);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(89, 23);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "Отмена";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(7, 209);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(89, 23);
            this.btn_add.TabIndex = 4;
            this.btn_add.Text = "Добавить";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_del_str
            // 
            this.btn_del_str.Location = new System.Drawing.Point(69, 108);
            this.btn_del_str.Name = "btn_del_str";
            this.btn_del_str.Size = new System.Drawing.Size(27, 25);
            this.btn_del_str.TabIndex = 1;
            this.btn_del_str.Text = "-";
            this.btn_del_str.UseVisualStyleBackColor = true;
            this.btn_del_str.Click += new System.EventHandler(this.btn_del_str_Click);
            // 
            // btn_add_str
            // 
            this.btn_add_str.Location = new System.Drawing.Point(7, 108);
            this.btn_add_str.Name = "btn_add_str";
            this.btn_add_str.Size = new System.Drawing.Size(27, 25);
            this.btn_add_str.TabIndex = 0;
            this.btn_add_str.Text = "+";
            this.btn_add_str.UseVisualStyleBackColor = true;
            this.btn_add_str.Click += new System.EventHandler(this.btn_add_str_Click);
            // 
            // dgv_requ_attr
            // 
            this.dgv_requ_attr.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_requ_attr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_requ_attr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_requ_attr.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_requ_attr.Location = new System.Drawing.Point(0, 0);
            this.dgv_requ_attr.Name = "dgv_requ_attr";
            this.dgv_requ_attr.Size = new System.Drawing.Size(495, 98);
            this.dgv_requ_attr.TabIndex = 4;
            // 
            // panel_attr
            // 
            this.panel_attr.Location = new System.Drawing.Point(0, 92);
            this.panel_attr.Name = "panel_attr";
            this.panel_attr.Size = new System.Drawing.Size(495, 276);
            this.panel_attr.TabIndex = 5;
            // 
            // Employe_add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 368);
            this.Controls.Add(this.panel_attr);
            this.Controls.Add(this.dgv_requ_attr);
            this.Controls.Add(this.panel1);
            this.Name = "Employe_add";
            this.Text = "Добавить сотрудника";
            this.Load += new System.EventHandler(this.Employe_add_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_requ_attr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_del_str;
        private System.Windows.Forms.Button btn_add_str;
        private System.Windows.Forms.DataGridView dgv_requ_attr;
        private System.Windows.Forms.Panel panel_attr;
    }
}