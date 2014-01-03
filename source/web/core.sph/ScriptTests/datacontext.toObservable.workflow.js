

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

$(document).ready(function () {
    var wf = {
        "$type": "Bespoke.Sph.Workflows_1_12.SampleWakafWorkflow, workflows.1.12",
        "tu": null,
        "Pemohon": {
            "$type": "Bespoke.Sph.Workflows_1_12.Applicant, workflows.1.12",
            "Name": "erymuzuan",
            "MyKad": null,
            "RegisteredDate": "0001-01-01T00:00:00",
            "Age": null,
            "Dob": null,
            "Address": {
                "$type": "Bespoke.Sph.Workflows_1_12.Address, workflows.1.12",
                "Street": null,
                "Postcode": null,
                "State": null,
                "City": null,
                "WebId": null
            },
            "Contact": {
                "$type": "Bespoke.Sph.Workflows_1_12.Contact, workflows.1.12",
                "Telephone": null,
                "Address": {
                    "$type": "Bespoke.Sph.Workflows_1_12.Address, workflows.1.12",
                    "Street": null,
                    "Postcode": null,
                    "State": null,
                    "City": null,
                    "WebId": null
                }, "WebId": null
            }, "WebId": null
        }, "VariableValueCollection": {
            "$type": "Bespoke.Sph.Domain.ObjectCollection`1[[Bespoke.Sph.Domain.VariableValue, domain.sph]], domain.sph", "$values": []
        },
        "WorkflowId": 0,
        "WorkflowDefinitionId": 0,
        "Name": null,
        "State": null,
        "IsActive": false,
        "CreatedBy": null,
        "CreatedDate": "0001-01-01T00:00:00",
        "ChangedBy": null,
        "ChangedDate": "0001-01-01T00:00:00",
        "WebId": null
    };

    setTimeout(function () {

        require(['services/datacontext'], function (context) {

            var o = context.toObservable(wf, /Bespoke\.Sph\.Workflows_1_12\.(.*?),/);
            test("Entity to observable", function () {
                // wf
                equal(typeof o.Name, "function", "Null value property should be convert to ko.observable");
                equal(o.Name.name, "observable", "Null value property should be convert to ko.observable");

                // complex variable
                equal(typeof o.Pemohon, "function", "Complex variable should be converted to ko.observable");
                equal(typeof o.Pemohon().Name, "function", "Complex variable property should be converted to ko.observable");
                equal(typeof o.Pemohon().Age, "function", "Complex variable property should be converted to ko.observable");

                // children of complex variable
                equal(typeof o.Pemohon().Contact, "function", "Complex variable children should be converted to ko.observable");
                equal(typeof o.Pemohon().Contact().Telephone, "function", "Comlplex variable children's property value property should be converted to ko.observable");
            });


        });
    }, 1000);
});