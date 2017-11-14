using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbstractFactory
{
    public interface IFactory
    {
        Task<List<Response<T>>> ExecuteCommand<T>(string method, List<Object> args, string lms = null);
    }
}
