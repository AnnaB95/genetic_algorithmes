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
    public partial class Add_new_attr : Form
    {

        public string attr_name; //имя атрибута

        public Add_new_attr()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e) //нажата кнопка ОК
        {         
            attr_name = tb_name_attr.Text;   //имя атрибута берется из текстового поля
            this.DialogResult = DialogResult.OK;    //возврат результата диалога для формы - ОК
            this.Close(); //закрытие окна
        }

        private void Add_new_attr_or_value_Load(object sender, EventArgs e) //загрузка окна
        {
            this.CenterToParent(); //размещение в центре родительского окна
            tb_name_attr.Text = ""; //очистка текстового поля
        }

    }
}
