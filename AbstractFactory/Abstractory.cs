using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Abstractory
{
    public class Factory
    {
        private readonly Dictionary<string, Dictionary<MethodInfo, object>> _dictionary;

        public Factory()
        {
            _dictionary = new Dictionary<string, Dictionary<MethodInfo, object>>();
            PopulateDictionary();
        }

        /// <summary>
        /// Finds all methods in an Class and loads them into a dictionary
        /// </summary>
        private void PopulateDictionary()
        {
            var types = GetTypesWithAbstractoryAttribute();

            List<string> instances = new List<string>();
            List<object> objects = new List<object>();

            foreach (var type in types)
            {
                List<MethodInfo> methods = FindAllMethods(type);

                object instance = Activator.CreateInstance(type);

                var attributeName = type.GetCustomAttribute<AbstractoryAttribute>().GetName();

                if (!_dictionary.ContainsKey(attributeName))
                {
                    _dictionary.Add(attributeName, new Dictionary<MethodInfo, object>());
                }

                foreach (var method in methods)
                {
                    _dictionary[attributeName].Add(method, instance);
                }
            }
        }

        private IEnumerable<Type> GetTypesWithAbstractoryAttribute()
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(AbstractoryAttribute))
                   select t;
        }
        
        /// <summary>
        /// Reads the public methods of a Class
        /// </summary>
        /// <returns>A List of Methods</returns>
        private List<MethodInfo> FindAllMethods(Type type)
        {
            return new List<MethodInfo>(type.GetMethods());
        }

        public List<string> ListAllTags()
        {
            return new List<string>(_dictionary.Keys);
        }
        
        /// <summary>
        /// Executes a string on a list of tags that the client enters. The List of objects is
        /// all of the arguments that the method needs.
        /// </summary>
        /// <param name="method">The method string to be exectuted</param>
        /// <param name="args">List of the arguments passed to the string method</param>
        /// <param name="tags">The attribute tags to execute the method on. Can be null</param>
        /// <returns>A AbstractoryResponse object with a dictionary where the keys are the tags and the values are the responses from the method</returns>
        public async Task<Response<Dictionary<string, Response<T>>>> ExecuteCommand<T>(string method, List<Object> args, List<string> tags)
        {
            Dictionary<string, Response<T>> data = new Dictionary<string, Response<T>>();

            var tagsThatExist = new List<string>();
            var tagsThatDontExist = new List<string>();

            foreach (var tag in tags)
            {
                if (_dictionary.ContainsKey(tag))
                {
                    tagsThatExist.Add(tag);
                }
                else
                {
                    tagsThatDontExist.Add(tag);
                }
            }

            foreach (string tag in tagsThatExist)
            {
                var methodDict = _dictionary[tag];

                var methodInfo = CheckIfContainsMethod(new List<MethodInfo>(methodDict.Keys), method);

                if (methodInfo == null)
                {
                    data.Add(tag, null);
                    continue;
                }

                var instance = methodDict[methodInfo];
                
                Response<T> response = await (Task<Response<T>>) instance.GetType().GetMethod(method).Invoke(instance, (args?.ToArray()));
                
                data.Add(tag, response);
            }

            return new Response<Dictionary<string, Response<T>>>
            {
                Data = data,
                ResponseStatus = (tagsThatDontExist.Count == 0 ? Enums.ResponseStatus.Success : Enums.ResponseStatus.NotExist),
                TagsNotFound = (tagsThatDontExist.Count == 0 ? null : tagsThatDontExist)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods">List of all methods that the factory contains for this tag</param>
        /// <param name="method">Method to lookup</param>
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
