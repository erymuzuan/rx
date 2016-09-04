using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "Microsoft SQL Server", Route = "receive.location.sqlserver/:id", FontAwesomeIcon = "windows", Name = "sqlserver")]
    public partial class SqlServerReceiveLocation : ReceiveLocation
    { }

    #region "todo"

    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "Email POP", Route = "receive.location.pop/:id", FontAwesomeIcon = "envelope-o", Name = "pop")]
    public partial class PopReceiveLocation : ReceiveLocation
    { }
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "FTP", Route = "receive.location.ftp/:id", FontAwesomeIcon = "files-o", Name = "ftp")]
    public partial class FtpReceiveLocation : ReceiveLocation
    { }


    #endregion
}