var bespoke =  bespoke || {};
bespoke.sph =  bespoke.sph || {};
bespoke.sph.integrations =  bespoke.sph.integrations || {};
bespoke.sph.integrations.adapters =  bespoke.sph.integrations.adapters || {};

bespoke.sph.integrations.adapters.SqlServerAdapter = function(optionOrWebid){
    /*
        public string Server { get; set; }
        public bool TrustedConnection { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }*/
  var model = {
      $type : "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter",
      Server : ko.observable(),
      TrustedConnection : ko.observable(),
      ColumnDisplayNameStrategy: ko.observable(),
      UserId : ko.observable(),
      Password : ko.observable(),
      Database : ko.observable()
  };

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    return _(model).extend(new bespoke.sph.domain.api.Adapter(optionOrWebid));
};