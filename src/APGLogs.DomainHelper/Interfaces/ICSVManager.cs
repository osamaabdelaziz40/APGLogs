using APGLogs.DomainHelper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APGLogs.DomainHelper.Interfaces
{
    public interface ICSVManager
    {
        Task<ExportedCSVFile> Export<T>(List<T> data, string reportName);
    }
}
