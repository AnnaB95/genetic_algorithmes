using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using GeneticAlg;

namespace gen_alg
{
    public partial class Form1 : Form
    {

        string work_file_name; //имя файла с исходными данными
        public static List<Employee> employees = new List<Employee>(); //список сотрдников
        public static List<Project> projects = new List<Project>();  //список проектов
        Random rnd; //переменная для генерации случайных чисел
        Population global_best_chromosome; //наилучшая хромосома

        bool flag_work_alg = false; //флаг, показывающий запускался ли генетический алгоритм

        List<List<int>> points_for_graphics = new List<List<int>>(); //список точек для построения графиков
        List<string> graphics_names = new List<string>();

        XmlDocument work_xml_doc; //переменная для работы с xml документами

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen(); //размещение формы в центре экрана

            Start_values(); 
            ReadIniFile();
            rnd = new Random(); //инициализация генератора случайных чисел
        }

        private void Start_values() //функция, задающая начальные значения элементам приложения
        {
            //dgv_start_data - таблица для вывода проектов и сотрудников во вкладке "Настройки"

            dgv_start_data.ColumnCount = 4; //задание количества столбцов

            //задание размеров, видимости и имён столбцов, а также параметров таблицы
            //имеется 2 столбца (с индексами 1 и 3), имеющие 0 длину и являющиеся невидимыми. Эти столбцы содержат id проектов и сотрудников для простоты обращения к ним в главных списках
            dgv_start_data.Columns[0].Width = dgv_start_data.Width / 2; 
            dgv_start_data.Columns[1].Width = 0;
            dgv_start_data.Columns[1].Visible = false;
            dgv_start_data.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_start_data.Columns[3].Width = 0;
            dgv_start_data.Columns[3].Visible = false;
            dgv_start_data.Columns[0].Name = "Проекты";
            dgv_start_data.Columns[2].Name = "Сотрудники";

            dgv_start_data.RowHeadersVisible = false; 
            dgv_start_data.ColumnHeadersVisible = false;
            dgv_start_data.AllowUserToAddRows = false;
            dgv_start_data.AllowUserToDeleteRows = false;
            dgv_start_data.AllowUserToResizeColumns = false;
            dgv_start_data.AllowUserToResizeRows = false;
            dgv_start_data.ReadOnly = true;
            dgv_start_data.Font = new Font("Modern No. 20", 12, FontStyle.Regular);


            tb_filename.Text = ""; //очистка textbox, хранящего имя исходного файла

            //задание возможности приближения на графике
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;            
        }

        private void menu_load_Click(object sender, EventArgs e) //обработчик события, возникающего при нажатии на кнопку загрузки в меню Файл
        {
            OpenFileDialog ofd = new OpenFileDialog(); //переменная OpenFileDialog
            ofd.Filter = "xml|*.xml"; //задание фильтра

            if (ofd.ShowDialog() == DialogResult.OK) //если был выбран файл
            { //то
                Clean_table(); //очистка таблицы dgv_start_data

                tb_filename.Text = ofd.FileName; //внесение имени файла в textbox

                dgv_start_data.ColumnHeadersVisible = true; //задание видимости для заголовков столбцов таблицы

                work_file_name = ofd.FileName; //присвоение переменной имени загруженного файла

                employees = new List<Employee>(); //инициализация списка сотрудников
                projects = new List<Project>(); //инициализация списка проектов

                Read_XmlDoc(ofd.FileName); //запуск функции чтения выбранного xml файла


                View_in_dgv(); //запуск функции вывода содержимого файла в таблицу

                btn_info.Enabled = true; //активация доступности кнопки "Подробности"
            }
        }

        private void Clean_table() //функция очистки таблицы dgv_start_data
        {
            while (dgv_start_data.Rows.Count > 0) //пока количество строк в таблице больше 0, удаляем первую строку
            {
                dgv_start_data.Rows.RemoveAt(0);
            }
        }

        private void ReadIniFile() //функция чтения файла настроек алгоритма
        {
            string path = Application.StartupPath + "\\settings.ini"; //задание пути к файлу

            FilesIni ini_file = new FilesIni(path); //инфициализация переменной ini-файла

            //получение значений параметров и размещение их в соответствующиих элементах
            string chrom_count = ini_file.GetPrivateString("chromosome_number");
            nUD_chrom_count.Value = Int32.Parse(chrom_count);
            string popul_count = ini_file.GetPrivateString("population_number");
            nUD_popul_count.Value = Int32.Parse(popul_count);
            string max_empl_count = ini_file.GetPrivateString("max_employees_count");
            nUD_max_empl.Value = Int32.Parse(max_empl_count);
            string selection_method = ini_file.GetPrivateString("selection_method");


            switch (selection_method)
            {
                case "roulette":
                    rb_roulette.Checked = true;
                    break;
                case "tournier":
                    rb_tournier.Checked = true;
                    break;
                case "rang":
                    rb_rang.Checked = true;
                    break;
                default:
                    rb_roulette.Checked = true;
                    break;
            }

            string mutation = ini_file.GetPrivateString("mutation_chance");
            tb_mutation.Text = mutation;
            string conversion = ini_file.GetPrivateString("conversion_chance");
            tb_conversion.Text = conversion;
            string graphics_number = ini_file.GetPrivateString("max_graphics_number");
            nUD_graphics.Value = Int32.Parse(graphics_number);
        }

