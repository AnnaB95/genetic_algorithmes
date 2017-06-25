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
    public partial class Graphic_name : Form
    {
        string graphic_name = "График"; //имя графика

        public Graphic_name()
        {
            InitializeComponent();
        }

        public Graphic_name(int num)
        {
            graphic_name = "График " + num.ToString();

            InitializeComponent();
        }

        private void Graphic_name_Load(object sender, EventArgs e) //загрузка окна
        {
            this.CenterToParent(); //размещение в центре экрана
            textBox1.Text = graphic_name; //задание имя в текстовом поле
        }

        private void button1_Click(object sender, EventArgs e) //нажата кнопка ОК
        {
            graphic_name = textBox1.Text; //занесение имени из текстового поля в переменную
            this.Close(); //закрытие окна
        }

        public string Get_graphic_name() //метод, возвращающий имя графика
        {
            return graphic_name;
        }
    }
}
