bespoke.sphcommercialspace.domain.CommercialSpacePartial = function() {

    return {
      StaticMap : ko.observable("/images/no-image.png")  
    };
}
bespoke.sphcommercialspace.domain.RolePartial = function () {

    return {
      permissions : ko.observableArray()  
    };
}