using System;
using TestFx;

namespace Dotter.Specs
{
    public class AssemblySetup : IAssemblySetup
    {
        public void Setup()
        {
            var loc = this.GetType().Assembly.Location;
            var path = System.IO.Path.GetDirectoryName(loc);
            Environment.CurrentDirectory = path;
        }

        public void Cleanup()
        {

        }

    }
}