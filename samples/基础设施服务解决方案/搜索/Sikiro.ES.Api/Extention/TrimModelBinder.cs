using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sikiro.Common.Utils;

namespace Sikiro.ES.Api.Extention
{
    /// <summary>
    /// 字符串去除空格
    /// </summary>
    public class TrimModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.ThrowIfNull();

            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            bindingContext.Result = ModelBindingResult.Success(value.Trim());
            return Task.CompletedTask;
        }
    }

    public class TrimModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            context.ThrowIfNull();

            if (context.Metadata.ModelType == typeof(string))
                return new TrimModelBinder();

            return null;
        }
    }
}
