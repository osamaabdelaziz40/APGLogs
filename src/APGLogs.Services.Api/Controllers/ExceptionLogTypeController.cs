using System;
using System.Collections.Generic;
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
    [Route(ServiceNameExceptionLogType.ServiceName)] //Test Commit
    public class ExceptionLogTypeController : ApiController
    {
        private readonly IExceptionLogTypeAppService _exceptionLogTypeAppService;

        public ExceptionLogTypeController(IExceptionLogTypeAppService exceptionLogTypeAppService)
        {
            _exceptionLogTypeAppService = exceptionLogTypeAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<ExceptionLogTypeViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<ExceptionLogTypeViewModel>> Get()
        {
            return await _exceptionLogTypeAppService.GetAll();
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] ExceptionLogTypeViewModel exceptionLogTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _exceptionLogTypeAppService.Add(exceptionLogTypeViewModel);
            return CustomResponse(true);
        }

    }
}
