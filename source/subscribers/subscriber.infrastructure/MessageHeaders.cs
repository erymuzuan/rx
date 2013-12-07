using System;
using System.Dynamic;
using System.IO;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class MessageHeaders : DynamicObject
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
                    var text = sr.ReadToEnd();
                    return text;
                }
            }
        }

        public AuditTrail Log
        {
            get
            {
                var operationBytes = m_args.Properties.Headers["log"] as byte[];
                if (null != operationBytes)
                {
                    var json = ByteToString(operationBytes);
                    if (string.IsNullOrWhiteSpace(json)) return null;
                    return json.DeserializeFromJson<AuditTrail>();
                }

                return null;
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

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var operationBytes = m_args.Properties.Headers[binder.Name] as byte[];

            if (null != operationBytes)
            {
                var r = ByteToString(operationBytes);
                if (binder.ReturnType == typeof(string[]))
                {
                    result = r.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return true;

                }
                if (binder.ReturnType == typeof(int))
                {
                    int no;
                    if (int.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(bool))
                {
                    bool no;
                    if (bool.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(decimal))
                {
                    decimal no;
                    if (decimal.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(double))
                {
                    double no;
                    if (double.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(DateTime))
                {
                    DateTime no;
                    if (DateTime.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }

                result = r;
                return true;
            }
            result = null;
            return false;
        }
    }
}