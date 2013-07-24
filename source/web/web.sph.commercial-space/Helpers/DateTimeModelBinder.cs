using System;
using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                           ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            DateTime date;
            if (DateTime.TryParse(valueResult.AttemptedValue, out date))
            {
                actualValue = date;
            }
            else
            {
                modelState.Errors.Add("Cannot convert to date time " + valueResult.AttemptedValue);
            }


            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
    public class RuleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                           ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            DateTime date;
            if (DateTime.TryParse(valueResult.AttemptedValue, out date))
            {
                actualValue = date;
            }
            else
            {
                modelState.Errors.Add("Cannot convert to date time " + valueResult.AttemptedValue);
            }


            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}