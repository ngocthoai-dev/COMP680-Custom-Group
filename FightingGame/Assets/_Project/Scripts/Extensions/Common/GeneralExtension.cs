using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Shared.Extension
{
    public static class GeneralExtension
    {
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }

        public static void CallMethod(this object obj, string methodName, params object[] args)
        {
            Type thisType = obj.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName);
            theMethod.Invoke(obj, args);
        }

        public static object CreateInstance(Type type)
        {
            object instace = Activator.CreateInstance(type);
            return instace;
        }

        public static string RandomName()
        {
            Random random = new();
            string name = "User" + random.Next(0, 1000000);
            return name;
        }

        public static bool ReferenceEqual(this object obj1, object obj2)
        {
            return ReferenceEquals(obj1, obj2);
        }

        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}