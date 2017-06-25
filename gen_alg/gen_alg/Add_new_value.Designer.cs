namespace gen_alg
{
    partial class Add_new_value
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
            this.tb_val = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_values = new System.Windows.Forms.DataGridView();
            this.btn_up_str = new System.Windows.Forms.Button();
            this.btn_down_str = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_del = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_values)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_val
            // 
            this.tb_val.Location = new System.Drawing.Point(28, 48);
            this.tb_val.Name = "tb_val";
            this.tb_val.Size = new System.Drawing.Size(324, 20);
            this.tb_val.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Введите значение атрибута";
            // 
            // dgv_values
            // 
            this.dgv_values.AllowUserToAddRows = false;
            this.dgv_values.AllowUserToDeleteRows = false;
            this.dgv_values.AllowUserToResizeColumns = false;
            this.dgv_values.AllowUserToResizeRows = false;
            this.dgv_values.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgv_values.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_values.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_values.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_values.GridColor = System.Drawing.SystemColors.ButtonFace;
            this.dgv_values.Location = new System.Drawing.Point(28, 89);
            this.dgv_values.MultiSelect = false;
            this.dgv_values.Name = "dgv_values";
            this.dgv_values.ReadOnly = true;
            this.dgv_values.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_values.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_values.Size = new System.Drawing.Size(224, 147);
            this.dgv_values.TabIndex = 2;
            // 
            // btn_up_str
            // 
            this.btn_up_str.Location = new System.Drawing.Point(308, 89);
            this.btn_up_str.Name = "btn_up_str";
            this.btn_up_str.Size = new System.Drawing.Size(27, 25);
            this.btn_up_str.TabIndex = 5;
            this.btn_up_str.Text = "up";
            this.btn_up_str.UseVisualStyleBackColor = true;
            this.btn_up_str.Click += new System.EventHandler(this.btn_up_str_Click);
            // 
            // btn_down_str
            // 
            this.btn_down_str.Location = new System.Drawing.Point(291, 120);
            this.btn_down_str.Name = "btn_down_str";
            this.btn_down_str.Size = new System.Drawing.Size(61, 25);
            this.btn_down_str.TabIndex = 6;
            this.btn_down_str.Text = "down";
            this.btn_down_str.UseVisualStyleBackColor = true;
            this.btn_down_str.Click += new System.EventHandler(this.btn_down_str_Click);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(281, 178);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(89, 23);
            this.btn_add.TabIndex = 7;
            this.btn_add.Text = "Добавить";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(141, 261);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(89, 23);
            this.btn_ok.TabIndex = 8;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_del
            // 
            this.btn_del.Location = new System.Drawing.Point(281, 213);
            this.btn_del.Name = "btn_del";
            this.btn_del.Size = new System.Drawing.Size(89, 23);
            this.btn_del.TabIndex = 9;
            this.btn_del.Text = "Удалить";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // Add_new_value
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 296);
            this.Controls.Add(this.btn_del);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_down_str);
            this.Controls.Add(this.btn_up_str);
            this.Controls.Add(this.dgv_values);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_val);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Add_new_value";
            this.Text = "Добавить значение атрибута";
            this.Load += new System.EventHandler(this.Add_new_value_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_values)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_val;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_values;
        private System.Windows.Forms.Button btn_up_str;
        private System.Windows.Forms.Button btn_down_str;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_del;
    }
}