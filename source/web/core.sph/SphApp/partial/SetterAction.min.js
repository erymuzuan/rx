bespoke.sph.domain.SetterActionPartial=function(){var n=require("durandal/system"),t=function(n){var t=this;return function(){t.SetterActionChildCollection.remove(n)}},i=function(){var t=new bespoke.sph.domain.SetterActionChild(n.guid());t.Field({Name:ko.observable("+ Field")});this.SetterActionChildCollection.push(t)};return{addChildAction:i,removeChildAction:t}};
/*
//# sourceMappingURL=SetterAction.min.js.map
*/