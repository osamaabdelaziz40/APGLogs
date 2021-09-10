using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Application.EventSourcedNormalizers;
using APGLogs.Application.Interfaces;
using APGLogs.Application.ViewModels;
using APGLogs.Constant;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
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

        public ExceptionLogController(IExceptionLogAppService exceptionLogAppService)
        {
            _exceptionLogAppService = exceptionLogAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<ExceptionLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<ExceptionLogViewModel>> Get()
        {
            return await _exceptionLogAppService.GetAll();
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
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
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

        //[AllowAnonymous]
        //[HttpGet("customer-management/history/{id:guid}")]
        //public async Task<IList<CustomerHistoryData>> History(Guid id)
        //{
        //    return await _customerAppService.GetAllHistory(id);
        //}
    }
}
