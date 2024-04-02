using System;
using System.Collections.Generic;

namespace PCTTools.Model
{
    public class InterfacesList
    {
        public Type TypeClasse { get; set; }

        public List<Type> Interfaces { get; set; }

        public int Count => Interfaces.Count;
    }
}
