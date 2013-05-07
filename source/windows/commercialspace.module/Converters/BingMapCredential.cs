using System;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    public class BingMapCredential : CredentialsProvider
    {
        public override void GetCredentials(Action<Credentials> callback)
        {
            var cred = new Credentials
                {
                    ApplicationId = "ArJEUSn6k9K9DNFPMHZd3XdmgkUelh95G_p3e9tNuH6ABzrOXZdepP3It3k-FSI4"
                };
            callback(cred);
        }
    }
}
