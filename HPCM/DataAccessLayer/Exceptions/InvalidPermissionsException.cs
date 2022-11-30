using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Exceptions
{
    public class InvalidPermissionsException:Exception
    {
        public InvalidPermissionsException()
        {
        }

        public InvalidPermissionsException(string message)
            : base(message)
        {
        }

        public InvalidPermissionsException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }

    
}
