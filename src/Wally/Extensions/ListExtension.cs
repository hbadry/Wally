using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wally.Extensions
{
    public static class ListExtension
    {
        public static T RandomItem<T> (this List<T> lst)
        {
            int index = new Random().Next(lst.Count);
            return lst[index]; 
        }
    }
}
