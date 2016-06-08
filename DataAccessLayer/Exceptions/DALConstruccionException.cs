using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Exceptions
{
    class DALConstruccionException:Exception
    {

        public DALConstruccionException(string message) : base(message)
        {
        }

    }
}
