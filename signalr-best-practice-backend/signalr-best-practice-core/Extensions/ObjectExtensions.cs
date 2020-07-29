using System;
using System.Linq;
using System.Reflection;

namespace signalr_best_practice_core.Extensions
{
    public static class ObjectExtensions
    {
        public static void Copy<Tin, TFrom>(this Tin obj, TFrom fromObj)
        {
            if (obj == null || fromObj == null) return;

            Func<PropertyInfo, bool> query = p => p.PropertyType.IsValueType || p.PropertyType.Name == "String";

            var fields1 = obj.GetType().GetProperties().Where(query).ToArray();
            var fields2 = fromObj.GetType().GetProperties().Where(query).ToArray();

            for (int i = 0; i < fields2.Length; i++)
            {
                if (fields2[i].Name == "Created")
                {
                    continue;
                }

                var value = fields2[i].GetValue(fromObj);
                fields1.FirstOrDefault(x => x.Name == fields2[i].Name)?.SetValue(obj, value);
            }
        }

        public static bool CompareProperty(this object n, object x, string propertyName)
        {
            return n.GetType().GetProperty(propertyName).GetValue(n) == x.GetType().GetProperty(propertyName).GetValue(x);
        }
    }
}
