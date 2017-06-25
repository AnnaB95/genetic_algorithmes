using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlg
{
    public class GA
    {
        public static List<Genotip> Start_population(List<int> projects, List<int> employees, int max_count_employees, int genotip_count, Random rnd) //формирование стартовой популяции
                                                                                                                                                            //входные данные - массив идентификаторов проектов, массив идентификаторов сотрудников, максимальное число сотрудников на проект, число хромосом, рандомайзер
                                                                                                                                                            //выходные данные - массив генотипов (стартовая популяция)
        {
          
            List<int> empl_st = new List<int>();    //список сотрудников
            List<int> empl_temp = new List<int>();  //временный список сотрудников

            List<int> project_st = new List<int>(); //список проектов
            List<int> project_temp = new List<int>(); //временный список проектов

            List<Genotip> start_pop = new List<Genotip>();    //стратовая популяция


            foreach (int employee in employees) //копирование идентификаторов сотрудников в список сотрудников
            {
                int j = employee;
                empl_st.Add(j);
            }

            foreach(int project in projects)    //копирование идентификаторов проектов в список проектов
            {
                int j = project;
                project_st.Add(j);              
            }

            while(start_pop.Count < genotip_count)   //пока количество генотипов в стратовой популяции не достигло заданного
            {              

                project_temp.Clear();   //очистка временного массива проектов

                foreach (int proj in project_st) //заполнение временного массива проектов
                {
                    int i = proj;
                    project_temp.Add(i);
                }
                List<Project_chromosome> pr_chrom = new List<Project_chromosome>(); //массив хромосом

                empl_temp.Clear();  //очистка временного массива сотрудников

                foreach (int empl in empl_st)   //заполнение временного массива сотрудников
                {
                    int i = empl;
                    empl_temp.Add(i);
                }

                while (pr_chrom.Count < projects.Count) //для всех проектов сначала выбираем случайный проект из списка, затем распределяем для него сотрудника со случайным номером
                {
                    Project_chromosome chromosome = new Project_chromosome();
                    int proj_id = rnd.Next(project_temp.Count);
                    chromosome.ID = project_temp.ElementAt(proj_id);
                    project_temp.RemoveAt(proj_id);

                  
                    int num = rnd.Next(empl_temp.Count);
                    chromosome.Add_employees(empl_temp.ElementAt(num));
                    empl_temp.RemoveAt(num);

                    pr_chrom.Add(chromosome);
                }

                List<int> max_empl_in_project = new List<int>();   //число проектов с максимально возможным числом сотрудников 

                while (empl_temp.Count > 0 & max_empl_in_project.Count < projects.Count)  //пока не опустошиться список сотрудников или все проекты достигнут максимально возможного числа сотрудников
                {
                    //выбор случайного сотрудника
                    int num = rnd.Next(empl_temp.Count);
                    int employee = empl_temp.ElementAt(num);
                   

                    bool flag = false;
                    bool del_flag = true;

                    while (flag == false)
                    {
                        
                        foreach (Project_chromosome pr in pr_chrom) //проход по проектам и добавление сотрудника в соответствии с случайной выборкой
                        {
                            int ch = rnd.Next(2);

                            if (ch == 1)
                            {
                                if (pr.Get_count_employees() < max_count_employees) //если число сотрудников меньше максимально допустимого, то добавляем в проект
                                {
                                    pr.Add_employees(employee);
                                    flag = true;
                                    break;
                                }
                                else
                                if (pr.Get_count_employees() == max_count_employees)
                                {
                                   if (max_empl_in_project.IndexOf(pr.ID) == -1)
                                    {
                                        max_empl_in_project.Add(pr.ID);
                                    }
                                    if (max_empl_in_project.Count == projects.Count)
                                    {
                                        flag = true;
                                        del_flag = false;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    pr_chrom = InsertionSort(pr_chrom); //сортировка проектов по возрастанию количсества сотрудников

                    if (del_flag) 
                    empl_temp.RemoveAt(num);
                }

                start_pop.Add(new Genotip(pr_chrom)); //добавление хромосомы
            }

            return start_pop;
        }

        private static Genotip Mutation(Genotip parent, List<int> employees, int max_count_employees, Random rnd) //мутация
                                                                                                                        //входные данные - генотип из родительского пула, список идентификаторов сотрудников, максимальное число сотрудников на проект, рандомайзер
                                                                                                                        //выходные данные - генотип
        {
            //мутация заключается в формировании новой хромосомы. 
            //формирвоание аналогично формированию хромосом для стартовой популяции
            List<int> empl_temp = new List<int>();


            List<int> prog_id = new List<int>();

            foreach (Project_chromosome pr in parent.Chromosome)
            {
                prog_id.Add(pr.ID);
            }

            List<Project_chromosome> pr_chrom = new List<Project_chromosome>();

            foreach (int em in employees)
            {
                int i = em;
                empl_temp.Add(i);
            }

            // foreach (Project_chromosome pr in parent.Chromosome)
            while (pr_chrom.Count < parent.Chromosome.Count)
            {
                Project_chromosome mut_pr = new Project_chromosome();
                int proj_id = rnd.Next(prog_id.Count);
                mut_pr.ID = prog_id.ElementAt(proj_id);
                prog_id.RemoveAt(proj_id);

                int num = rnd.Next(empl_temp.Count);
                mut_pr.Add_employees(empl_temp.ElementAt(num));
                empl_temp.RemoveAt(num);

                pr_chrom.Add(mut_pr);
            }


            List<int> max_empl_in_project = new List<int>();

            while (empl_temp.Count > 0 & max_empl_in_project.Count < parent.Get_chromosome_count())
                {
                    int num = rnd.Next(empl_temp.Count);
                    int employee = empl_temp.ElementAt(num);


                    bool flag = false;
                bool del_flag = true;

                    while (flag == false)
                    {

                        foreach (Project_chromosome pr in pr_chrom)
                        {
                            int ch = rnd.Next(2);

                            if (ch == 1)
                            {
                                if (pr.Get_count_employees() < max_count_employees)
                                {
                                    pr.Add_employees(employee);
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (max_empl_in_project.IndexOf(pr.ID) == -1)
                                {
                                    max_empl_in_project.Add(pr.ID);
                                }
                                    if (max_empl_in_project.Count == parent.Get_chromosome_count())
                                    {
                                        flag = true;
                                    del_flag = false;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    pr_chrom = InsertionSort(pr_chrom);

                if (del_flag)
                    empl_temp.RemoveAt(num);
                }


            Genotip mut_individ = new Genotip(pr_chrom);

            return mut_individ;
        }

        private static List<Genotip> Crossengover(Genotip parent_one, Genotip parent_two, List<int> employees, int max_count_employees, Random rnd) //равномерное скрещивание
                                                                                                                                     //входные данные - генотип из родительского пула 1, генотип из родительского пула 2, список идентификаторов сотрудников, максимальное число сотрудников на проект, рандомайзер
                                                                                                                                     //выходные данные - массив генотипов
        {
            List<int> empl1 = new List<int>();  //массив сотрудников 1 потомка
            List<int> empl2 = new List<int>();  //массив сотрудников 2 потомка

            List<int> none_empl_on1 = new List<int>();
            List<int> none_empl_on2 = new List<int>();

            int sum_par_one = 0;
            int sum_par_two = 0;

            foreach (int em in employees) //заполнение массивов
            {
                int i = em;
                int j = em;
                empl1.Add(i);
                empl2.Add(j);
            }

          
            List<Genotip> new_individs = new List<Genotip>(); //массив дочерних генотипов

            Genotip child_one = new Genotip();  //потомок 1
            Genotip child_two = new Genotip();  //потомок 2

            for (int i = 0; i < parent_one.Get_chromosome_count(); ++i) //для всех проектов в хромосоме  выбирается информация о соответствующем проекте из обеих хромосом
            {
                int proj_id = parent_one.Chromosome[i].ID;

                Project_chromosome par1 = parent_one.Chromosome[i];

                Project_chromosome par2 = new Project_chromosome();

                foreach (Project_chromosome pr in parent_two.Chromosome)
                {
                    if (pr.ID == proj_id)
                    {
                        par2 = pr;
                        break;
                    }
                }

               /* count_empl_on_par1.Add(par1.Get_count_employees());
                count_empl_on_par2.Add(par2.Get_count_employees());*/

                sum_par_one += par1.Get_count_employees();  //подсчёт суммарного числа сотрудников в родителе 1
                sum_par_two += par2.Get_count_employees();  //подсчёт суммарного числа сотрудников в родителе 2

                int max_count = (par1.Get_count_employees() > par2.Get_count_employees()) ? par1.Get_count_employees() : par2.Get_count_employees();    //максимум - максимальная длина цепочки сотрудников в проекте из 2 хромсом

                Project_chromosome ch1 = new Project_chromosome();
                ch1.ID = proj_id;
                Project_chromosome ch2 = new Project_chromosome();
                ch2.ID = proj_id;

                for (int j = 0; j < max_count; ++j)
                {
                    int randomize = rnd.Next(0, 2);
                    //случайное число показывает порядок копирвоания: если 1 - то из 1 родителя в 1 потомка, из 2 родителя во 2 потомка, 0 - наоборот
                   // int rand2 = randomize;
                   //если найдена попытка повторного добавления одного и того же сотрудника в потомка, то идентификатор этого сотрудника заносится в список сотрудников первого порядка для другого потомка                                       
                    if (randomize == 1)
                    {

                        if (j < par1.Get_count_employees())
                        {

                            if (empl1.IndexOf(par1.Employees[j]) != -1)
                            {
                                ch1.Add_employees(par1.Employees[j]);
                                empl1.Remove(par1.Employees[j]);
                            }
                            else
                                none_empl_on2.Add(par1.Employees[j]);
                        }


                        if (j < par2.Get_count_employees())
                        {

                            if (empl2.IndexOf(par2.Employees[j]) != -1)
                            {
                                ch2.Add_employees(par2.Employees[j]);
                                empl2.Remove(par2.Employees[j]);
                            }
                            else

                                none_empl_on1.Add(par2.Employees[j]);
                        }
                    }
                    else
                    {
                        if (j < par2.Get_count_employees())
                        {

                            if (empl1.IndexOf(par2.Employees[j]) != -1)
                            {
                                ch1.Add_employees(par2.Employees[j]);
                                empl1.Remove(par2.Employees[j]);

                            }
                            else
                                none_empl_on2.Add(par2.Employees[j]);
                        }


                        if (j < par1.Get_count_employees())
                        {

                           if (empl2.IndexOf(par1.Employees[j]) != -1)
                            {
                                ch2.Add_employees(par1.Employees[j]);
                                empl2.Remove(par1.Employees[j]);
                            }
                           else
                                none_empl_on1.Add(par1.Employees[j]);

                        }
                    }
                }
             
                child_one.Add_chromosome(ch1);
                child_two.Add_chromosome(ch2);

                List<int> max_empl_in_project = new List<int>();

                //если массивы предположительных сотрудников для потомков не пусты, то производится занесение сотрудников в соответствующего потомка, при нехватке в нём сотрудников 
                //распределение сотрудников по проектам производится аналогично способу, используемому при формировании стартового поколения

                if (none_empl_on1.Count > 0) child_one.Chromosome = InsertionSort(child_one.Chromosome);

                while (none_empl_on1.Count > 0 & max_empl_in_project.Count < child_one.Get_chromosome_count())
                {
                    int num = rnd.Next(none_empl_on1.Count);
                    int employee = none_empl_on1.ElementAt(num);

                    

                    bool flag = false;
                    bool del_flag = true;

                    while (flag == false)
                    {
                        if (empl1.IndexOf(employee) == -1)
                            break;

                            foreach (Project_chromosome pr in child_one.Chromosome)
                        {
                            int ch = rnd.Next(2);

                            if (ch == 1)
                            {
                                if (pr.Get_count_employees() < max_count_employees)
                                {
                                    pr.Add_employees(employee);
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (max_empl_in_project.IndexOf(pr.ID) == -1)
                                    {
                                        max_empl_in_project.Add(pr.ID);
                                        if (max_empl_in_project.Count == child_one.Get_chromosome_count())
                                        {
                                            flag = true;
                                            del_flag = false;
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                    }

                    child_one.Chromosome = InsertionSort(child_one.Chromosome);

                    if (del_flag)
                    {
                        none_empl_on1.RemoveAt(num);
                        empl1.Remove(employee);
                    }
                }

                max_empl_in_project.Clear();

                if (none_empl_on2.Count > 0) child_two.Chromosome = InsertionSort(child_two.Chromosome);

                while (none_empl_on2.Count > 0 & max_empl_in_project.Count < child_two.Get_chromosome_count())
                {
                    int num = rnd.Next(none_empl_on2.Count);
                    int employee = none_empl_on2.ElementAt(num);


                    bool flag = false;
                    bool del_flag = true;

                    while (flag == false)
                    {
                        if (empl2.IndexOf(employee) == -1)
                            break;

                        foreach (Project_chromosome pr in child_two.Chromosome)
                        {
                            int ch = rnd.Next(2);

                            if (ch == 1)
                            {
                                if (pr.Get_count_employees() < max_count_employees)
                                {
                                    pr.Add_employees(employee);
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (max_empl_in_project.IndexOf(pr.ID) == -1)
                                    {
                                        max_empl_in_project.Add(pr.ID);
                                        if (max_empl_in_project.Count == child_two.Get_chromosome_count())
                                        {
                                            flag = true;
                                            del_flag = false;
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                    }

                    child_two.Chromosome = InsertionSort(child_two.Chromosome);

                    if (del_flag)
                    {
                        none_empl_on2.RemoveAt(num);
                        empl2.Remove(employee);
                    }
                }



            }

            int sum_child_one = 0;
            int sum_child_two = 0;

            //поиск числа сотрудников в генотипах потомков
            foreach(Project_chromosome c1 in child_one.Chromosome)
            {
                sum_child_one += c1.Get_count_employees();
            }

            foreach(Project_chromosome c2 in child_two.Chromosome)
            {
                sum_child_two += c2.Get_count_employees();
            }

            //если суммы не совпадают, то дополняем потомков из основного массива для сотрудников

            if (sum_par_one != sum_child_one)
            {
                List<int> max_empl_in_project = new List<int>();

                if (empl1.Count > 0) child_one.Chromosome = InsertionSort(child_one.Chromosome);

                while (empl1.Count > 0 & max_empl_in_project.Count < child_one.Get_chromosome_count())
                {
                    int num = rnd.Next(empl1.Count);
                    int employee = empl1.ElementAt(num);


                    bool flag = false;
                    bool del_flag = true;

                    while (flag == false)
                    {

                        foreach (Project_chromosome pr in child_one.Chromosome)
                        {
                            int ch = rnd.Next(2);

                            if (ch == 1)
                            {
                                if (pr.Get_count_employees() < max_count_employees)
                                {
                                    pr.Add_employees(employee);
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (max_empl_in_project.IndexOf(pr.ID) == -1)
                                    {
                                        max_empl_in_project.Add(pr.ID);
                                        if (max_empl_in_project.Count == child_one.Get_chromosome_count())
                                        {
                                            flag = true;
                                            del_flag = false;
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                    }

                    child_one.Chromosome = InsertionSort(child_one.Chromosome);

                    if (del_flag)
                        empl1.RemoveAt(num);
                }
            }

            if (sum_child_two != sum_par_two)
            {
               List<int> max_empl_in_project = new List<int>();

                if (empl2.Count > 0) child_two.Chromosome = InsertionSort(child_two.Chromosome);

                while (empl2.Count > 0 & max_empl_in_project.Count < child_two.Get_chromosome_count())
                {
                    int num = rnd.Next(empl2.Count);
                    int employee = empl2.ElementAt(num);


                    bool flag = false;
                    bool del_flag = true;

                    while (flag == false)
                    {

                        foreach (Project_chromosome pr in child_two.Chromosome)
                        {
                            int ch = rnd.Next(2);

                            if (ch == 1)
                            {
                                if (pr.Get_count_employees() < max_count_employees)
                                {
                                    pr.Add_employees(employee);
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (max_empl_in_project.IndexOf(pr.ID) == -1)
                                    {
                                        max_empl_in_project.Add(pr.ID);
                                        if (max_empl_in_project.Count == child_two.Get_chromosome_count())
                                        {
                                            flag = true;
                                            del_flag = false;
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                    }

                    child_two.Chromosome = InsertionSort(child_two.Chromosome);

                    if (del_flag)
                        empl2.RemoveAt(num);
                }
            }

            new_individs.Add(child_one);
            new_individs.Add(child_two);

            return new_individs;
        }

        public static List<Genotip> Formation_new_population (List<Genotip> parent_pool, List<int> employees, int genotip_count,  int max_count_employees, double crossover_propbability, double mutation_probablity, Random rnd)  //формирование новой популяции
                                                                                                                                                                                                                                            //входные данные - массив генотипов (родительский пул), список идентификаторов сотрудников, число хромосом, максимальное число сотрудников на проект, вероятность скрещивания, вероятность мутации, рандомайзер
                                                                                                                                                                                                                                            //выходные данные - список генотипов (новая популяция) 
        {
            List<Genotip> new_population = new List<Genotip>();          

            int position = 0;
            int max = 0;
            int counter = 0;

            //поиск позиции элемента с максимальным значением функции приспособленности
            foreach(Genotip genom in parent_pool)
            {
                if (genom.Fitness_function > max)
                {
                    max = genom.Fitness_function;
                    position = counter;
                }
                ++counter;
            }
            //добавление в новую популяцию элемента с максимальным значением функции приспособленности
            new_population.Add(parent_pool.ElementAt(position));



           while (new_population.Count < genotip_count)
            {
                //формируется случайное десятичное число
                double probablity = rnd.NextDouble();
                //если оно меньше или равно проценту мутации, то в новую популяцию добавляется мутировавшая хромосома
                if (probablity <= mutation_probablity)
             // for (int i = 0; i < chromosome_count * mutation_probablity; ++i)
                {
                    if (new_population.Count < genotip_count)
                    {
                        int num = rnd.Next(genotip_count);
                        Genotip mut_pop = new Genotip();
                        mut_pop = parent_pool[num];

                        mut_pop = Mutation(mut_pop, employees, max_count_employees, rnd);

                        new_population.Add(mut_pop);
                    }
                    else
                        break;

                }
               else
                //добавляются 2 хромосомы с помощью скрещивания
               // for (int i = Convert.ToInt32(Math.Ceiling(chromosome_count * mutation_probablity)); i < chromosome_count * (mutation_probablity + crossover_propbability); ++i)
                if (probablity <= (crossover_propbability + mutation_probablity))
                {
               
                if (new_population.Count == genotip_count - 1) break; //continue;
                    if (new_population.Count < genotip_count)
                    {
                        Genotip parent_one = new Genotip();
                        Genotip parent_two = new Genotip();

                        int num = rnd.Next(genotip_count);
                        parent_one = parent_pool[num];
                        num = rnd.Next(genotip_count);
                        parent_two = parent_pool[num];

                        List<Genotip> crossingover_result = Crossengover(parent_one, parent_two, employees, max_count_employees, rnd);

                        for (int j = 0; j < crossingover_result.Count; ++j)
                        {
                            /* double prob = rnd.NextDouble();

                             if (prob <= mutation_probablity)
                                 crossingover_result[i] = Mutation(crossingover_result[i], rnd);*/

                            new_population.Add(crossingover_result[j]);

                        }
                    }
                    else
                        break;

                }
                //for (int i = Convert.ToInt32(Math.Ceiling(chromosome_count * (mutation_probablity + crossover_propbability))); i < chromosome_count; ++i)
              else
             {
             //копирование случайной хромосомы родительского пула
                int num = rnd.Next(genotip_count);
                Genotip clone = new Genotip();
                clone = parent_pool[num];

                new_population.Add(clone);               
                }
            }

            return new_population;
        }

        private static List<Project_chromosome> InsertionSort(List<Project_chromosome> pr_chrom)    //сортировка массива проектов по числу сотрудников по возрастанию
                                                                                                    //входные данные - массив хромосом
                                                                                                    //выходные данные - отсортированный массив хромосом
        {
            //сортировка вставками
            List<Project_chromosome> result = new List<Project_chromosome>();

            foreach(Project_chromosome proj in pr_chrom)
            {
                result.Add(proj);
            }

            for (int i = 0; i < pr_chrom.Count; i++)
            {
                int j = i;
                while (j > 0 && result[j - 1].Get_count_employees() > pr_chrom[i].Get_count_employees())
                {
                    result[j] = result[j - 1];
                    j--;
                }
                result[j] = pr_chrom[i];
            }
            return result;
        }

        private static Genotip Best_genotip(List<Genotip> population)
        {
            Genotip best = population[0];

            foreach(Genotip pop in population)
            {
                if (pop.Fitness_function > best.Fitness_function)
                    best = pop;
            }

            return best;
        }

        public static List<Genotip> Selection_roulette(List<Genotip> population, int genotip_count, Random rnd)    //селекция методом рулетки
                                                                                                                            //входные данные - массив генотипов (популяция), число генотипов, рандомайзер
                                                                                                                            //выходные данные - массив генотипов (родительский пул)
        {
            List<double> sector = new List<double>();

            int function_sum = 0;   //сумма всех функция приспособленности

            foreach(Genotip pop in population)
            {
                function_sum += pop.Fitness_function;
            }

            for (int i = 0; i < population.Count; i++)  //расчет секторов для каждого генотипа
            {
                if (i > 0)
                {
                    sector.Add(sector.ElementAt(i - 1) + (double)population.ElementAt(i).Fitness_function / (double)function_sum);
                }
                else
                {
                    sector.Add((double)population.ElementAt(i).Fitness_function / (double)function_sum);
                }
            }


            List<Genotip> parent_pool = new List<Genotip>();

            parent_pool.Add(Best_genotip(population));

            //выборка генотипов по принадлежности к выпавшему сектору
            for (int i = 0; i < genotip_count; ++i)
            {
                double number = rnd.NextDouble();
                int counter = 0;
                foreach (double sect in sector)
                {
                    if (number <= sect)
                    {
                        parent_pool.Add(population.ElementAt(counter));
                        break;
                    }
                    counter++;
                }
            }

            return parent_pool;
        }
      

        public static List<Genotip> Selection_tournirs(List<Genotip> population, int genotip_count, int group_length, Random rnd)  //селекция турнирным методом
                                                                                                                                            //входные данные - массив генотипов (популяция), число генотипов, количество элементов в группе, рандомайзер
                                                                                                                                            //выходные данные - массив генотипов (родительский пул)
        {
            int group_num;

            //установка размера группы
            if (group_length < 2)
            {
                group_num = 2;
            }
            else
            {
                group_num = group_length;
            }

            Genotip[] group = new Genotip[group_num];

            List<Genotip> parent_pool = new List<Genotip>();
            parent_pool.Add(Best_genotip(population));
            //выборка лучшего из группы
            for (int i = 0; i < genotip_count; ++i)
            {
                for (int j = 0; j < group_num; ++j)
                {
                    int num = rnd.Next(population.Count);
                    group[j] = population.ElementAt(num);
                }

                for (int j = 0; j < group.Length - 1; ++j)
                    for (int k = 0; k < group_length - j - 1; ++k)
                    {
                        if (group[k].Fitness_function < group[k + 1].Fitness_function)
                        {
                            Genotip temp = group[k];
                            group[k] = group[k + 1];
                            group[k + 1] = temp;
                        }
                    }

                parent_pool.Add(new Genotip(group[0]));


            }

            return parent_pool;
        }


        public static List<Genotip> Selection_rang(List<Genotip> population, int genotip_count, Random rnd)    //селекция ранговым методом
                                                                                                                        //входные данные - массив генотипов (популяция), число генотипов, рандомайзер
                                                                                                                        //выходные данные - массив генотипов (родительский пул)
        {
            List<Genotip> sort_population = new List<Genotip>();

            
            foreach (Genotip chrom in population)
            {
                sort_population.Add(chrom);
            }

            //сортировка генотипов в популяции по возрастанию функции приспособленности
            for (int j = 0; j < sort_population.Count - 1; ++j)
            {
                bool flag = false;
                for (int i = 0; i < sort_population.Count - j - 1; ++i)
                {
                    if (sort_population.ElementAt(i).Fitness_function > sort_population.ElementAt(i + 1).Fitness_function)
                    {
                        Genotip temp = sort_population.ElementAt(i);
                        sort_population[i] = sort_population.ElementAt(i + 1);
                        sort_population[i + 1] = temp;

                        flag = true;
                    }
                }
                if (!flag)
                {
                    break;
                }
            }

            List<double> ranges = new List<double>();

            List<Genotip> temporary = new List<Genotip>();

            int counter = 0;

            //назначение рангов
            //если функции приспособленности элементов одинаковые, то их ранг равен сумме их позиций / количество элементов
            //иначе ранг равен позиции элемента, начиная с 1
            for (int i = 0; i < sort_population.Count; ++i)
            {

                if (i != sort_population.Count - 1 && (sort_population[i].Fitness_function == sort_population[i + 1].Fitness_function))
                {
                    counter++;
                }
                else
                {
                    if (counter == 0)
                    {
                        ranges.Add(i + 1);
                    }
                    else
                    {
                        counter++;
                        double sum = 0;

                        for (int j = 0; j < counter; j++)
                        {
                            sum += ranges.Count + 1 + j;
                        }

                        double rang = sum / counter;

                        for (int j = 0; j < counter; j++)
                        {
                            ranges.Add(rang);
                        }
                        counter = 0;

                    }
                }
            }

            List<double> sector = new List<double>();

            double rang_sum = 0.0;

            //сумма рангов
            foreach (double rang in ranges)
            {
                rang_sum += rang;
            }

            //формирование секторов в соответствии с рангами
            for (int i = 0; i < population.Count; ++i)
            {
                if (i > 0)
                {
                    sector.Add(sector.ElementAt(i - 1) + (ranges[sort_population.IndexOf(population[i])]) / rang_sum);

                }
                else
                {
                    sector.Add((ranges[sort_population.IndexOf(population[i])]) / rang_sum);
                }
            }

            List<Genotip> parent_pool = new List<Genotip>();
            parent_pool.Add(Best_genotip(population));

            //выборка по принадлежности к сектору
            for (int i = 0; i < genotip_count; ++i)
            {
                double number = rnd.NextDouble();
                int count = 0;
                foreach (double sect in sector)
                {
                    if (number <= sect)
                    {
                        parent_pool.Add(population.ElementAt(count));
                        break;
                    }
                    count++;
                }
            }

            return parent_pool;


        }
    }
}
