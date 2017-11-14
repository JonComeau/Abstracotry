using Abstractory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbtractoryConsole.Examples
{
    [Abstractory("Person")]
    public class Name
    {
        private string _first;
        private string _last;

        public string GetFirst( ){ return _first; }
        public void SetFirst(string first) { _first = first; }
        public string GetLast() { return _last; }
        public void SetLast(string last) { _last = last; }
        public string GetName() { return $"{_first} {_last}"; }
        public void SetName(string first, string last){ _first = first; _last = last; }
    }
}