        private void Read_XmlDoc(string file_name) //чтение загружаемого xml-файла
                                                   //структура файла описана в документе "Приложение.docx". 
                                                   //Перебор элементов, атрибутов, а также получение их параметров производится стандартными функциями в соответствии с описанной структурой файла
                                                   //считывание из рабочего xml файла полной информации о проектах и сотрудников и занесение её в массивы сотрудников и проектов
        {
            work_xml_doc = new XmlDocument();
            work_xml_doc.Load(file_name);

            foreach (XmlNode root in work_xml_doc.DocumentElement) 
            {

                if (root.Name == "projects-list") 
                {
                    foreach (XmlNode proj in root.ChildNodes)
                    {

                        Project new_project = new Project();

                        XmlNode id_pr = proj.Attributes.GetNamedItem("id");

                        if (id_pr != null)
                        {
                            new_project.ID = Int32.Parse(id_pr.Value);
                        }

                        foreach (XmlNode attr in proj.ChildNodes)
                        {
                            if (attr.Name == "required-attr")
                            {
                                foreach (XmlNode requ_attr in attr.ChildNodes)
                                {
                                    XmlNode name = requ_attr.Attributes.GetNamedItem("name");

                                    if (name != null)
                                    {
                                        new_project.Name = name.Value;
                                    }
                                }
                            }
                            else
                            if (attr.Name == "attr-list")
                            {
                                foreach (XmlNode attr_list in attr.ChildNodes)
                                {
                                    XmlNode weight = attr_list.Attributes.GetNamedItem("weight");
                                    XmlNode name = attr_list.Attributes.GetNamedItem("name");
                                    XmlNode value = attr_list.Attributes.GetNamedItem("value");

                                    if (weight != null & name != null & value != null)
                                    {
                                        new_project.Add_attrib(name.Value, Int32.Parse(weight.Value), value.Value);
                                    }
                                }
                            }
                        }

                        projects.Add(new_project);

                    }
                }

                if (root.Name == "employees-list")
                {
                    foreach (XmlNode empl in root.ChildNodes)
                    {
                        Employee new_employee = new Employee();

                        XmlNode id_emp = empl.Attributes.GetNamedItem("id");

                        if (id_emp != null)
                        {
                            new_employee.ID = Int32.Parse(id_emp.Value);
                        }

                        foreach (XmlNode attr in empl.ChildNodes)
                        {
                            if (attr.Name == "required-attr")
                            {
                                foreach (XmlNode requ_attr in attr.ChildNodes)
                                {
                                    XmlNode name = requ_attr.Attributes.GetNamedItem("name");

                                    if (name != null)
                                    {
                                        new_employee.Name = name.Value;
                                    }

                                    XmlNode age = requ_attr.Attributes.GetNamedItem("age");

                                    if (age != null)
                                    {
                                        new_employee.Age = Int32.Parse(age.Value);
                                    }

                                    XmlNode experience = requ_attr.Attributes.GetNamedItem("experience");

                                    if (experience != null)
                                    {
                                        new_employee.Experience = Int32.Parse(experience.Value);
                                    }
                                }
                            }
                            else
                                if (attr.Name == "attr-list")
                            {
                                foreach (XmlNode attr_list in attr.ChildNodes)
                                {
                                    XmlNode weight = attr_list.Attributes.GetNamedItem("weight");
                                    XmlNode name = attr_list.Attributes.GetNamedItem("name");
                                    XmlNode value = attr_list.Attributes.GetNamedItem("value");

                                    if (weight != null & name != null & value != null)
                                    {                                        
                                        new_employee.Add_attrib(name.Value, Int32.Parse(weight.Value), value.Value);
                                    }
                                }
                            }
                        }

                        employees.Add(new_employee);
                    }
                }

                
            }
        }


        private void View_in_dgv() //занесение информации о проектах и сотрудниках в таблицу dgv_start_data
        {
            Clean_table();
            int count_pr = projects.Count;
            int count_empl = employees.Count;
            int count = (count_pr > count_empl) ? count_pr : count_empl;


            for (int i = 0; i < count; i++)
            {
                int id_pr;
                string name_pr;

                int id_empl;
                string name_empl;

                if (i < count_pr)
                {
                    id_pr = projects[i].ID;
                    name_pr = projects[i].Name;
                }
                else
                {
                    id_pr = 0;
                    name_pr = "";
                }

                if (i < count_empl)
                {
                    id_empl = employees[i].ID;
                    name_empl = employees[i].Name;
                }
                else
                {
                    id_empl = 0;
                    name_empl = "";
                }

                dgv_start_data.Rows.Add(name_pr, id_pr, name_empl, id_empl);
            }
        }

