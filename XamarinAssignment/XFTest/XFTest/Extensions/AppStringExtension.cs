using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFTest.Extensions
{
    [ContentProperty("Text")]
    public class AppStringExtension : IMarkupExtension
    {
        const string ResourceId = "XFTest.Extensions.AppStrings.Appstrings";
        public string Text { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return Text;
            }

            ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(AppStringExtension).GetTypeInfo().Assembly);
            var currentCulture = CultureInfo.CurrentUICulture;
            var translation = resourceManager.GetString(Text, currentCulture);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text,
                                  ResourceId, currentCulture.Name),
                                 "Text");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
