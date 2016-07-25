
$(document).ready(function () {
   

    setTimeout(function () {

        require(["services/datacontext", "knockout", "_test-data-sql-adapter"], function (context, ko, sqlAdapter) {
            console.log(typeof ko);
            // array
            var arrayTypeNamePattern = /\[/;
            test("Array type pattern", function () {
                var match = arrayTypeNamePattern.exec(sqlAdapter.TableDefinitionCollection.$type);
                ok(match, "array of int ");

                var cf = arrayTypeNamePattern.exec(sqlAdapter.TableDefinitionCollection.$type);
                ok(cf, "should not be null");

            });



            //, "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter"
            var o = context.toObservable(sqlAdapter);
            test("Space to observable", function () {
                ok(typeof o.TableDefinitionCollection === "function", "TableDefinitionCollection should be a function ");

                equal(o.OperationDefinitionCollection().length, sqlAdapter.OperationDefinitionCollection.$values.length);
                deepEqual("ufnGetStock", o.OperationDefinitionCollection()[0].Name(), "There value should be ufnGetStock");
            });


            test("Aggregate field", function () {
                equal("function", typeof o.TrustedConnection, "Should be observable ");
                deepEqual("AdventureWorks", o.Database());

            });


            test("Custom fields ", function () {
                equal(typeof o.OperationDefinitionCollection, "function", "Custom fields is a function(observable)");
                deepEqual(500, o.OperationDefinitionCollection()[0].ErrorRetry().Wait(), "Custom fields values");
            });

        });
    }, 1000);
});