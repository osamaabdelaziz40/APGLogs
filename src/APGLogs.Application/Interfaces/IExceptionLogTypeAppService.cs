using APGLogs.Application.ViewModels;
using APGLogs.DomainHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Application.Interfaces
{
    public interface IExceptionLogTypeAppService : IDisposable
    {
        Task<IEnumerable<ExceptionLogTypeViewModel>> GetAll();
        Task<ExceptionLogTypeViewModel> GetById(Guid id);
        Task Add(ExceptionLogTypeViewModel exceptionLogTypeViewModel);
        Task Update(ExceptionLogTypeViewModel exceptionLogTypeViewModel);
        Task Remove(Guid id);
    }
}
