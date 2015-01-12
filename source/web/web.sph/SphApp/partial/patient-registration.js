define([], function(){
	var name = ko.observable(),
		options = ko.observableArray(),
		
		

		activate = function(patient){
			console.clear();
			console.log("**************************");
			console.log("id read from partial", patient.Id());
			var tcs = new $.Deferred();
			setTimeout(function(){
				tcs.resolve(true);
			}, 500);

			return tcs.promise();


		},
		attached  = function(view){
			$(view).find('input[name="FullName"]').
				css("background-color", "azure");
		},
		hello = function(){

		};

	return {
		activate : activate,
		hello : hello,
		attached : attached,
		name : name,
		options : options

	};

});