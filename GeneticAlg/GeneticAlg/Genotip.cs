using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlg
{
    public class Genotip
    {
        List<Project_chromosome> chromosome;    //список хромосом
        int fitness_function;   //функция приспособленности

        public Genotip() //конструктор без параметров
        {
            this.chromosome = new List<Project_chromosome>();
            this.fitness_function = 0;
        }

        public Genotip(Genotip population)    //конструктор с передаваемым объектом класса
        {
            this.chromosome = population.chromosome;
            this.fitness_function = population.fitness_function;
        }

        public Genotip (List<Project_chromosome> chromosome)     //конструктор с передаваемым массивом хромосом
        {
            this.chromosome = chromosome;
            this.fitness_function = 0;
        }

        public int Get_chromosome_count()   //возврат числа хромосом
                                            //выходные данные - количества хромосом
        {
            return chromosome.Count;
        }

        public void Add_chromosome(Project_chromosome chromosome)   //добавление хромосомы
        {
            this.chromosome.Add(chromosome);
        }

        public List<Project_chromosome> Chromosome  //свойство возврата и изменения списка хромосом
        {
            get
            {
                return chromosome;
            }
            set
            {
                this.chromosome = value;
            }
        }

        public int Fitness_function //свойство возврата и изменения функции приспособленности                               
        {
            get
            {
                return fitness_function;
            }
            set
            {
                this.fitness_function = value;
            }
        }

        public void Calculate_Fitness_function()    //расчем функции приспособленности
        {
            //функция приспособленности генотипа равна сумме функций приспособленности всех его хромосом
            int function = 0;
            foreach (Project_chromosome chrom in chromosome)
            {
                function += chrom.Function;
            }
            this.fitness_function = function;
        }

        
    }
}
