﻿using System;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace elasticsearc.repository.test
{
    public static class TestOutputHelperExtension
    {

        public static void WriteLine(this ITestOutputHelper console, object value)
        {
            console.WriteLine($"{value}");
        }
        public static JObject WriteJson(this ITestOutputHelper console, object value)
        {
            if (value is string text)
            {
                try
                {
                    var json = JObject.Parse(text);
                    console.WriteLine(json.ToString());
                    return json;
                }
                catch (Exception)
                {
                    console.WriteLine(text);
                }
            }

            if (value is DomainObject dm)
            {
                var text2 = dm.ToJsonString(true);
                console.WriteLine(text2);
                return JObject.Parse(text2);
            }

            throw new InvalidOperationException($"{value} Cannot be turn into json");
        }
    }
}