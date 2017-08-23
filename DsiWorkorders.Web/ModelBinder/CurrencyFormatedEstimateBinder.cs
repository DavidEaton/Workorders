using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DsiWorkorders.Data.Enums;
using DsiWorkorders.Web.ViewModels;

namespace DsiWorkorders.Web.ModelBinder
{
  public class CurrencyFormatedEstimateBinder : DefaultModelBinder
  {
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var result = bindingContext.ValueProvider.GetValue("Estimate");

      if (result != null)
      {
        decimal estimate;
        if (Decimal.TryParse(result.AttemptedValue, NumberStyles.Currency, null, out estimate) == false)
        {
          bindingContext.ModelState.AddModelError("Estimate", "invalid Estimate format");
        }
        else
        {
          int? poNumber = null;
          DateTime? closed = null;
          if (!string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("Closed").AttemptedValue))
          {
            closed = DateTime.Parse(bindingContext.ValueProvider.GetValue("Closed").AttemptedValue);
          }

          if (!string.IsNullOrEmpty(bindingContext.ValueProvider.GetValue("PoNumber").AttemptedValue))
          {
            poNumber = int.Parse(bindingContext.ValueProvider.GetValue("PoNumber").AttemptedValue);
          }

          return new WorkorderEditViewModel

                   { //only editable fields + Id
                     Id = int.Parse(bindingContext.ValueProvider.GetValue("Id").AttemptedValue),
                     DepartmentId = int.Parse(bindingContext.ValueProvider.GetValue("DepartmentId").AttemptedValue),
                     Priority = (Priority)Enum.Parse(typeof(Priority), bindingContext.ValueProvider.GetValue("Priority").AttemptedValue),
                     Closed = closed,
                     Details = bindingContext.ValueProvider.GetValue("Details").AttemptedValue,
                     Closer = bindingContext.ValueProvider.GetValue("Closer").AttemptedValue,
                     Resolution = bindingContext.ValueProvider.GetValue("Resolution").AttemptedValue,
                     PoNumber = poNumber,
                     Estimate = estimate
                   };

        }
      }

      return base.BindModel(controllerContext, bindingContext);
    }
  }
}