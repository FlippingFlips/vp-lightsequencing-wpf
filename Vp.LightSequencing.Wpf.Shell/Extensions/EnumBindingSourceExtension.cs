using System;
using System.Windows.Markup;

namespace Vp.LightSequencing.Wpf.Shell.Extensions
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public EnumBindingSourceExtension() { }

        public Type EnumType { get; private set; }

        public EnumBindingSourceExtension(Type type = null)
        {
            if (type is null || !type.IsEnum)
                throw new Exception("Enum type cannot be null");

            EnumType = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
