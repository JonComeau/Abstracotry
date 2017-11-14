using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AbstractFactory
{
    public class Factory<T> : IFactory
    {
        private readonly Dictionary<Type, Dictionary<MethodInfo, T>> _dictionary;

        public Factory()
        {
            _dictionary = new Dictionary<Type, Dictionary<MethodInfo, T>>();
            PopulateDictionary();
        }

        /// <summary>
        /// Finds all methods in an LMS and loads them into a dictionary
        /// </summary>
        private void PopulateDictionary()
        {
            List<Type> types = FindAllLmsInstances();

            List<string> instances = new List<string>();
            List<T> objects = new List<T>();

            foreach (var type in types)
            {
                List<MethodInfo> methods = FindAllLmsMethods(type);
                T lms = (T)Activator.CreateInstance(type);

                if (!_dictionary.ContainsKey(type))
                {
                    _dictionary.Add(
                        type,
                        new Dictionary<MethodInfo, T>());
                }

                foreach (var method in methods)
                {
                    _dictionary[lms.GetLmsName().ToLower()].Add(method, lms);
                }
            }
        }

        private bool Contains
        
        /// <summary>
        /// Reads the public methods of a LMS
        /// </summary>
        /// <returns>A List of Methods</returns>
        private List<MethodInfo> FindAllLmsMethods(Type lmsRepository)
        {
            return new List<MethodInfo>(lmsRepository.GetMethods());
        }

        /// <summary>
        /// Finds all Classes that derive from ILmsRepository
        /// </summary>
        /// <returns>A List of Lms Instances</returns>
        private List<Type> FindAllLmsInstances()
        {
            var assembly = Assembly.GetAssembly(typeof(ILmsRepository));
            Type[] types = assembly.GetTypes();
            var intfaces = types.Where(x => x.GetInterfaces().Contains(typeof(ILmsRepository))).ToList();
            var lms = new List<Type>();

            foreach (var intface in intfaces)
            {
                lms.AddRange(types.Where(x => x.GetInterfaces().Contains(intface)));
            }

            return lms;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method">The method string to be exectuted</param>
        /// <param name="args">List of the arguments passed to the string method</param>
        /// <param name="lms">The LMS to execute the method on. Can be null</param>
        /// <returns></returns>
        public async Task<List<Response<TReturn>>> ExecuteCommand<TReturn>(string method, List<Object> args, string lms = null)
        {
            List<string> lmsList;
            List<Response<TReturn>> responses = new List<Response<TReturn>>();
            
            if (lms == null)
            {
                lmsList = new List<string>(_dictionary.Keys);
            }
            else if (_dictionary.ContainsKey(lms.ToLower()))
            {
                lmsList = new List<string>(new string[] { lms.ToLower() });
            }
            else
            {
                var errorMessage = "";

                foreach (var key in _dictionary.Keys)
                {
                    errorMessage += $" {key}";
                }

                throw new Exception($"{lms.ToLower()}: {errorMessage}");
            }

            foreach (string lmsName in lmsList)
            {
                var methodDict = _dictionary[lmsName];

                var methodInfo = CheckIfContainsMethod(new List<MethodInfo>(methodDict.Keys), method);

                if (methodInfo == null)
                {
                    responses.Add(null);
                    continue;
                }

                var lmsRepository = methodDict[methodInfo];
                
                Response<TReturn> response = await (Task<Response<TReturn>>) lmsRepository.GetType().GetMethod(method).Invoke(lmsRepository, (args?.ToArray()));
              //  response.Lms = lms;
                
                responses.Add(response);
            }

            return responses;
        }

        /// <summary>
        /// Checks to see if the selected LMS contains a method
        /// </summary>
        /// <param name="holder">An object made up of the LMSRepository and a list of methods</param>
        /// <param name="method">The method to check against</param>
        /// <returns></returns>
        private MethodInfo CheckIfContainsMethod(List<MethodInfo> methods, string method)
        {
            foreach (var methodName in methods)
            {
                if (methodName.Name == method)
                {
                    return methodName;
                }
            }
            return null;
        }
    }
}
