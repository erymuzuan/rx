using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using Newtonsoft.Json;

namespace domain.test.entities
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value, bool verbose = false)
        {
            if (verbose)
            {

                var json = JsonConvert.SerializeObject(value, Formatting.Indented);
                Console.WriteLine("     ========================    ");
                Console.WriteLine(json);
                Console.WriteLine("     ========================    ");
            }
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return (ExpandoObject)expando;
        }
    }
}