namespace Collector.Common.RestContracts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// SensitiveString indicates that a string contains sensitive information.
    /// <example>
    ///     <para>Example: [SensitiveProperty] public string NewPassword { get; set; }</para>
    /// </example>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SensitiveAttribute : Attribute
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, SensitiveAttribute>> CachedSensitiveProperties = new ConcurrentDictionary<Type, IReadOnlyDictionary<string, SensitiveAttribute>>();

        const string DefaultMask = "***";

        public string Text { get; set; } = DefaultMask;
        public int ShowFirst { get; set; }
        public int ShowLast { get; set; }
        public bool PreserveLength { get; set; }

        internal bool IsDefaultMask()
        {
            return Text == DefaultMask;
        }

        internal string FormatMaskedValue(object propValue)
        {
            var val = propValue?.ToString();

            if (string.IsNullOrEmpty(val))
                return val;

            if (ShowFirst == 0 && ShowLast == 0)
            {
                if (PreserveLength)
                    return new string(Text[0], val.Length);

                return Text;
            }

            if (ShowFirst > 0 && ShowLast == 0)
            {
                var first = val.Substring(0, Math.Min(ShowFirst, val.Length));

                if (!PreserveLength || !IsDefaultMask())
                    return first + Text;

                var mask = "";
                if (ShowFirst <= val.Length)
                    mask = new string(Text[0], val.Length - ShowFirst);

                return first + mask;

            }

            if (ShowFirst == 0 && ShowLast > 0)
            {
                var last = ShowLast > val.Length ? val : val.Substring(val.Length - ShowLast);

                if (!PreserveLength || !IsDefaultMask())
                    return Text + last;

                var mask = "";
                if (ShowLast <= val.Length)
                    mask = new string(Text[0], val.Length - ShowLast);

                return mask + last;
            }

            if (ShowFirst > 0 && ShowLast > 0)
            {
                if (ShowFirst + ShowLast >= val.Length)
                    return val;

                var first = val.Substring(0, ShowFirst);
                var last = val.Substring(val.Length - ShowLast);

                return first + Text + last;
            }

            return DefaultMask;
        }

        internal static IReadOnlyDictionary<string, SensitiveAttribute> GetSensitiveProperties(Type type)
        {
            return CachedSensitiveProperties.GetOrAdd(key: type, valueFactory: GetSensitivePropertiesWithReflection);
        }

        private static IReadOnlyDictionary<string, SensitiveAttribute> GetSensitivePropertiesWithReflection(Type newKey)
        {
#if NET45
            var propertyInfos = newKey.GetProperties();
#endif
#if NETSTANDARD1_6
            var propertyInfos = newKey.GetTypeInfo().GetProperties();
#endif
            return new ReadOnlyDictionary<string, SensitiveAttribute>(
                propertyInfos
                    .Select(p => new { p.Name, Attribute = p.GetCustomAttributes(typeof(SensitiveAttribute), true).SingleOrDefault() as SensitiveAttribute })
                    .Where(o => o.Attribute != null)
                    .ToDictionary(o => o.Name, o => o.Attribute));
        }
    }
}