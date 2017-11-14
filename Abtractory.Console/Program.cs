using System;
using Abstractory;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbtractoryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new Factory();

            var name = factory.ExecuteCommand<string>("SetFirst", new List<object> { "Dummy" }, new List<string> { "Person" }).Result;

            var factory.ExecuteCommand<string>("GetName", null, new List<string> { "Person" }).Result;
        }
    }
}
