namespace Fanex.RemotingRef.Service.ModelBinders
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    public class NullOrEmptyStringModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(string))
            {
                var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                string stringValue = val == null
                    ? null
                    : val.RawValue as string;

                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    bindingContext.Model = stringValue;

                    return true;
                }
            }

            return false;
        }
    }
}