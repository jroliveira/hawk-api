namespace Hawk.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Dynamic;

    public static class DynamicExtension
    {
        public static bool IsPropertyExist(in dynamic @this, in string name)
        {
            if (@this is ExpandoObject)
            {
                return ((IDictionary<string, object>)@this).ContainsKey(name);
            }

            return @this.GetType().GetProperty(name) != null;
        }
    }
}
