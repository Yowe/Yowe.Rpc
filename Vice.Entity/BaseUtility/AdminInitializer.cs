using Vice.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Vice.Entity
{
    public class AdminInitializer : DropCreateDatabaseIfModelChanges<ViceDataContext>
    {
    }
}
