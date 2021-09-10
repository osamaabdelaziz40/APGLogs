using System.Reflection;

namespace APGLogs.DomainHelper.Attributes
{
    public class ExportProperty
    {
        public PropertyInfo PropertyInfo { get; set; }

        public ExportAttribute ExportAttribute { get; set; }
    }
}
