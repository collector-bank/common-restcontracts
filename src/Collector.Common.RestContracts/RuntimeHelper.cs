using System;

namespace Collector.Common.RestContracts
{
    using System.Reflection;
    public class RuntimeHelper
    {
        private static readonly Func<Type, object> getUninitializedObjectDelegate =
            (Func<Type, object>)
                typeof(string)
                    .GetTypeInfo()
                    .Assembly
                    .GetType("System.Runtime.Serialization.FormatterServices")
                    .GetMethod("GetUninitializedObject", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                    .CreateDelegate(typeof(Func<Type, object>));

        /// <summary>
        /// Returns an unitialized object with FormatterServices.
        /// </summary>
        /// <param name="type">The type to create</param>
        public static object GetUninitializedObjectWithFormatterServices(Type type)
        {
            return getUninitializedObjectDelegate.Invoke(type);
        }
    }
}