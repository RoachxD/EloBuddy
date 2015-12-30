using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarksmanAIO.Champions
{
    abstract class AIOChampion
    {
        public abstract void Init(); //100% necessary, abstract
        //Init only needs to declare abilities and necessary events for the specific champion
        public abstract void Combo(); //100% necessary, abstract
        public virtual void Laneclear() //not 100% necessary, override if needed
        {

        }
        public virtual void Flee() //not 100% necessary, override if needed
        {

        }
    }
}
