using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlg
{
    public class Project_chromosome
    {
        int id; //идентификатор проекта
        List<int> employees = new List<int>();  //список идентификаторов сотрудников
        int function = new int();   //оценочная функция


        public Project_chromosome() //конструктор без параметров
        {        
            function = 0;
            id = 0;
        }

        public Project_chromosome(int id)   //конструктор с передаваемым идентификатором проекта
        {           
            function = 0;
            this.id = id;
        }

        public Project_chromosome(Project_chromosome ga)    //конструктор с передаваемым объектом класса
        {
            this.employees = ga.employees;
            this.function = ga.function;
            this.id = ga.id;
        }

        public List<int> Employees //свойство возврата и задания списка сотрудников
        {
            get
            {
                return employees;
            }
            set
            {
                this.employees = value;
            }
        }

        public int Function //свойство возврата и задания оценочной функции
        {
            get
            {
                return function;
            }
            set
            {
                this.function = value;
            }
        }

        public int ID   //свойство возврата и задания идентификатора проекта
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }

        public void Add_employees(int employee) //добавление сотрудника в список
        {
            this.employees.Add(employee);
        }

        public int Get_count_employees()    //возврат числа сотрудников в списке
                                            //выходные данные - количество сотрудников
        {
            return employees.Count;
        }

        public static int Calculate_sum_weight(Dictionary<string, string> attributes, Dictionary<string, int> attributes_weight, Dictionary<string, Dictionary<int, string>> values_weight) //подсчет суммарного веса
                                                                                                                                                                                            //входные данные - массив атрибутов и их значений, массив атрибутов и их весов, массив атрибутов и их ранжированных значений
                                                                                                                                                                                            //выходные данные - выходной коэффициент
        {
            //W = сумма по всем атрибутам объекта (вес атрибута * вес значения)
            int sum_weight = 0;

            foreach (KeyValuePair<string, string> attr in attributes)
            {
                int a_weight = attributes_weight[attr.Key];
                int v_weight = 0;
                foreach (KeyValuePair<int, string> pair in values_weight[attr.Key])
                {
                    if (pair.Value == attr.Value)
                    {
                        v_weight = pair.Key;
                        break;
                    }
                }

                sum_weight += a_weight * v_weight;
            }

            return sum_weight;
        }

        public void Calculate_fitness_function(int projects_weight, List<int> employees_weight) //расчет оценочной функции
                                                                                                //входные данные - вес проекта и список весов сотрудников
        {
            //если вес проекта меньше или равен весу сотрудника, то их веса перемножаются и прибавляются к оценочной функции
            int fitness_function = 0;

            for (int i = 0; i < this.employees.Count; i++)
            {
                if (projects_weight <= employees_weight[employees[i] - 1])
                {
                    fitness_function += projects_weight * employees_weight[employees[i] - 1];
                }
            }

            this.function = fitness_function;
        }



    }
}