        private void menu_save_Click(object sender, EventArgs e) //загрузка исходного файла 
        {
            if (work_file_name != null)
                saveFileDialog1.FileName = work_file_name;

            saveFileDialog1.Filter = "XML-файл |*.xml";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                //если выбрано имя файла, то на основе данных массивово проектов и сотрудников формируется файл исходных данных, со структурой, описанной в Приложении
            {
                XmlDocument xml_doc = new XmlDocument();

                XmlTextWriter textWritter = new XmlTextWriter(saveFileDialog1.FileName, System.Text.Encoding.UTF8);

                textWritter.WriteStartDocument();

                textWritter.WriteStartElement("root");

                textWritter.WriteEndElement();

                textWritter.Close();
               
                xml_doc.Load(saveFileDialog1.FileName);

                XmlElement root = xml_doc.DocumentElement;

                if (projects.Count > 0)
                {
                    XmlElement pr_list = xml_doc.CreateElement("projects-list");

                    foreach (Project pr in projects)
                    {
                        XmlElement project = xml_doc.CreateElement("projects");
                        XmlAttribute id = xml_doc.CreateAttribute("id");
                        XmlText text_id = xml_doc.CreateTextNode(pr.ID.ToString());

                        id.AppendChild(text_id);

                        project.Attributes.Append(id);

                        XmlElement requ = xml_doc.CreateElement("required-attr");
                        XmlElement ra_name = xml_doc.CreateElement("attr");
                        XmlAttribute pr_name = xml_doc.CreateAttribute("name");
                        XmlText text_name = xml_doc.CreateTextNode(pr.Name);

                        pr_name.AppendChild(text_name);
                        ra_name.Attributes.Append(pr_name);
                        requ.AppendChild(ra_name);
                        project.AppendChild(requ);

                        if (pr.Get_all_attrib().Count > 0)
                        {
                            XmlElement attr_list = xml_doc.CreateElement("attr-list");

                            foreach (KeyValuePair<KeyValuePair<string, int>, string> kvp in pr.Get_all_attrib())
                            {
                                XmlElement attr = xml_doc.CreateElement("attr");

                                XmlAttribute weight = xml_doc.CreateAttribute("weight");
                                XmlText text_weight = xml_doc.CreateTextNode(kvp.Key.Value.ToString());

                                XmlAttribute attr_name = xml_doc.CreateAttribute("name");
                                XmlText text_attr_name = xml_doc.CreateTextNode(kvp.Key.Key);

                                XmlAttribute attr_value = xml_doc.CreateAttribute("value");
                                XmlText text_attr_value = xml_doc.CreateTextNode(kvp.Value);

                                weight.AppendChild(text_weight);
                                attr_name.AppendChild(text_attr_name);
                                attr_value.AppendChild(text_attr_value);

                                attr.Attributes.Append(weight);
                                attr.Attributes.Append(attr_name);
                                attr.Attributes.Append(attr_value);

                                attr_list.AppendChild(attr);
                            }

                            project.AppendChild(attr_list);
                        }
                        pr_list.AppendChild(project);

                    }

                    root.AppendChild(pr_list);
                }



                if (employees.Count > 0)
                {
                    XmlElement empl_list = xml_doc.CreateElement("employees-list");

                    foreach (Employee pr in employees)
                    {
                        XmlElement employee = xml_doc.CreateElement("employee");
                        XmlAttribute id = xml_doc.CreateAttribute("id");
                        XmlText text_id = xml_doc.CreateTextNode(pr.ID.ToString());

                        id.AppendChild(text_id);

                        employee.Attributes.Append(id);

                        XmlElement requ = xml_doc.CreateElement("required-attr");

                        XmlElement ra_name = xml_doc.CreateElement("attr");
                        XmlAttribute pr_name = xml_doc.CreateAttribute("name");
                        XmlText text_name = xml_doc.CreateTextNode(pr.Name);

                        XmlElement ra_age = xml_doc.CreateElement("attr");
                        XmlAttribute pr_age = xml_doc.CreateAttribute("age");
                        XmlText text_age = xml_doc.CreateTextNode(pr.Age.ToString());

                        XmlElement ra_exp = xml_doc.CreateElement("attr");
                        XmlAttribute pr_exp = xml_doc.CreateAttribute("experience");
                        XmlText text_exp = xml_doc.CreateTextNode(pr.Experience.ToString());

                        pr_name.AppendChild(text_name);
                        ra_name.Attributes.Append(pr_name);
                        pr_age.AppendChild(text_age);
                        ra_age.Attributes.Append(pr_age);
                        pr_exp.AppendChild(text_exp);
                        ra_exp.Attributes.Append(pr_exp);

                        requ.AppendChild(ra_name);
                        requ.AppendChild(ra_age);
                        requ.AppendChild(ra_exp);

                        employee.AppendChild(requ);

                        if (pr.Get_all_attrib().Count > 0)
                        {
                            XmlElement attr_list = xml_doc.CreateElement("attr-list");

                            foreach (KeyValuePair<KeyValuePair<string, int>, string> kvp in pr.Get_all_attrib())
                            {
                                XmlElement attr = xml_doc.CreateElement("attr");

                                XmlAttribute weight = xml_doc.CreateAttribute("weight");
                                XmlText text_weight = xml_doc.CreateTextNode(kvp.Key.Value.ToString());

                                XmlAttribute attr_name = xml_doc.CreateAttribute("name");
                                XmlText text_attr_name = xml_doc.CreateTextNode(kvp.Key.Key);

                                XmlAttribute attr_value = xml_doc.CreateAttribute("value");
                                XmlText text_attr_value = xml_doc.CreateTextNode(kvp.Value);

                                weight.AppendChild(text_weight);
                                attr_name.AppendChild(text_attr_name);
                                attr_value.AppendChild(text_attr_value);

                                attr.Attributes.Append(weight);
                                attr.Attributes.Append(attr_name);
                                attr.Attributes.Append(attr_value);

                                attr_list.AppendChild(attr);
                            }

                            employee.AppendChild(attr_list);
                        }
                        empl_list.AppendChild(employee);

                    }

                    root.AppendChild(empl_list);
                }
                xml_doc.Save(saveFileDialog1.FileName);
            }
         
        }

        private void menu_exit_Click(object sender, EventArgs e) //закрытие приложения
        {
            this.Close(); //закрытие окна приложения
        }

