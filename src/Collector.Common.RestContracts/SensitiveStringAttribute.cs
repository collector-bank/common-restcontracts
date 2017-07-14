namespace Collector.Common.RestContracts
{
    using System;

    /// <summary>
    /// SensitiveString indicates that a string contains sensitive information.
    /// <example>
    ///     <para>Example: [SensitiveString] public string NewPassword { get; set; }</para>
    /// </example>
    /// </summary>
    public class SensitiveStringAttribute : Attribute
    {
    }
}