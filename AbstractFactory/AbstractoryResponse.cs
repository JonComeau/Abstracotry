using Abstractory.Enums;
using System;
using System.Collections.Generic;

namespace Abstractory
{
    public class Response<T>
    {
        public T Data { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<string> TagsNotFound { get; set; }
        public List<Exception> Errors { get; set; }
    }
}