        private void menu_pr_add_Click(object sender, EventArgs e) //добавление проекта
        {
            //открытие окна добавления проекта
            Project_add form = new Project_add(work_file_name);

            if (form.ShowDialog() == DialogResult.OK)
            {
                //если проект добавлен, то вызываем функцию добавления его в массив проектов и обновление таблицы
                Add_or_change_proj_array(form.Get_requ_attr(), form.Get_attrs(), form.Get_id());
                View_in_dgv();
                btn_info.Enabled = true;
            }
        }

        private void menu_pr_change_Click(object sender, EventArgs e) //Редактирование проектов
        {
            //запуск окна редактирования проектов
            Project_change form = new Project_change(work_file_name);

            DialogResult result = form.ShowDialog();

            //обновление таблицы при внесении изменений

            if (result == DialogResult.OK)
            {
                View_in_dgv();
            }
            else
            {
                if (result == DialogResult.Abort)
                {
                    View_in_dgv();
                    if (employees.Count == 0 & projects.Count == 0)
                    {
                        btn_info.Enabled = false;
                    }
                }
            }
            
        }

        private void menu_empl_add_Click(object sender, EventArgs e) //добавление сотрудника
        {
            //открытие окна добавления сотрудника
            Employe_add form = new Employe_add(work_file_name);

            if (form.ShowDialog() == DialogResult.OK)
            {
                //если сотрудник добавлен, то вызываем функцию добавления его в массив сотрудников и обновление таблицы
                Add_or_change_empl_array(form.Get_requ_attr(), form.Get_attrs(), form.Get_id());
                View_in_dgv();
                btn_info.Enabled = true;
            }
            
        }

        private void menu_empl_change_Click(object sender, EventArgs e) //редактирование сотрудников
        {
            //запуск окна редактирования сотрудников
            Employee_change form = new Employee_change(work_file_name);
            DialogResult result = form.ShowDialog();

            //обновление таблицы при внесении изменений

            if (result == DialogResult.OK)
            {
                View_in_dgv();
            }
            else
            {
                if (result == DialogResult.Abort)
                {
                    View_in_dgv();
                    if (employees.Count == 0 & projects.Count == 0)
                    {
                        btn_info.Enabled = false;
                    }
                }
            }       
        }

        private void btn_info_Click(object sender, EventArgs e) //подробная информация о выделенном проекте или сотруднике
        {
            //открытие окна с информацией о выбранном проекте или сотруднике
            object info_obj;
            if (dgv_start_data.CurrentCell.ColumnIndex == 0)
            {
                info_obj = projects.ElementAt(dgv_start_data.CurrentCell.RowIndex);
            }
            else
            {
                info_obj = employees.ElementAt(dgv_start_data.CurrentCell.RowIndex);
            }

            More inf_form = new More(info_obj);
            inf_form.Show();
        }

        public static void Add_or_change_proj_array(List<string> requ_attr, Dictionary<KeyValuePair<string, int>, string> attrs, int id) //изменений массива проектов
        {
            Project ch_pr = new Project(); //создание нового проекта


            if (projects == null)
            {
                projects = new List<Project>();
            }

            bool flag = true;
            int counter = 0;

            //ищем в массиве проектов проект с заданным идентификатором
            foreach (Project pr in projects)
            {
                if (pr.ID == id)
                {
                    ch_pr = pr;
                    flag = false;
                    break;
                }
                ++counter;
            }

            
            if (flag)
            {
                ch_pr = new Project();
            }
            //формуируем информацию о создаваемом/обновляемом проекте
            ch_pr.ID = id;
            ch_pr.Name = requ_attr.ElementAt(0);

            ch_pr.Change_attrib(attrs);
          

            //если в массиве был найден проект с заданным идентификатором, то обновляем информацию о нём
            if (!flag)
            {
                projects.RemoveAt(counter);
                projects.Insert(counter, ch_pr);
            }
            else //иначе, добавляем новый преокт
            {
                projects.Add(ch_pr);
            }




        }

        public static void Add_or_change_empl_array(List<string> requ_attr, Dictionary<KeyValuePair<string, int>, string> attrs, int id) //изменений массива сотрудников
        {

            Employee ch_empl = new Employee(); //создание нового сотрудника

            if (employees == null)
            {
                employees = new List<Employee>();
            }

            bool flag = true;
            int counter = 0;

            //ищем в массиве сотрудников сотрудника с заданным идентификатором
            foreach (Employee pr in employees)
            {
                if (pr.ID == id)
                {
                    ch_empl = pr;
                    flag = false;
                    break;
                }
                ++counter;
            }

            if (flag)
            {
                ch_empl = new Employee();
            }

            //формуируем информацию о создаваемом/обновляемом сотруднике
            ch_empl.ID = id;
            ch_empl.Name = requ_attr.ElementAt(0);
            ch_empl.Age = Int32.Parse(requ_attr.ElementAt(1));
            ch_empl.Experience = Int32.Parse(requ_attr.ElementAt(2));

            ch_empl.Change_attrib(attrs);
          
            //если в массиве был найден сотрудник с заданным идентификатором, то обновляем информацию о нём
            if (!flag)
            {
                employees.RemoveAt(counter);
                employees.Insert(counter, ch_empl);
            }
            else //иначе, добавляем нового сотрудника
            {
                employees.Add(ch_empl);
            }


        }

