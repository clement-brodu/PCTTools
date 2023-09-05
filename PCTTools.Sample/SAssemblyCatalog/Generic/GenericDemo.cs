using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.Generic
{
    public class GenericDemo<T>
    {

        public T Value { get; set; }

        public List<T> ValueList { get; set; }

        public T Hello()
        {
            throw new NotImplementedException();
        }
        public List<T> HelloList()
        {
            throw new NotImplementedException();
        }
        public List<Dictionary<int, T>> HelloList2()
        {
            throw new NotImplementedException();
        }
    }
    public class GenericDemo2<T, U> where U : Dictionary<string, GenericDemo<T>>
    {

        public T Value { get; set; }

        public T Hello()
        {
            throw new NotImplementedException();
        }

    }


    public class GenericDemoInt : GenericDemo<int> { }
}
