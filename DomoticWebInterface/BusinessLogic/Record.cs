using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Record<T>
    {
        public string date { get; set; }
        public T value { get; set; }
    }

    class RootRecord<T>
    {
        public List<Record<T>> record { get; set; }
    }


}
