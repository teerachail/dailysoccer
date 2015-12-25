using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Repositories
{
    class DemoRepository : IDemoRepository
    {
        public string GetEmail(string userId)
        {
            var coltn = MongoAccess.MongoUtil.GetCollection("collection.name");
            return string.Format("You've send a {0}.", userId);
        }
    }
}
