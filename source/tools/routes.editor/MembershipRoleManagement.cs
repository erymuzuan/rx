using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace routes.editor
{
    [Serializable]
    public class MembershipRoleManagement : MarshalByRefObject
    {
        private readonly string m_config;

        public MembershipRoleManagement(string config)
        {
            m_config = config;
            Debug.WriteLine("config: " + config);
        }

        public void UpdateRoles(IEnumerable<string> roles)
        {
            var args = roles.Select(r => string.Format("-r {0}", r))
                .ToArray();
            Process.Start(ConfigurationManager.AppSettings["mru"], string.Join(" ", args) + " -c " + m_config);
        }
    }
}