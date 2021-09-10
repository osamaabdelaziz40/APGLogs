using System;

namespace APGLogs.DomainHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class ExportAttribute : Attribute
    {
        public string ExportName { get; set; }
        public string Format { get; set; }
        public int Order { get; set; }
    }
}
