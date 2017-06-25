namespace gen_alg
{
    partial class Add_new_attr
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
            this.btn_ok = new System.Windows.Forms.Button();
            this.tb_name_attr = new System.Windows.Forms.TextBox();
            this.lbl_text = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(105, 99);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.Text = "ОК";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // tb_name_attr
            // 
            this.tb_name_attr.Location = new System.Drawing.Point(30, 57);
            this.tb_name_attr.Name = "tb_name_attr";
            this.tb_name_attr.Size = new System.Drawing.Size(237, 20);
            this.tb_name_attr.TabIndex = 1;
            // 
            // lbl_text
            // 
            this.lbl_text.AutoSize = true;
            this.lbl_text.Location = new System.Drawing.Point(84, 20);
            this.lbl_text.Name = "lbl_text";
            this.lbl_text.Size = new System.Drawing.Size(120, 13);
            this.lbl_text.TabIndex = 2;
            this.lbl_text.Text = "Введите имя атрибута";
            // 
            // Add_new_attr_or_value
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 135);
            this.Controls.Add(this.lbl_text);
            this.Controls.Add(this.tb_name_attr);
            this.Controls.Add(this.btn_ok);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Add_new_attr_or_value";
            this.Text = "Новый атрибут";
            this.Load += new System.EventHandler(this.Add_new_attr_or_value_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TextBox tb_name_attr;
        private System.Windows.Forms.Label lbl_text;
    }
}