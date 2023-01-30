using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class ServerException
    {
        string message;

        [DataMember]
        public string Message { get => message; set => message = value; }

        public ServerException(string message)
        {
            Message = message;
        }
    }
}
