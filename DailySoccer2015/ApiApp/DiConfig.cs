using repo = ApiApp.Repositories;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    static class DiConfig
    {
        public static IContainer CreateContainer()
        {
            return new Container(c =>
            {
                c.For<repo.IDemoRepository>().Use<repo.DemoRepository>();
            });
        }
    }
}