        private void save_prop_Click(object sender, EventArgs e) //сохранение настроек
        {
            //внесение изменений в файл настроек из соответствующих полей формы
            string path = Application.StartupPath + "\\settings.ini";

            FilesIni ini_file = new FilesIni(path);
            string selection_method = "";

            ini_file.WritePrivateString("chromosome_count", nUD_chrom_count.Value.ToString());
            ini_file.WritePrivateString("population_number", nUD_popul_count.Value.ToString());
            ini_file.WritePrivateString("mutation_chance", tb_mutation.Text);
            ini_file.WritePrivateString("conversion_chance", tb_conversion.Text);
            ini_file.WritePrivateString("max_employees_count", nUD_max_empl.Value.ToString());
            ini_file.WritePrivateString("max_graphics_number", nUD_graphics.Value.ToString());


            if (rb_roulette.Checked)
            {
                selection_method = "roulette";
            }
            else
                 if (rb_tournier.Checked)
            {
                selection_method = "tournier";
            }
            else if (rb_rang.Checked)
            {
                selection_method = "rang";
            }
            else
            {
                selection_method = "roulette";
            }

            ini_file.WritePrivateString("selection_method", selection_method);


        }

        private void button1_Click(object sender, EventArgs e) //работа генетического алгоритма
        {
            if (employees.Count != 0 & projects.Count != 0) //если есть информация о проектах и сотрудниках
            {   //то
                if (employees.Count >= projects.Count) //если количество сотрудников больше числа проектов
                {   //то

                    global_best_chromosome = null; //сброс глобальной лучшей хромосомы
                    List<int> greatest_ff = new List<int>();    //массив лучших значений функций приспособленности популяций в течение работы алгоритма

                    List<int> employees_weight = new List<int>(); //массива относительных весов сотрудников
                    List<int> projects_weight = new List<int>(); //массив относительных весов проектов


                    //расчёт относительных весов и заполнение массивов
                    foreach (Employee empl in employees)
                    {
                        employees_weight.Add(Project_chromosome.Calculate_sum_weight(empl.Get_attributes_and_value_pair(), empl.Get_attributes_and_weight_pair(), new Employe_add().Read_XML_employees(new Dictionary<string, Dictionary<int, string>>())));
                    }

                    foreach (Project proj in projects)
                    {
                        projects_weight.Add(Project_chromosome.Calculate_sum_weight(proj.Get_attributes_and_value_pair(), proj.Get_attributes_and_weight_pair(), new Project_add().Read_XML_project(new Dictionary<string, Dictionary<int, string>>())));
                    }
                    //

                    List<Population> current_pop = new List<Population>(); //текущая популяция

                    List<int> empl_id = new List<int>(); //массив идентификаторов сотрудников

                    //добавление идентификаторов в массив
                    foreach (Employee empl in employees)
                    {
                        empl_id.Add(empl.ID);
                    }

                    List<int> project_id = new List<int>(); //массив идентификаторов проектов

                    //добавление идентификаторов в массив
                    foreach (Project proj in projects)
                    {
                        project_id.Add(proj.ID);
                    }

                    //формирование стартовой популяции
                    current_pop = GA.Start_population(project_id, empl_id, Int32.Parse(nUD_max_empl.Value.ToString()), Int32.Parse(nUD_chrom_count.Value.ToString()), rnd);

                    int current_population_count = 0; //текущий номер популяции
                    int max_population_count = Int32.Parse(nUD_popul_count.Value.ToString()); //максимально возможное число популяций

                    Population current_best_chromosome; //наилучший геном в популяции
                    int iter_counter = 0; //количество популяций с одинаковым максимальным значением функции приспособленности

                    tab_control.SelectedTab = result_page; //переход на вкладку "Результат"
                    tab_control.Refresh(); //обновление элемента с вкладками

                    for (;;) //бесконечный цикл
                    {

                        current_population_count++; //увеличение номера популяции

                        tb_population_number.Text = current_population_count.ToString(); //вывод номера популяции в текстовое поле
                        tb_population_number.Refresh();

                        current_best_chromosome = null; //сброс лучшего генома в популяции

                        //для каждого генома популяции производим расчет функций приспособленности каждой хромосомы генома и общей функции приспособленности
                        foreach (Population pop in current_pop)
                        {
                            foreach (Project_chromosome chrom in pop.Chromosome)
                            {
                                chrom.Calculate_fitness_function(projects_weight.ElementAt(chrom.ID - 1), employees_weight);
                            }
                            pop.Calculate_Fitness_function();

                            //по необходимости корректируем значение лучшего генома в популяции
                            if (current_best_chromosome == null) current_best_chromosome = pop;
                            else
                            if (pop.Fitness_function > current_best_chromosome.Fitness_function)
                                current_best_chromosome = pop;
                        }


                        greatest_ff.Add(current_best_chromosome.Fitness_function);  //занесение в массив точек значение лучше    функции приспособленности на шаге

                        //если заданный предел популяций достигнут, то лучший геном популяции становится глобальным и цикл завершается
                        if (current_population_count == max_population_count)
                        {
                            global_best_chromosome = current_best_chromosome;
                            break;
                        }

                        else //иначе
                        {
                            //сравниваем значение глобального генома и локального
                            //если функции приспособленности совпадают более 10 шагов, то лучший геном популяции становится глобальным и цикл завершается
                            if (global_best_chromosome == null) global_best_chromosome = current_best_chromosome;
                            if (global_best_chromosome.Fitness_function == current_best_chromosome.Fitness_function)
                            {
                                ++iter_counter;
                                if (iter_counter > Int32.Parse(nUD_popul_count.Value.ToString()) / 100)
                                {
                                    break;
                                }
                            }
                            else //иначе
                            {
                                //глобальный геном становится равным локальному
                                global_best_chromosome = current_best_chromosome;
                                iter_counter = 0;
                            }
                        }

                        tb_func.Text = global_best_chromosome.Fitness_function.ToString();
                        tb_func.Refresh();

                        List<Population> parent_pool; //массив родительских геномов

                        //в зависимости от выбранного метода селекции формируется родительский пул
                        if (rb_roulette.Checked)
                        {
                            parent_pool = GA.Selection_roulette(current_pop, Int32.Parse(nUD_chrom_count.Value.ToString()), rnd);
                        }
                        else
                            if (rb_tournier.Checked)
                        {
                            parent_pool = GA.Selection_tournirs(current_pop, Int32.Parse(nUD_chrom_count.Value.ToString()), Int32.Parse(nUD_group.Value.ToString()), rnd);

                        }
                        else
                        {
                            parent_pool = GA.Selection_rang(current_pop, Int32.Parse(nUD_chrom_count.Value.ToString()), rnd);
                        }

                        current_pop = new List<Population>();
                        //формирование новой популяции из хромосом родительского пула
                        current_pop = GA.Formation_new_population(parent_pool, empl_id, Int32.Parse(nUD_chrom_count.Value.ToString()), Int32.Parse(nUD_max_empl.Value.ToString()), Double.Parse(tb_conversion.Text) / (double)100, Double.Parse(tb_mutation.Text) / (double)100, rnd);

                    }

                    //если количество графиков достигло 3, то происходит очистка области отображения графика и массива точек для графиков
                    if (points_for_graphics.Count >= Int32.Parse(nUD_graphics.Value.ToString()))
                    {
                        while (points_for_graphics.Count >= Int32.Parse(nUD_graphics.Value.ToString()))
                        {
                            points_for_graphics.RemoveAt(0);
                            graphics_names.RemoveAt(0);
                        }
                       // points_for_graphics.Clear();
                       /* chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();
                        chart1.Series[2].Points.Clear();

                        chart1.Series[0].Color = Color.White;
                        chart1.Series[1].Color = Color.White;
                        chart1.Series[2].Color = Color.White;

                        chart1.Series[0].Name = " ";
                        chart1.Series[1].Name = "  ";
                        chart1.Series[2].Name = "   ";*/


                    }

                    chart1.Series.Clear();
                    //добавление в массив точек нового набора точек
                    points_for_graphics.Add(greatest_ff);

                    //определение количества колонок в таблице результата
                    int column_count = 2;

                    foreach (Project_chromosome chr in global_best_chromosome.Chromosome)
                    {
                        if (chr.Get_count_employees() + 1 > column_count)
                            column_count = chr.Get_count_employees() + 1;
                    }

                    //очистка таблицы результата
                    while (dgv_result.Rows.Count > 0)
                    {
                        dgv_result.Rows.RemoveAt(0);
                    }

                    //задание параметров таблицы результата
                    dgv_result.ColumnCount = 2 * column_count;

                    int i = 0;

                    while (i < (2 * column_count))
                    {
                        if (i == 2 * column_count - 2)
                            dgv_result.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        else
                            dgv_result.Columns[i].Width = dgv_result.Width / column_count;

                        dgv_result.Columns[i + 1].Width = 0;
                        dgv_result.Columns[i + 1].Visible = false;

                        if (i == 0) dgv_result.Columns[i].Name = "Проект";
                        else dgv_result.Columns[i].Name = "Сотрудник";


                        i += 2;
                    }


                    dgv_result.RowHeadersVisible = false;
                    dgv_result.ColumnHeadersVisible = false;
                    dgv_result.AllowUserToAddRows = false;
                    dgv_result.AllowUserToDeleteRows = false;
                    dgv_result.AllowUserToResizeColumns = false;
                    dgv_result.AllowUserToResizeRows = false;
                    dgv_result.ReadOnly = true;
                    dgv_result.Font = new Font("Modern No. 20", 12, FontStyle.Regular);

                    //заполнение таблицы результата названиеями проектов и именами сотрудников
                    foreach (Project_chromosome chr in global_best_chromosome.Chromosome)
                    {
                        dgv_result.Rows.Add();
                        string project_name = "";

                        foreach (Project pr in projects)
                        {
                            if (pr.ID == chr.ID)
                            {
                                project_name = pr.Name;
                                break;
                            }
                        }

                        dgv_result[0, dgv_result.Rows.Count - 1].Value = project_name;
                        dgv_result[1, dgv_result.Rows.Count - 1].Value = chr.ID.ToString();

                        int empl_counter = 0;

                        foreach (int empl in chr.Employees)
                        {
                            string employee_name = "";
                            foreach (Employee emp in employees)
                            {
                                if (emp.ID == empl)
                                {
                                    employee_name = emp.Name;
                                    break;
                                }
                            }

                            dgv_result[empl_counter * 2 + 2, dgv_result.Rows.Count - 1].Value = employee_name;
                            dgv_result[empl_counter * 2 + 3, dgv_result.Rows.Count - 1].Value = empl.ToString();

                            ++empl_counter;
                        }


                    }

                    //масштабирование осей графика
                    int max_ff = 0;// points_for_graphics[points_for_graphics.Count - 1].Max();
                    int min_ff = points_for_graphics[0].Min(); // points_for_graphics[points_for_graphics.Count - 1].Min();
                    int max_x = 1;
                    foreach(List<int> pop in points_for_graphics)
                    {
                        if (pop.Max() > max_ff)
                        {
                            max_ff = pop.Max();
                        }

                        if (pop.Min() < min_ff)
                        {
                            min_ff = pop.Min();
                        }

                        if (pop.Count > max_x)
                            max_x = pop.Count;
                    }

                    
                        chart1.ChartAreas[0].AxisY.Minimum = min_ff-100;
                   
                        chart1.ChartAreas[0].AxisY.Maximum = max_ff + 100;

                    chart1.ChartAreas[0].AxisX.Minimum = 1;

                   
                        chart1.ChartAreas[0].AxisX.Maximum = max_x;


                    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(1, chart1.ChartAreas[0].AxisX.Maximum);
                    chart1.ChartAreas[0].AxisY.ScaleView.Zoom(chart1.ChartAreas[0].AxisY.Minimum, chart1.ChartAreas[0].AxisY.Maximum);

                    int counter = 1;
                    int numeric = 0;

                    foreach (List<int> pop in points_for_graphics)
                    {
                        counter = 1;

                        if (graphics_names.Count > numeric)
                        chart1.Series.Add(graphics_names[numeric]);
                        else
                            chart1.Series.Add(" ");

                        chart1.Series[numeric].BorderWidth = 2;
                        chart1.Series[numeric].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                        //добавление точек для построения графика
                        foreach (int ff in pop)
                        {
                            chart1.Series[numeric].Points.AddXY(counter, ff);
                            ++counter;
                        }
                        chart1.Series[numeric].Color = Get_graphic_color(numeric);
                        ++numeric;
                    }

                   

                           // chart1.Series[points_for_graphics.Count - 1].Color = col;

                    //установка флага срабатывания алгоритма
                    flag_work_alg = true;

                }
                else //иначе
                {
                    //окно с сообщением
                    MessageBox.Show("Количество сотрудников должно совпадать или превышать количеств проектов");
                }
            }
        }

