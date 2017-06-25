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
    public partial class Employee_change : Form
    {
        string work_file_name;
        DialogResult res = DialogResult.No; //значения диалога формы

        List<string> requ_attr; //массив обязательных параметров
        Dictionary<KeyValuePair<string, int>, string> attr; //массив дополнительных аттрибутов


        public Employee_change()
        {
            InitializeComponent();
        }

        public Employee_change(string work_file_name)
        {
            this.work_file_name = work_file_name;

            InitializeComponent();
        }

        private void Employe_change_Load(object sender, EventArgs e) //загрузка окна
        {
            //задание параметров таблицы отображения
            if (Form1.employees != null)
            {
                if (Form1.employees.Count > 0)
                {
                    dgv_emp.ColumnCount = 4;

                    dgv_emp.Columns[0].Width = 0;
                    dgv_emp.Columns[0].Visible = false;
                    dgv_emp.Columns[1].Width = dgv_emp.Width / 2;
                    dgv_emp.Columns[2].Width = dgv_emp.Width / 4;
                    dgv_emp.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv_emp.Columns[1].Name = "ФИО";
                    dgv_emp.Columns[2].Name = "Возраст";
                    dgv_emp.Columns[3].Name = "Опыт работы";

                    dgv_emp.RowHeadersVisible = false;
                    dgv_emp.ColumnHeadersVisible = true;
                    dgv_emp.AllowUserToAddRows = false;
                    dgv_emp.AllowUserToDeleteRows = false;
                    dgv_emp.AllowUserToResizeColumns = false;
                    dgv_emp.AllowUserToResizeRows = false;
                    dgv_emp.ReadOnly = true;
                    dgv_emp.Font = new Font("Modern No. 20", 12, FontStyle.Regular);

                    dgv_View();
                }
                else
                {
                    MessageBox.Show("Нет ни одного сотрудника");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Нет ни одного сотрудника");
                this.Close();
            }
        }



        private void dgv_View() //функция для отображения названий проектов в таблицу
        {         


            while (dgv_emp.Rows.Count > 0)
            {
                dgv_emp.Rows.RemoveAt(0);
            }

            foreach (Employee empl in Form1.employees)
            {
                dgv_emp.Rows.Add(empl.ID, empl.Name, empl.Age, empl.Experience);
            }
        }

        private void btn_info_Click(object sender, EventArgs e) //нажата кнопка Подробнее
        {
            //открытие окна с информацией о выбранном проекте
            object info_obj;
            
                info_obj = Form1.employees.ElementAt(dgv_emp.CurrentCell.RowIndex);
            

            More inf_form = new More(info_obj);
            inf_form.Show();
        }

        private void btn_change_Click(object sender, EventArgs e) //нажата кнопка Изменить
        {
            //открытие окна добавления/изменения проекта и передача параметров выбранного сотрудника
            Employe_add form = new Employe_add(work_file_name, Int32.Parse(dgv_emp[0, dgv_emp.CurrentRow.Index].Value.ToString()));//, id); выбранного элемента
            if (form.ShowDialog() == DialogResult.OK)
            {
                //изменений значений атрибутов, если диалог формы равен ОК
                requ_attr = form.Get_requ_attr();
                attr = form.Get_attrs();

                dgv_emp[1, dgv_emp.CurrentRow.Index].Value = requ_attr.ElementAt(0);
                dgv_emp[2, dgv_emp.CurrentRow.Index].Value = requ_attr.ElementAt(1);
                dgv_emp[3, dgv_emp.CurrentRow.Index].Value = requ_attr.ElementAt(2);

                Form1.Add_or_change_empl_array(requ_attr, attr, form.Get_id());

                res = DialogResult.OK;
            }
        }

        private void btn_delete_Click(object sender, EventArgs e) //нажата кнопка Удалить
        {
            //окно с подтверждением удаления
            DialogResult rsl = MessageBox.Show("Вы уверены, что хотите удалить информацию о сотруднике?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rsl == DialogResult.Yes)
            {
                //если удаление подтверждено, то удаление выбранного сотрудника из массива
                int remove_id = Int32.Parse(dgv_emp[0, dgv_emp.CurrentRow.Index].Value.ToString());

                
                foreach (Employee emp in Form1.employees)
                {
                    if (emp.ID == remove_id)
                    {
                        Form1.employees.Remove(emp);
                        break;
                    }

                }

                dgv_View();

                res = DialogResult.Abort;
            }
        }

        private void Employee_change_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = res;
        }
    }
}
