namespace Hawk.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Dynamic;

    public static class DynamicExtension
    {
        public static bool IsPropertyExist(dynamic @this, string name)
        {
            if (@this is ExpandoObject)
            {
                return ((IDictionary<string, object>)@this).ContainsKey(name);
            }

            return @this.GetType().GetProperty(name) != null;
        }
    }
}
