using System;
using System.ComponentModel;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Reflects built-in SharePoint BrowserFileHandling class.
    /// </summary>

    [Flags]
    public enum BuiltInBrowserFileHandling
    {
        [Description("Permissive mode of handling files. Value is 0.")]
        Permissive = 0,
        [Description("Strict mode of handling files. Value is 1.")]
        Strict = 1
    }
}
