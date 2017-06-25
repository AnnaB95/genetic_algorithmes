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
    public partial class Project_change : Form
    {
        string work_file_name; 

        List<string> requ_attr; //массив обязательных параметров
        Dictionary<KeyValuePair<string, int>, string> attr; //массив дополнительных аттрибутов

        DialogResult res = DialogResult.No; //значения диалога формы

        public Project_change()
        {
            InitializeComponent();
        }

        public Project_change(string work_file_name)    
        {
            this.work_file_name = work_file_name;

            InitializeComponent();
        }

        private void btn_info_Click(object sender, EventArgs e) //нажата кнопка Подробнее
        {
            //открытие окна с информацией о выбранном проекте
            object info_obj;

            info_obj = Form1.projects.ElementAt(dgv_project.CurrentCell.RowIndex);


            More inf_form = new More(info_obj);
            inf_form.Show();
        }

        private void btn_change_Click(object sender, EventArgs e) //нажата кнопка Изменить
        {
            //открытие окна добавления/изменения проекта и передача параметров выбранного проекта
            Project_add form = new Project_add(work_file_name, Int32.Parse(dgv_project[0, dgv_project.CurrentRow.Index].Value.ToString()));//, id); выбранного элемента

            if (form.ShowDialog() == DialogResult.OK)
            {
                //изменений значений атрибутов, если диалог формы равен ОК
                requ_attr = form.Get_requ_attr();

                dgv_project[1, dgv_project.CurrentRow.Index].Value = requ_attr.ElementAt(0);
                attr = form.Get_attrs();

                Form1.Add_or_change_proj_array(requ_attr, attr, form.Get_id());

                res = DialogResult.OK;
            }
        }

        private void btn_delete_Click(object sender, EventArgs e) //нажата кнопка Удалить
        {
            //окно с подтверждением удаления
            DialogResult rsl = MessageBox.Show("Вы уверены, что хотите удалить информацию о проекте?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rsl == DialogResult.Yes)
            {
                //если удаление подтверждено, то удаление выбранного проекта из массива
                
                int remove_id = Int32.Parse(dgv_project[0, dgv_project.CurrentRow.Index].Value.ToString());

              
                foreach (Project proj in Form1.projects)
                {
                    if (proj.ID == remove_id)
                    {
                        Form1.projects.Remove(proj);
                        break;
                    }

                }
                //обновление области отображения проектов
                dgv_View();


                res = DialogResult.Abort;
            }
        }

        private void Project_change_Load(object sender, EventArgs e) //загрузка окна
        {
            //задание параметров таблицы отображения
            if (Form1.projects != null)
            {
                if (Form1.projects.Count > 0)
                {
                    dgv_project.ColumnCount = 2;

                    dgv_project.Columns[0].Width = 0;
                    dgv_project.Columns[0].Visible = false;

                    dgv_project.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv_project.Columns[1].Name = "Название";
              

                    dgv_project.RowHeadersVisible = false;
                    dgv_project.ColumnHeadersVisible = true;
                    dgv_project.AllowUserToAddRows = false;
                    dgv_project.AllowUserToDeleteRows = false;
                    dgv_project.AllowUserToResizeColumns = false;
                    dgv_project.AllowUserToResizeRows = false;
                    dgv_project.ReadOnly = true;
                    dgv_project.Font = new Font("Modern No. 20", 12, FontStyle.Regular);

                    dgv_View();
                }
                else
                {
                    MessageBox.Show("Нет ни одного проекта");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Нет ни одного проекта");
                this.Close();
            }
        }

        private void dgv_View() //функция для отображения названий проектов в таблицу
        {


            while (dgv_project.Rows.Count > 0)
            {
                dgv_project.Rows.RemoveAt(0);
            }

            foreach (Project proj in Form1.projects)
            {
                dgv_project.Rows.Add(proj.ID, proj.Name);
            }
        }

        private void Project_change_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = res;
        }


    }
}
