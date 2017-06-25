using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace gen_alg
{
    public partial class Project_add : Form
    {
        string work_file_name;

        int id = -1;    //идентификатор изменяемого или добавляемого проекта

        Project adding_project; //добавляемый/изменяемый проект
        Dictionary<string, Dictionary<int, string>> attributes_list;    //массив атрибутов и всех их значений
        List<string> required_attr; //массив обязательных атрибутов проекта
        Dictionary<List<ComboBox>, NumericUpDown> attributes;   //массив пар элементов: выпадающий списков и числовых полей
        //кадый элемент массива содержит массив из пары выпадающих списков, первый из которых - список атрибутов, второй - список значений, числовое поле - вес атрибута для проекта
       
        XmlDocument project_xml_doc;    //документ project.xml



        List<string> required_attrs_value;  //массив значений обязательных атрибутов
        Dictionary<KeyValuePair<string, int>, string> attributes_values;    //массив атрибутов проекта


        public Project_add()
        {
            InitializeComponent();
        }

        public Project_add(string work_file_name)
        {
            this.work_file_name = work_file_name;

            InitializeComponent();
        }

        public Project_add(string work_file_name, int id)
        {
            this.work_file_name = work_file_name;
            this.id = id;

            InitializeComponent();
        }


        private void Project_add_Load(object sender, EventArgs e)   //загрузка окна
        {
            this.CenterToParent();

          
            adding_project = new Project();
            attributes_list = new Dictionary<string, Dictionary<int, string>>();
            required_attr = new List<string>();
            attributes = new Dictionary<List<ComboBox>, NumericUpDown>();

            //задание параметров таблицы обязательных атрибутов
            dgv_requ_attr.ColumnCount = 3;

            dgv_requ_attr.Columns[0].Width = dgv_requ_attr.Width / 2;
            dgv_requ_attr.Columns[1].Width = (dgv_requ_attr.Width / 2) * 3 / 4;
            dgv_requ_attr.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_requ_attr.Columns[0].Name = "Атрибут";
            dgv_requ_attr.Columns[1].Name = "Значение";
            dgv_requ_attr.Columns[2].Name = "Вес";

            dgv_requ_attr.RowHeadersVisible = false;
            dgv_requ_attr.ColumnHeadersVisible = true;
            dgv_requ_attr.AllowUserToAddRows = false;
            dgv_requ_attr.AllowUserToDeleteRows = false;
            dgv_requ_attr.AllowUserToResizeColumns = false;
            dgv_requ_attr.AllowUserToResizeRows = false;
            dgv_requ_attr.ReadOnly = false;
            dgv_requ_attr.Font = new Font("Modern No. 20", 12, FontStyle.Regular);
                  

            attributes_list = Read_XML_project(attributes_list);

            //если нужно изменить проект, то отображаем информацию о проекте
            if (id > 0)
            {
                this.Text = "Редактировать проект";
                btn_add.Text = "Редактировать";
                View_changable_pr();
            }
           else //иначе создаём заготовку для создания нового проекта
            {
                this.Text = "Добавить проект";
                btn_add.Text = "Добавить";
                View_maket_for_add_pr();
            }
        }
     

        private ComboBox CB_attrs_name(int position)    //формирование выпадающего списка атрибутов
        {
            ComboBox cb = new ComboBox();
           
            foreach (string names in attributes_list.Keys)
            {
                //добавление в список имен всех атрибутов проектов
                cb.Items.Add(names);
                //позиция на экране
                cb.Location = new Point(0, 21 * position);
                //размер
                cb.Size = new Size(panel_attr.Width / 2, 21);
                //реакция на изменение выбранного элемента
                cb.SelectedIndexChanged += new System.EventHandler(ComboBox1_SelectedIndexChanged);

            }
            cb.Items.Add("Другое...");

            if (cb.Items.Count == 1)
            {
                cb.Items.Insert(0, " ");
            }

            if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }

            return cb;
        }

        private NumericUpDown nUD_wieght(int position)  //формирование текстового поля для относительного веса
        {
            NumericUpDown nUD = new NumericUpDown();
            //задание параметров, начального значения, места отображения и размера
            nUD.Minimum = 1;
            nUD.Maximum = 100;
            nUD.Value = nUD.Minimum;
            nUD.Location = new Point((panel_attr.Width / 2) + (panel_attr.Width / 2) * 3 / 4, 21 * position);
            nUD.Size = new Size((panel_attr.Width / 2) / 4, 21);

            return nUD;
        }

     

        private void ComboBox_attr_val_SelectedIndexChanged(object sender, System.EventArgs e)  //изменение значения атрибута в списке
        {
            ComboBox comboBox = (ComboBox)sender;
            foreach (List<ComboBox> cb in attributes.Keys)
            {
                if (cb[1] == comboBox)
                {
                    //если выбрано "Другое", то запуск окна добавления значения атрибута
                    if (cb[1].SelectedIndex == (cb[1].Items.Count - 1))
                    {
                        Add_new_value form = new Add_new_value(cb[0].SelectedItem.ToString(), attributes_list[cb[0].SelectedItem.ToString().Trim()]);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            //если значение атрибута добавлено, то обновить список значений атрибута
                            attributes_list[cb[0].SelectedItem.ToString().Trim()] = form.Get_attr_values();
                            //записать значение атрибута в файл
                            Write_attr_val_in_XML(cb[0].SelectedItem.ToString(), form.Get_attr_values());
                            cb[1] = Change_cb_items(cb[1], cb[0].SelectedItem.ToString());
                        
                        }
                        else
                        {
                            cb[1].SelectedIndex = 0;
                        }
                    }
                }
            }
        }

    private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e) //реакция на изменение выбранного элемента в списке атрибутов
    {
         ComboBox comboBox = (ComboBox)sender;

            if (panel_attr.Controls.Contains(comboBox))
            {
                foreach (List<ComboBox> cb in attributes.Keys)
                {
                    if (cb[0] == comboBox)
                    {
                        //если выбрано "Другое", то открытие окна добавления атрибута
                        if (cb[0].SelectedIndex == (cb[0].Items.Count - 1))
                        {
                            Add_new_attr form = new Add_new_attr();
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                //если новый атрибут задан, то он отображается во всех выпадающих списках
                                CB_add_new_attr(form.attr_name);
                           
                                //заносится в перечень возможных атрибутов проекта
                                attributes_list.Add(form.attr_name, null);
                                //записывается в файл проекта
                                Write_new_attr_in_XML(form.attr_name);
                                cb[0].SelectedIndex = cb[0].Items.Count - 2;                            
                                                              

                            }
                            else
                            {
                                cb[0].SelectedIndex = 0;
                            }
                        }
                        else //иначе изменение парного выпадающего списка значений атрибута
                        {

                            cb[1] = Change_cb_items(cb[1], cb[0].SelectedItem.ToString());
                            if (cb[1].Items.Count > 0)
                            {
                                cb[1].SelectedIndex = 0;
                            }

                            break;
                        }
                    }
                }
            }
    }

        private void CB_add_new_attr(string attr_name) //добавление нового атрибута во все выпадающие списки атрибутов
        {
            foreach (List<ComboBox> cb in attributes.Keys)
            {
                cb[0].Items.Insert(cb[0].Items.Count - 1, attr_name);
               
            }
        }
        private ComboBox CB_values_for_attr(int position, string attr_name) //формирование выпадающего списка для значений атрибута
        {
            ComboBox cb = new ComboBox();
            foreach (KeyValuePair<int, string> values in attributes_list[attr_name])
            {
                //выборка значений по заданному атрибуту
                cb.Items.Add(values.Value);
                //местоположение на форме
                cb.Location = new Point(panel_attr.Width / 2, 21 * position);
                //размер
                cb.Size = new Size((panel_attr.Width / 2) * 3 / 4, 21);
                
                //реакция на изменение значения
                cb.SelectedIndexChanged += new System.EventHandler(ComboBox_attr_val_SelectedIndexChanged);

            }
            cb.Items.Add("Другое...");

            if (cb.Items.Count == 1)
            {
                cb.Items.Insert(0, " ");
            }

            if (cb.Items.Count > 0)
            {
                cb.SelectedIndex = 0;
            }
            return cb;
        }

        private ComboBox Add_cb_items(ComboBox cb, string item) //добаление нового значения атрибута
        {
            cb.Items.Add(item);
            return cb;
        }

        private ComboBox Change_cb_items(ComboBox cb, string attr_name) //изменение списка значений по атрибуту
        {
            //выборка в список новых значений в соответствии с изменившимся атрибутом
            cb.Items.Clear();
            if (attributes_list.Keys.Contains(attr_name))
            {
                if (attributes_list[attr_name] != null)
                {
                    foreach (KeyValuePair<int, string> values in attributes_list[attr_name])
                    {
                        cb.Items.Add(values.Value);
                    }
                    
                }
            }
            cb.Items.Add("Другое...");

            if (cb.Items.Count == 1)
            {
                cb.Items.Insert(0, " ");
            }

            cb.SelectedIndex = 0;

            return cb;
        }

        private void View_changable_pr()    //метод для отображения изменяемого проекта
        {
            //выборка информации о проекте
            foreach (Project proj in Form1.projects)
            {
                if (proj.ID == this.id)
                {
                    adding_project = proj;
                    break;
                }
            }
            //очистка таблицы обязательных атрибутов
            Clear_table();
            //занесение информации в таблицу обязательных атрибутов
            dgv_requ_attr.Rows.Add("Название", adding_project.Name, "");
            dgv_requ_attr[0, 0].ReadOnly = true;
            dgv_requ_attr[2, 0].ReadOnly = true;
            
            //очистка панели дополнительных атрибутов
            Panel_clear();

            //отображение атрибутов проекта, их значений и весов
            foreach (KeyValuePair<KeyValuePair<string, int>, string> attr in adding_project.Get_all_attrib())
            {
                ComboBox cb = CB_attrs_name(attributes.Count);
                cb.SelectedItem = attr.Key.Key;
                ComboBox cb_v = CB_values_for_attr(attributes.Count, cb.SelectedItem.ToString());
                cb_v.SelectedItem = attr.Value;
                List<ComboBox> lcb = new List<ComboBox>();
                lcb.Add(cb); lcb.Add(cb_v);
                NumericUpDown nUD = nUD_wieght(attributes.Count);
                nUD.Value = attr.Key.Value;
                attributes.Add(lcb, nUD);

                panel_attr.Controls.Add(lcb.ElementAt(0));
                panel_attr.Controls.Add(lcb.ElementAt(1));
                panel_attr.Controls.Add(nUD);
            }
            
        }


        private void Clear_table()  //очистка таблицы обязательных атрибутов
        {
            while (dgv_requ_attr.Rows.Count > 0)
            {
                dgv_requ_attr.Rows.RemoveAt(0);
            }
        }

        private void Panel_clear()  //очистка панели атрибутов
        {
            panel_attr.Controls.Clear();
        }

        private void View_maket_for_add_pr()    //создание шаблона для добавления проекта
        {
            //очистка таблицы обязательных атрибутов
            Clear_table();
            //формирование заготовки
            dgv_requ_attr.Rows.Add("Название", adding_project.Name, "");
            dgv_requ_attr[0, 0].ReadOnly = true;
            dgv_requ_attr[2, 0].ReadOnly = true;
        }


        private void Write_new_attr_in_XML(string attr)         //запись атрибута в файл с информацией об атрибутах проектов
        {
            //создание нового элемента и запись его в файл
            project_xml_doc.Load("projects.xml");

            XmlElement root = project_xml_doc.DocumentElement;

            XmlNode attr_list = root.LastChild;

            XmlElement new_attr = project_xml_doc.CreateElement("attr");
            XmlAttribute name_attr = project_xml_doc.CreateAttribute("name");
            XmlText text_name = project_xml_doc.CreateTextNode(attr);

            name_attr.AppendChild(text_name);
            new_attr.Attributes.Append(name_attr);
            attr_list.AppendChild(new_attr);
            project_xml_doc.Save("projects.xml");
        }

     
        private void Write_attr_val_in_XML(string attr, Dictionary<int, string> values)    //запись информации о значениях атрибута в файл об атрибутах проектов
        {
            //поиск нужного атрибута и обновление информации о его значениях
            project_xml_doc.Load("projects.xml");

            XmlElement root = project_xml_doc.DocumentElement;

            XmlNode attr_list = root.LastChild;

            foreach (XmlNode attributes in attr_list.ChildNodes)
            {
                XmlNode name = attributes.Attributes.GetNamedItem("name");

                if (name != null)
                {
                    if (name.Value == attr)
                    {
                       while (attributes.ChildNodes[0] != null)
                        {
                            attributes.RemoveChild(attributes.ChildNodes[0]);
                        }

                        foreach (KeyValuePair<int, string> val in values)
                        {
                            XmlElement new_attr = project_xml_doc.CreateElement("value");

                            XmlAttribute id_attr = project_xml_doc.CreateAttribute("id");
                            XmlText text_id = project_xml_doc.CreateTextNode(val.Key.ToString());
                            XmlAttribute name_attr = project_xml_doc.CreateAttribute("name");
                            XmlText text_name = project_xml_doc.CreateTextNode(val.Value);

                            id_attr.AppendChild(text_id);
                            name_attr.AppendChild(text_name);
                            new_attr.Attributes.Append(id_attr);
                            new_attr.Attributes.Append(name_attr);
                            attributes.AppendChild(new_attr);
                           
                        }
                        break;
                    }
                }
            }
            project_xml_doc.Save("projects.xml");
        }

       
        public Dictionary<string, Dictionary<int, string>> Read_XML_project(Dictionary<string, Dictionary<int, string>> attributes_list)     //чтение файла об атрибутах проектов
        {
            //чтение происходит в соответствии со структурой файла, описанной в приложении
            project_xml_doc = new XmlDocument();
            project_xml_doc.Load("projects.xml");

            foreach (XmlNode root in project_xml_doc.DocumentElement)
            {
                if (root.Name == "required-attr")
                {
                    foreach (XmlNode requ_attr in root.ChildNodes)
                    {
                        XmlNode name = requ_attr.Attributes.GetNamedItem("name");

                        if (name != null)
                        {
                            required_attr.Add(name.Value);
                        }
                    }
                }
                else
                {
                    if (root.Name == "attr-list")
                    {
                        foreach (XmlNode attr_list in root.ChildNodes)
                        {
                            XmlNode name = attr_list.Attributes.GetNamedItem("name");

                            if (name != null)
                            {
                                Dictionary<int, string> values = new Dictionary<int, string>();

                                foreach (XmlNode val in attr_list.ChildNodes)
                                {
                                    XmlNode id = val.Attributes.GetNamedItem("id");
                                    XmlNode names = val.Attributes.GetNamedItem("name");

                                    if (id != null & names != null)
                                    {
                                        values.Add(Int32.Parse(id.Value), names.Value);
                                    }
                                }

                                attributes_list.Add(name.Value, values);
                            }
                        }
                    }
                }
            }
            return attributes_list;

            
        }

        private void btn_cancel_Click(object sender, EventArgs e)   //нажата кнопка Отмена
        {
            //возврат диалога формы NO и закртыие окна
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btn_add_str_Click(object sender, EventArgs e)  //нажата кнопка +
        {   
            //формирование новых выпадающих списков и текстового поля и добавление их на форму         
            ComboBox cb = CB_attrs_name(attributes.Count);
            ComboBox cb_v = CB_values_for_attr(attributes.Count, cb.SelectedItem.ToString());
            List<ComboBox> lcb = new List<ComboBox>();
            lcb.Add(cb); lcb.Add(cb_v);

            NumericUpDown nUD = nUD_wieght(attributes.Count);

            attributes.Add(lcb,nUD);

            panel_attr.Controls.Add(cb);
            panel_attr.Controls.Add(cb_v);
            panel_attr.Controls.Add(nUD);
        }      

        private void btn_del_str_Click(object sender, EventArgs e)  //нажата кнопка -
        {
            //удаление последних выпадающих списков и числового поля
            if (attributes.Count > 0)
            {
                panel_attr.Controls.Remove(attributes.Keys.ElementAt(attributes.Count - 1)[0]);
                panel_attr.Controls.Remove(attributes.Keys.ElementAt(attributes.Count - 1)[1]);
                panel_attr.Controls.Remove(attributes.Values.ElementAt(attributes.Count - 1));
                attributes.Remove(attributes.Keys.ElementAt(attributes.Count - 1));
            }            
        }
             

        private void btn_add_Click(object sender, EventArgs e)  //нажата кнопка Добавить
        {
            //формирование идентификатора проекта (при добавлении), занесение информации об атрибутах в массивы обязательных и дополнительных атрибутов проекта
            required_attrs_value = new List<string>();

            if (id < 0)
            {
                int counter = 0;

                List<int> ids = new List<int>();

                foreach (Project pr in Form1.projects)
                {
                    ids.Add(pr.ID);
                }

                do
                {
                    ++counter;
                }
                while (ids.Contains(counter));

                id = counter;
            }

                    for (int i = 0; i < dgv_requ_attr.Rows.Count; i++)
            {
                required_attrs_value.Add(dgv_requ_attr[1, i].Value.ToString());
            }

            attributes_values = new Dictionary<KeyValuePair<string, int>, string>();

            foreach (KeyValuePair<List<ComboBox>, NumericUpDown> attr in attributes)
            {
                bool flag = true;
               foreach (KeyValuePair<string, int> pa in attributes_values.Keys)
                {
                    if (pa.Key == attr.Key[0].SelectedItem.ToString())
                    {
                        flag = false;
                        break;
                    }
                }
               if (flag)
                attributes_values.Add((new KeyValuePair<string, int>(attr.Key[0].SelectedItem.ToString(), Int32.Parse(attr.Value.Value.ToString()))), attr.Key[1].SelectedItem.ToString());
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public  List<String> Get_requ_attr()    //возврат массива обязательных атрибутов
        {
            return required_attrs_value;
        }

        public Dictionary<KeyValuePair<string, int>, string> Get_attrs()    //возврат массива атрибутов
        {
            return attributes_values;
        }

        public int Get_id() //возврат идентификатора
        {
            return id;
        }
    }
}
