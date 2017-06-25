using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gen_alg
{
    public class Project
    {
        int id; //идентификатор проекта
        string name; //имя проекта
   

        Dictionary<KeyValuePair<string, int>, string> attrib; //массив аттрибутов вида: пара (атрибут + его относительый вес) - значение атрибута)

        public Project()
        {
            this.id = 0;
            this.name = "";
          
            attrib = new Dictionary<KeyValuePair<string, int>, string>();
        }

        public Project(int id, string name)
        {
            this.id = id;
            this.name = name;
            attrib = new Dictionary<KeyValuePair<string, int>, string>();
        }

       
        public void Change_attrib(Dictionary<KeyValuePair<string, int>, string> new_attrib) //изменение массива атрибутов
        {
            this.attrib = new_attrib;
        }

        public void Add_attrib(string attr, int weight, string value)   //добавление атрибута в массив
        {
            KeyValuePair<string, int> at = new KeyValuePair<string, int>(attr, weight);          

            this.attrib.Add(at, value);
        }
        public void Delete_attrib(string attr)  //удаление атрибута из массива по его названию
        {
            foreach (KeyValuePair<string, int> key in attrib.Keys)
            {
                if (key.Key == attr)
                attrib.Remove(key);
                break;
            }
        }
        public Dictionary<KeyValuePair<string, int>, string> Get_all_attrib()   //возврат массива атрибутов
        {
            return attrib;
        }

        public int Attrib_count()
        {
            return attrib.Count;
        }

        public KeyValuePair<KeyValuePair<string,int>, string> Get_attrib(int position)  //возврат значения из массива атрибутов по его позиции
        {
            return this.attrib.ElementAt(position);
        }

        public string Get_attrib_value(string key)  //возврат значения атрибута по его названию
        {
            string value = null;
            foreach (KeyValuePair<string, int> key_pair in attrib.Keys)
            {
                if (key_pair.Key == key)
                {
                    attrib.TryGetValue(key_pair, out value);
                }
                break;
                    
            }
            return value;
        }

        public int Get_attrib_weight(string key)    //возврат относительного веса атрибута по его названию
        {
            int weight = 0;
            foreach (KeyValuePair<string, int> key_pair in attrib.Keys)
            {
                if (key_pair.Key == key)
                {
                    weight = key_pair.Value;
                }
                break;

            }
            return weight;
        }

        public Dictionary<string, string> Get_attributes_and_value_pair()   //возврат массива вида: атрибут - его значение
        {
            Dictionary<string, string> attr_and_value = new Dictionary<string, string>();

            foreach(KeyValuePair<KeyValuePair<string, int>, string> attrs in attrib)
            {
                attr_and_value.Add(attrs.Key.Key, attrs.Value);
            }

            return attr_and_value;
        }

        public Dictionary<string, int> Get_attributes_and_weight_pair() //возврат массива вида: атрибут - вес атрибута
        {
            Dictionary<string, int> attr_and_weight = new Dictionary<string, int>();

            foreach(KeyValuePair<string, int> attrs in attrib.Keys)
            {
                attr_and_weight.Add(attrs.Key, attrs.Value);
            }

            return attr_and_weight;
        }       

        public int ID   //свойство для задания и возврата ID
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

        public string Name  //свойство для задания и возврата имени
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}
