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
    [Route(ServiceNameCommunicationLogType.ServiceName)]
    public class CommunicationLogTypeController : ApiController
    {
        private readonly ICommunicationLogTypeAppService _communicationLogTypeAppService;

        public CommunicationLogTypeController(ICommunicationLogTypeAppService communicationLogTypeAppService)
        {
            _communicationLogTypeAppService = communicationLogTypeAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<CommunicationLogTypeViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<CommunicationLogTypeViewModel>> Get()
        {
            return await _communicationLogTypeAppService.GetAll();
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] CommunicationLogTypeViewModel CommunicationLogTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _communicationLogTypeAppService.Add(CommunicationLogTypeViewModel);
            return CustomResponse(true);
        }

    }
}
