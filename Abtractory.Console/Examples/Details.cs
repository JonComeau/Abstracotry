using Abstractory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbtractoryConsole.Examples
{
    [Abstractory("Person")]
    public class Details
    {
        private string _email;
        private string _iNumber;

        public string GetINumber() { return _iNumber; }
        public void SetINumber(string iNumber) { _iNumber = iNumber; }
        public string GetEmail() { return _email; }
        public void SetEmail(string email) { _email = email; }
    }
}
