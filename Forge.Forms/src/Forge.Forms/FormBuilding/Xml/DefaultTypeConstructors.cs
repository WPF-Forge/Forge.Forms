using System;
using Forge.Forms.Annotations;

namespace Forge.Forms.FormBuilding.Xml
{
    internal static class DefaultTypeConstructors
    {
        public static TypeConstructor NullableTime(TypeConstructionContext context)
        {
            return Time(context, true);
        }

        public static TypeConstructor Time(TypeConstructionContext context)
        {
            return Time(context, false);
        }

        private static TypeConstructor Time(TypeConstructionContext context, bool nullable)
        {
            object is24Hours = false;
            if (context is XmlConstructionContext xmlContext)
            {
                var e = xmlContext.Element;
                is24Hours = e.TryGetAttribute("is24hours");
            }

            return new TypeConstructor(
                nullable ? typeof(DateTime?) : typeof(DateTime),
                new TimeAttribute
                {
                    Is24Hours = is24Hours
                });
        }
    }
}
