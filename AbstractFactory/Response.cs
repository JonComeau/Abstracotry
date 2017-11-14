using System;
using System.Collections.Generic;

namespace AbstractFactory
{
    public class Response<T>
    {
        T Data { get; set; }
        List<Exception> errors { get; set; }
    }
}