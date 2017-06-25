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
    public partial class More : Form
    {
        object obj;

        public More()
        {
            InitializeComponent();
        }

        public More(object obj)
        {
            this.obj = obj;

            InitializeComponent();
        }

        private void More_info_Load(object sender, EventArgs e) //загрузка окна
        {
            //очистка области отображения
            listBox1.Items.Clear();

          
            //отображение инфорамции о проекте или сотруднике с указанием всех атрибутов, их значений и относительных весов
            if (obj is Project)
            {
                Project inf_obj = (Project)obj;
                listBox1.Items.Add("Проект: " + inf_obj.Name);
              

                foreach(KeyValuePair<KeyValuePair<string, int>, string> attr in inf_obj.Get_all_attrib())
                {
                    listBox1.Items.Add(attr.Key.Key + ": " + attr.Value + ", относительный вес: " + attr.Key.Value);
                }
            }
            else
                if (obj is Employee)
            {
                Employee inf_obj = (Employee)obj;
                listBox1.Items.Add("Сотрудник: " + inf_obj.Name);
                listBox1.Items.Add("Возраст: " + inf_obj.Age);
                listBox1.Items.Add("Опыт работы: " + inf_obj.Experience);

                foreach (KeyValuePair<KeyValuePair<string, int>, string> attr in inf_obj.Get_all_attrib())
                {
                    listBox1.Items.Add(attr.Key.Key + ": " + attr.Value + ", относительный вес: " + attr.Key.Value);
                }
            }
        }

       
        
    }
}
