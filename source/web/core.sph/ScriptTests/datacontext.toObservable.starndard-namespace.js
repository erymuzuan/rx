
$(document).ready(function () {


    bespoke.sph.domain.SimpleMemberPartial = function (model) {
        "use strict";

        var sp = ko.observable("simple-partial");
        model.Name.subscribe(function (name) {
            console.log("Log from SimpleMemberPartial ", name);
            sp(name);
        });
        return {
            "TestPartial": sp
        }
    };

    bespoke.sph.domain.MemberPartial = function (model) {
        "use strict";
        var bp = ko.observable("base-partial");

        model.Name.subscribe(function (name) {
            console.log("Log from base member.js ", name);
            bp(name);
        });
        return {
            "BasePartial": bp
        }
    };

    setTimeout(function () {

        require(["services/datacontext", "knockout", "_test-data-entity-defintions"], function (context, ko, entityDefinition) {
            console.log(typeof ko);
            // array
            var arrayTypeNamePattern = /\[/;
            test("Array type pattern", function () {
                var match = arrayTypeNamePattern.exec(entityDefinition.MemberCollection.$type);
                ok(match, "array of int ");

                var cf = arrayTypeNamePattern.exec(entityDefinition.MemberCollection.$type);
                ok(cf, "should not be null");

                var contract = arrayTypeNamePattern.exec(entityDefinition.ServiceContract.$type);
                ok(contract === null, "ServiceContract is not an array ");
            });



            var o = context.toObservable(entityDefinition);
            test("Entity to observable", function () {
                equal("Country", ko.unwrap(o.Name), "Name should be Country");
                ok(typeof o.MemberCollection === "function", "MemberCollection should be a function ");

                equal(o.MemberCollection().length, entityDefinition.MemberCollection.$values.length, "There should only be 1 options ");
            });


            test("aggregate field", function () {
                equal("function", typeof o.ServiceContract, "Should be observable ");
                deepEqual("Bespoke.DevV1.IntegrationApis", o.ServiceContract().CodeNamespace(), "Address is aggregate and country should be observable");

            });
            test("Inherited member", function () {
                equal(true, ko.isObservable(o.MemberCollection()[0].AllowMultiple), "Should be observable ");
                deepEqual("Bespoke.DevV1.IntegrationApis", o.ServiceContract().CodeNamespace(), "ServiceContract is aggregate should be observable");

            });


            // test partial
            var nameMember = o.MemberCollection()[0];
            test("Partial", function () {
                equal("Name", ko.unwrap(nameMember.Name));
                equal("Bespoke.Sph.Domain.SimpleMember, domain.sph", ko.unwrap(nameMember.$type));

                equal(true, ko.isObservable(nameMember.TestPartial));
                equal("simple-partial", ko.unwrap(nameMember.TestPartial));

                equal(true, ko.isObservable(nameMember.BasePartial));
                equal("base-partial", ko.unwrap(nameMember.BasePartial));


                // hook event to model
                nameMember.Name("Country2");

                equal("Country2", ko.unwrap(nameMember.TestPartial));
                equal("Country2", ko.unwrap(nameMember.BasePartial));


            });

        });
    }, 1000);
});