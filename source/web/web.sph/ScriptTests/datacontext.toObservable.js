
$(document).ready(function () {
    var hit = {
        "$type": "Bespoke.Sph.Domain.Space, domain.sph",
        "SearchKeywords": "BPH.UT/KR/7  Wilayah Persekutuan Putrajaya",
        "ApplicationTemplateOptions": {
            "$type": "System.Int32[], mscorlib",
            "$values": [6]
        },
        "LotCollection": {
            "$type": "Bespoke.Sph.Domain.ObjectCollection`1[[Bespoke.Sph.Domain.Unit, domain.sph]], domain.sph",
            "$values": []
        },
        "CustomFieldValueCollection": {
            "$type": "Bespoke.Sph.Domain.ObjectCollection`1[[Bespoke.Sph.Domain.CustomFieldValue, domain.sph]], domain.sph",
            "$values": [{
                "$type": "Bespoke.Sph.Domain.CustomFieldValue, domain.sph",
                "Name": "Name",
                "Type": "String",
                "Value": "KEDAI RUNCIT",
                "WebId": null
            },
                {
                    "$type": "Bespoke.Sph.Domain.CustomFieldValue, domain.sph",
                    "Name": "Location",
                    "Type": "String",
                    "Value": "Presint 5",
                    "WebId": null
                },
                {
                    "$type": "Bespoke.Sph.Domain.CustomFieldValue, domain.sph",
                    "Name": "Status",
                    "Type": "String",
                    "Value": "Aktif",
                    "WebId": null
                }]
        },
        "Address": {
            "$type": "Bespoke.Sph.Domain.Address, domain.sph",
            "UnitNo": null,
            "Floor": null,
            "Block": null,
            "Street": "Apartmen P5R3, No. 2, Jalan P5/7",
            "City": null,
            "Postcode": "62200",
            "State": "Wilayah Persekutuan Putrajaya",
            "Country": "Malaysia",
            "WebId": null
        },
        "PhotoCollection": {
            "$type": "Bespoke.Sph.Domain.ObjectCollection`1[[Bespoke.Sph.Domain.Photo, domain.sph]], domain.sph",
            "$values": []
        },
        "CustomListValueCollection": {
            "$type": "Bespoke.Sph.Domain.ObjectCollection`1[[Bespoke.Sph.Domain.CustomListValue, domain.sph]], domain.sph",
            "$values": []
        },
        "FeatureDefinitionCollection": {
            "$type": "Bespoke.Sph.Domain.ObjectCollection`1[[Bespoke.Sph.Domain.FeatureDefinition, domain.sph]], domain.sph",
            "$values": []
        },
        "SpaceId": 15,
        "TemplateId": 4,
        "TemplateName": null,
        "BuildingId": 120,
        "LotName": null,
        "FloorName": "Tingkat 1",
        "Size": 0.0,
        "Category": "Kedai Runcit",
        "RentalType": "Sebulan",
        "IsOnline": true,
        "RegistrationNo": "BPH.UT/KR/7",
        "IsAvailable": true,
        "ContactPerson": "MOHD HELMI BIN MOHD ABDUL HALIM",
        "ContactNo": null,
        "State": "Wilayah Persekutuan Putrajaya",
        "City": "NULL",
        "BuildingName": "Kedai ,  Apartmen P5R3, No. 2, Jalan P5/7  Wilayah Persekutuan Putrajaya",
        "BuildingLot": null,
        "RentalRate": 730.0,
        "Location": null,
        "Description": null,
        "CreatedBy": null,
        "CreatedDate": "0001-01-01T00:00:00",
        "ChangedBy": null,
        "ChangedDate": "0001-01-01T00:00:00",
        "WebId": null
    };

    setTimeout(function () {

        require(['services/datacontext'], function (context) {
            console.log(typeof ko);
            // array
            var arrayTypeNamePattern = /\[/;
            test("Array type pattern", function () {
                var match = arrayTypeNamePattern.exec(hit.ApplicationTemplateOptions.$type);
                ok(match, "array of int ");

                var cf = arrayTypeNamePattern.exec(hit.CustomFieldValueCollection.$type);
                ok(cf, "should not be null");

                var address = arrayTypeNamePattern.exec(hit.Address.$type);
                ok(address === null, "Address is not an array ");
            });



            var o = context.toObservable(hit);
            test("Entity to observable", function () {
                equal(15, o.SpaceId(), "Space id");
                ok(typeof o.ApplicationTemplateOptions === "function", "ApplicationTemplateOptions should be a function ");

                equal(o.ApplicationTemplateOptions().length, 1, "There should only be 1 options ");
                deepEqual([6], o.ApplicationTemplateOptions(), "There value should be 6");

                equal(typeof o.Location,"function",  "Null value property should be convert to ko.observable");
                equal(o.Location.name,"observable", "Null value property should be convert to ko.observable");
            });


            test("aggregate field", function () {
                equal("function", typeof o.Address, "Should be observable ");
                deepEqual("Malaysia", o.Address().Country(), "Address is aggregate and country should be observable");

            });


            test("Custom fields ", function () {
                equal(typeof o.CustomFieldValueCollection, "function", "Custom fields is a function(observable)");
                deepEqual("KEDAI RUNCIT", o.CustomFieldValueCollection()[0].Value(), "Custom fields values");
            });

        });
    }, 1000);
});