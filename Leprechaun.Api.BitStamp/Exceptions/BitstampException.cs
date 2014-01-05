using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leprechaun.BitStamp.Api.Client
{
    public class BitStampException : Exception
    {
        public BitStampException(string message, Exception innerException = null)
            : base(message, innerException)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                this.Error = new Error { Messages = new List<string>() };
                this.Error.Messages.Add(message);
            }
        }

        public BitStampException(Error error, Exception innerException = null)
            : base("BitStampException", innerException)
        {
            this.Error = error;
        }

        public Error Error { get; set; }
    }
}