        private Color Get_graphic_color(int num)
        {
            //выбор цвета графика
            Color col = new Color();

            switch (num)//цвет сектора
            {
                case 0:
                    col = Color.Red;
                    break;
                case 1:
                    col = Color.Green;
                    break;
                case 2:
                    col = Color.Blue;
                    break;
                case 3:
                    col = Color.Yellow;
                    break;
                case 4:
                    col = Color.Black;
                    break;
                case 5:
                    col = Color.Chocolate;
                    break;
                case 6:
                    col = Color.Orange;

                    break;
                case 7:
                    col = Color.Purple;
                    break;
                case 8:
                    col = Color.Red;
                    break;
            }

            return col;
        }
        private void rb_tournier_CheckedChanged(object sender, EventArgs e) //выбор турнирного метода селекции
        {
            if (rb_tournier.Checked) nUD_group.Enabled = true; else nUD_group.Enabled = false; //если выбран турнирный метод селекции, то можно выбрать длину групп
        }

        private void tab_control_Selected(object sender, TabControlEventArgs e) //смена вкладки
        {
            //задаётся активность/неактивность подпунктов меню в зависимости от выбранной вкладки
            switch (tab_control.SelectedTab.Name) 
            {
                case "start_page":
                    menu_pr.Enabled = true;
                    menu_empl.Enabled = true;
                    menu_load.Enabled = true;
                    menu_save.Enabled = true;
                    save_prop.Enabled = true;
                    save_result.Enabled = false;
                    save_graphic.Enabled = false;
                    break;
                case "result_page":
                    menu_pr.Enabled = false;
                    menu_empl.Enabled = false;
                    menu_load.Enabled = false;
                    menu_save.Enabled = false;
                    save_prop.Enabled = false;
                    save_result.Enabled = true;
                    save_graphic.Enabled = false;
                    break;
                case "statistic_page":
                    menu_pr.Enabled = false;
                    menu_empl.Enabled = false;
                    menu_load.Enabled = false;
                    menu_save.Enabled = false;
                    save_prop.Enabled = false;
                    save_result.Enabled = false;
                    save_graphic.Enabled = true;

                    //если отработал алгоритм и выполняется переход на вкладку статистики, то можно задать имя графика

                    if (flag_work_alg) 
                    {
                        Graphic_name form = new Graphic_name(points_for_graphics.Count);
                        form.ShowDialog();
                        graphics_names.Add(form.Get_graphic_name());
                        chart1.Series[points_for_graphics.Count - 1].Name = form.Get_graphic_name();
                        flag_work_alg = false;
                    }
                    break;
            }
        }

