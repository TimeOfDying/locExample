using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocExample
{
    public class CustomResolver
    {
        private static Dictionary<Type, Type> bindingTypeToType = new Dictionary<Type, Type>();
        private static Dictionary<Type, Object> bindingTypeToObject = new Dictionary<Type, Object>();

        public void Bind<T,V>()
        {
            bindingTypeToType.Add(typeof(T), typeof(V));

        }

        public void BindToObject<T>(Object obj)
        {
            bindingTypeToObject.Add(typeof(T), obj);

        }

        public T Get<T>()
        {
            var type = typeof(T);
            return (T)Get(type);
        }

        private object Get(Type type)
        {
            if (bindingTypeToObject.Keys.Contains(type))
            {
                return bindingTypeToObject[type];
            }
            else
            {
                var targetType = DefineType(type);
                var constructor = Utils.GetSingleConstructor(targetType);
                var parameters = constructor.GetParameters();
                List<Object> definedParameters = new List<object>();

                foreach (var parameter in parameters)
                {
                    definedParameters.Add(Get(parameter.ParameterType));
                }

                return Utils.CreateInstance(targetType, definedParameters);

            }
        }

        private Type DefineType(Type type)
        {
            if (bindingTypeToType.Keys.Contains(type))
            {
                return bindingTypeToType[type];
            }

            return type;
        }

        
    }
}
