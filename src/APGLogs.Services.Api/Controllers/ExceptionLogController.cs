using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APGLogs.Application.EventSourcedNormalizers;
using APGLogs.Application.Interfaces;
using APGLogs.Application.ViewModels;
using APGLogs.Constant;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using APGLogs.DomainHelper.Services;
using APGLogs.Services.Api.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.Identity.Authorization;

namespace APGLogs.Services.Api.Controllers
{
    //[Authorize]
    [Route(ServiceNameExceptionLog.ServiceName)]
    public class ExceptionLogController : ApiController
    {
        private readonly IExceptionLogAppService _exceptionLogAppService;
        private readonly IExceptionLogTypeAppService _exceptionLogTypeAppService;

        public ExceptionLogController(IExceptionLogAppService exceptionLogAppService, IExceptionLogTypeAppService exceptionLogTypeAppService)
        {
            _exceptionLogAppService = exceptionLogAppService;
            _exceptionLogTypeAppService = exceptionLogTypeAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<ExceptionLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<ExceptionLogViewModel>> Get()
        {
            try
            {
                var res = await _exceptionLogAppService.GetAll();
                foreach (var item in res)
                {
                    item.InnerException = item.InnerException.DecodeBase64();
                    item.StackTrace = item.StackTrace.DecodeBase64();
                    item.Message = item.Message.DecodeBase64();
                }
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<ExceptionLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] ExceptionLogFilter filter)
           => CustomResponse(await _exceptionLogAppService.GetAllPaged(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] ExceptionLogFilter filter)
           => CustomResponse(await _exceptionLogAppService.GetAllExported(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExceptionLogViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<ExceptionLogViewModel> Get(Guid id)
        {
            return await _exceptionLogAppService.GetById(id);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<ExceptionLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameExceptionLog.GetByCommunicationLogId)]
        public async Task<IEnumerable<ExceptionLogViewModel>> GetByCommunicationLogId(Guid id)
        {
            return await _exceptionLogAppService.GetByCommunicationLogId(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] ExceptionLogViewModel exceptionLogViewModel)
        {
            Guid guidOutput = Guid.NewGuid();
            bool isValid = Guid.TryParse(exceptionLogViewModel.ExceptionType, out guidOutput);

            var exceptionLogType = !isValid ? null: _exceptionLogTypeAppService.GetById(Guid.Parse(exceptionLogViewModel.ExceptionType));
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }else if (exceptionLogType.Result == null)
            {
                return CustomResponse(false);
            }
            exceptionLogViewModel.InnerException = exceptionLogViewModel.InnerException.EncodeBase64();
            exceptionLogViewModel.StackTrace = exceptionLogViewModel.StackTrace.EncodeBase64();
            exceptionLogViewModel.Message = exceptionLogViewModel.Message.EncodeBase64();
            await _exceptionLogAppService.Add(exceptionLogViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] ExceptionLogViewModel exceptionLogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _exceptionLogAppService.Update(exceptionLogViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exceptionLogAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
