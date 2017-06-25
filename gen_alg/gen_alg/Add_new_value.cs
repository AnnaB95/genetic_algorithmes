using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gen_alg
{
    public partial class Add_new_value : Form
    {
        string attr_name;   //имя атрибута
        Dictionary<int, string> attr_values; //массив ссответствий: относительное положение значения - значение атрибута


        public Add_new_value()
        {
            InitializeComponent();
        }

        public Add_new_value(string attr, Dictionary<int, string> attr_values)
        {
            InitializeComponent();

            this.attr_name = attr;
            this.attr_values = attr_values;
        }

        private void Add_new_value_Load(object sender, EventArgs e) //загрузка окна
        {
            this.CenterToParent(); //размещение в центре родительской формы
            this.Text = "Добавить значение атрибута:" + attr_name; //текстовое значение на форме

            //задание параметров таблицы значений атрибута
            dgv_values.ColumnCount = 1; 
           
            dgv_values.Columns[0].Name = "Значение";
            dgv_values.RowHeadersWidth = dgv_values.Width / 5;
            dgv_values.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_values.ReadOnly = true;
            dgv_values.Font = new Font("Modern No. 20", 12, FontStyle.Regular);

            //заполнение таблицы значений атрибута
            for (int i = 0; i < attr_values.Count; i++)
            {
                dgv_values.Rows.Add();
                dgv_values.Rows[i].HeaderCell.Value = (i+1).ToString();
            }
            foreach (KeyValuePair<int, string> val in attr_values)
            {
                dgv_values[0, val.Key-1].Value = val.Value;
            }      
        }

        private void btn_ok_Click(object sender, EventArgs e) //нажата кнопка ОК
        {
            attr_values.Clear(); //очистка массива значений атрибута

            //заполнение массива значений атрибута
            for (int i = 0; i < dgv_values.Rows.Count; i++)
            {
                attr_values.Add(i + 1, dgv_values[0, i].Value.ToString().Trim());
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_add_Click(object sender, EventArgs e) //нажата кнопка Добавить
        {
            //если в текстовое поле занесена информация, то она заносится в таблицу значений атрибута
            if (tb_val.Text.Trim() != "")
            {
                dgv_values.Rows.Add(tb_val.Text.Trim());
                dgv_values.Rows[dgv_values.Rows.Count - 1].HeaderCell.Value = dgv_values.Rows.Count.ToString();
                tb_val.Text = "";
            }
        }

        private void btn_del_Click(object sender, EventArgs e) //нажата кнопка Удалить
        {

            if (dgv_values.Rows.Count > 0)
            {
                //окно для подтверждения удаления
                DialogResult rsl = MessageBox.Show("Вы уверены, что хотите удалить значение атрибута: " + dgv_values.CurrentCell.Value.ToString() +"?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rsl == DialogResult.Yes)
                {
                    //при подтверждении - удаление выбранного значения атрибута
                    dgv_values.Rows.RemoveAt(dgv_values.CurrentRow.Index);
                }
            }
        }

        private void btn_up_str_Click(object sender, EventArgs e) //нажата кнопка UP
        {
            Up_Down("up");
        }

        private void Up_Down(string action) //функция для перемещения выбранного значения в списке значений атрибута
        {
            //для перемещения вверх элемент должен быть не верхним, а для перемещения вниз - не последним
            //при соблюдении этих условий производится перемещение

            if (dgv_values.Rows.Count > 1)
            {
                int pos;
                if (action == "up") pos = 0;
                else if (action == "down") pos = dgv_values.Rows.Count - 1;
                else return;

                if (dgv_values.CurrentRow.Index != pos)
                {
                    string str = dgv_values.CurrentCell.Value.ToString().Trim();
                    int row = dgv_values.CurrentRow.Index;
                    dgv_values.Rows.RemoveAt(row);
                  
                    if (action == "up")
                    {
                        dgv_values.Rows.Insert(row - 1, str);
                        dgv_values.Rows[row - 1].HeaderCell.Value = (row).ToString();
                    }

                    else
                     if (action == "down")
                    {
                        dgv_values.Rows.Insert(row + 1, str);
                        dgv_values.Rows[row + 1].HeaderCell.Value = (row + 2).ToString();
                    }
                    else
                        dgv_values.Rows.Insert(row, str);

                    dgv_values.Rows[row].HeaderCell.Value = (row + 1).ToString();
                }
            }
        }

        private void btn_down_str_Click(object sender, EventArgs e) //нажата кнопка DOWN
        {
            Up_Down("down");
        }

        public Dictionary<int, string> Get_attr_values()
        {
            return attr_values;
        }
    }
}
