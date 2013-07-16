using System;
using System.IO;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class MessageHeaders
    {
        private readonly ReceivedMessageArgs m_args;

        internal MessageHeaders(ReceivedMessageArgs args)
        {
            m_args = args;
        }

        private string ByteToString(byte[] content)
        {
            using (var orginalStream = new MemoryStream(content))
            {
                using (var sr = new StreamReader(orginalStream))
                {
                    var xml = sr.ReadToEnd();
                    return xml;
                }
            }
        }

        public string Operation
        {
            get
            {
                var operationBytes = m_args.Properties.Headers["operation"] as byte[];
                if (null != operationBytes)
                    return ByteToString(operationBytes);

                return null;
            }
        }
        public CrudOperation Crud
        {
            get
            {
                var crud = CrudOperation.None;
                var operationBytes = m_args.Properties.Headers["crud"] as byte[];
                if (null != operationBytes)
                {
                    var v = ByteToString(operationBytes);
                    if (Enum.TryParse(v, true, out crud))
                        return crud;

                }

                return crud;

            }
        }
    }
}