        private void save_graphic_Click(object sender, EventArgs e) //сохранение графика
        {
            saveFileDialog1.Filter = "Изображение |*.jpeg";
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(saveFileDialog1.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
            }
        }

        private void save_result_Click(object sender, EventArgs e) //сохранение результата
            //формируется xml-файл стурктуры, описанной в приложении
        {
            if (global_best_chromosome == null) return;
            saveFileDialog1.FileName = "";

            saveFileDialog1.Filter = "XML-файл |*.xml";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xml_doc = new XmlDocument();

                //if (!File.Exists(saveFileDialog1.FileName))

                // {
                XmlTextWriter textWritter = new XmlTextWriter(saveFileDialog1.FileName, System.Text.Encoding.UTF8);

                textWritter.WriteStartDocument();

                textWritter.WriteStartElement("root");

                textWritter.WriteEndElement();

                textWritter.Close();
                // }




                //xml_doc.Load(saveFileDialog1.FileName);

                /*if (File.Exists(saveFileDialog1.FileName))
                    xml_doc.Save(saveFileDialog1.FileName);*/

                xml_doc.Load(saveFileDialog1.FileName);

                XmlElement root = xml_doc.DocumentElement;

                foreach (Project_chromosome pop in global_best_chromosome.Chromosome)
                {
                    foreach (Project pr in projects)
                    {
                        if (pr.ID == pop.ID)
                        {
                            XmlElement project = xml_doc.CreateElement("project");

                            XmlAttribute id = xml_doc.CreateAttribute("id");
                            XmlText text_id = xml_doc.CreateTextNode(pr.ID.ToString());

                            id.AppendChild(text_id);

                            XmlAttribute pr_name = xml_doc.CreateAttribute("name");
                            XmlText text_name = xml_doc.CreateTextNode(pr.Name);

                            pr_name.AppendChild(text_name);

                            project.Attributes.Append(id);
                            project.Attributes.Append(pr_name);

                            if (pr.Get_all_attrib().Count > 0)
                            {
                                XmlElement attr_list = xml_doc.CreateElement("attr-list");

                                foreach (KeyValuePair<KeyValuePair<string, int>, string> kvp in pr.Get_all_attrib())
                                {
                                    XmlElement attr = xml_doc.CreateElement("attr");

                                    XmlAttribute weight = xml_doc.CreateAttribute("weight");
                                    XmlText text_weight = xml_doc.CreateTextNode(kvp.Key.Value.ToString());

                                    XmlAttribute attr_name = xml_doc.CreateAttribute("name");
                                    XmlText text_attr_name = xml_doc.CreateTextNode(kvp.Key.Key);

                                    XmlAttribute attr_value = xml_doc.CreateAttribute("value");
                                    XmlText text_attr_value = xml_doc.CreateTextNode(kvp.Value);

                                    weight.AppendChild(text_weight);
                                    attr_name.AppendChild(text_attr_name);
                                    attr_value.AppendChild(text_attr_value);

                                    attr.Attributes.Append(weight);
                                    attr.Attributes.Append(attr_name);
                                    attr.Attributes.Append(attr_value);

                                    attr_list.AppendChild(attr);
                                }

                                project.AppendChild(attr_list);
                            }

                            XmlElement empl = xml_doc.CreateElement("employees");

                            foreach (int ids in pop.Employees)
                            {
                                foreach (Employee em in employees)
                                {
                                    if (em.ID == ids)
                                    {
                                        XmlElement employee = xml_doc.CreateElement("employee");
                                        XmlAttribute em_id = xml_doc.CreateAttribute("id");
                                        XmlText em_text_id = xml_doc.CreateTextNode(em.ID.ToString());

                                        XmlAttribute em_name = xml_doc.CreateAttribute("name");
                                        XmlText em_text_name = xml_doc.CreateTextNode(em.Name);

                                        XmlAttribute em_age = xml_doc.CreateAttribute("age");
                                        XmlText em_text_age = xml_doc.CreateTextNode(em.Age.ToString());

                                        XmlAttribute em_exp = xml_doc.CreateAttribute("experience");
                                        XmlText em_text_exp = xml_doc.CreateTextNode(em.Experience.ToString());

                                        em_id.AppendChild(em_text_id);
                                        em_name.AppendChild(em_text_name);
                                        em_age.AppendChild(em_text_age);
                                        em_exp.AppendChild(em_text_exp);

                                        employee.Attributes.Append(em_id);
                                        employee.Attributes.Append(em_name);
                                        employee.Attributes.Append(em_age);
                                        employee.Attributes.Append(em_exp);

                                        if (em.Get_all_attrib().Count > 0)
                                        {
                                            XmlElement attr_list = xml_doc.CreateElement("attr-list");

                                            foreach (KeyValuePair<KeyValuePair<string, int>, string> kvp in em.Get_all_attrib())
                                            {
                                                XmlElement attr = xml_doc.CreateElement("attr");

                                                XmlAttribute weight = xml_doc.CreateAttribute("weight");
                                                XmlText text_weight = xml_doc.CreateTextNode(kvp.Key.Value.ToString());

                                                XmlAttribute attr_name = xml_doc.CreateAttribute("name");
                                                XmlText text_attr_name = xml_doc.CreateTextNode(kvp.Key.Key);

                                                XmlAttribute attr_value = xml_doc.CreateAttribute("value");
                                                XmlText text_attr_value = xml_doc.CreateTextNode(kvp.Value);

                                                weight.AppendChild(text_weight);
                                                attr_name.AppendChild(text_attr_name);
                                                attr_value.AppendChild(text_attr_value);

                                                attr.Attributes.Append(weight);
                                                attr.Attributes.Append(attr_name);
                                                attr.Attributes.Append(attr_value);

                                                attr_list.AppendChild(attr);
                                            }

                                            employee.AppendChild(attr_list);

                                        }

                                        empl.AppendChild(employee);
                                        break;
                                    }
                                }

                            }
                            project.AppendChild(empl);
                            root.AppendChild(project);
                            break;
                        }
                    }               

                }
                xml_doc.Save(saveFileDialog1.FileName); xml_doc.Save(saveFileDialog1.FileName);
            }

        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }

        private void tb_mutation_TextChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(tb_mutation.Text) + Int32.Parse(tb_conversion.Text) > 100)
            {
              
                MessageBox.Show("Суммарная вероятность мутации и скрещивания не может быть больше 100");
                tb_mutation.Text = (100 - Int32.Parse(tb_conversion.Text)).ToString();
            }
        }

        private void tb_conversion_TextChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(tb_mutation.Text) + Int32.Parse(tb_conversion.Text) > 100)
            {

                MessageBox.Show("Суммарная вероятность мутации и скрещивания не может быть больше 100");
                tb_conversion.Text = (100 - Int32.Parse(tb_mutation.Text)).ToString();
            }
        }
    }
}
