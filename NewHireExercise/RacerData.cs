using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewHireExercise
{
    class RacerData
    {
        public string name;
        public int time;
        public int age;
        public int ranking;

        public RacerData(string name,int time,int age)
        {
            this.name = name;
            this.time = time;
            this.age = age;
            //initialized with a negative ranking for testign
            this.ranking = -1;
        }

        public string toString()
        {
            return name + "\t" + Convert.ToString(time) + "\t" + Convert.ToString(age) + "\t" + Convert.ToString(ranking); 
        }
    }
}
