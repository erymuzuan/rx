﻿using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace assembly.test
{
    public class AssemblyClassToTest
    {
        public string SayHello(string name)
        {
            return string.Format("Hello " + name);
        }
        public string Greet(string greet, string name)
        {
            return string.Format(greet + " " + name);
        }
        public string SayBuildingName(Building masjid, string greet)
        {
            return string.Format( greet + " " + masjid.Name);
        }
        public async Task<object> GreetAsync(Building masjid, string greet)
        {
            await Task.Delay(500);
            return string.Format( greet + " " + masjid.Name);
        }
    }
}