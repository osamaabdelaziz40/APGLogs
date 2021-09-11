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
    [Route(ServiceNameCommunicationLog.ServiceName)]
    public class CommunicationLogController : ApiController
    {
        private readonly ICommunicationLogAppService _communicationLogAppService;

        public CommunicationLogController(ICommunicationLogAppService communicationLogAppService)
        {
            _communicationLogAppService = communicationLogAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<CommunicationLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<CommunicationLogViewModel>> Get()
        {
            var res = await _communicationLogAppService.GetAll();
            foreach (var item in res)
            {
                item.InternalRequest = item.InternalRequest.DecodeBase64();
                item.InternalResponse = item.InternalResponse.DecodeBase64();
                item.ExternalRequest = item.ExternalRequest.DecodeBase64();
                item.ExternalResponse = item.ExternalResponse.DecodeBase64();
                item.ServiceName = item.ServiceName.DecodeBase64();
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<CommunicationLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] CommunicationLogFilter filter)
           => CustomResponse(await _communicationLogAppService.GetAllPaged(filter).ConfigureAwait(false));


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<CommunicationLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.CheckReplayAttach)]
        public async Task<IActionResult> CheckReplayAttach([FromQuery] CommunicationLogFilter filter)
        {
            return CustomResponse(await _communicationLogAppService.CheckReplayAttach(filter).ConfigureAwait(false));

        }



        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] CommunicationLogFilter filter)
           => CustomResponse(await _communicationLogAppService.GetAllExported(filter).ConfigureAwait(false));


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<CommunicationLogViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<CommunicationLogViewModel> Get(Guid id)
        {
            return await _communicationLogAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] CommunicationLogViewModel CommunicationLogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            CommunicationLogViewModel.InternalRequest = CommunicationLogViewModel.InternalRequest.EncodeBase64();
            CommunicationLogViewModel.InternalResponse = CommunicationLogViewModel.InternalResponse.EncodeBase64();
            CommunicationLogViewModel.ExternalRequest = CommunicationLogViewModel.ExternalRequest.EncodeBase64();
            CommunicationLogViewModel.ExternalResponse = CommunicationLogViewModel.ExternalResponse.EncodeBase64();
            CommunicationLogViewModel.ServiceName = CommunicationLogViewModel.ServiceName.EncodeBase64();
            await _communicationLogAppService.Add(CommunicationLogViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] CommunicationLogViewModel CommunicationLogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _communicationLogAppService.Update(CommunicationLogViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _communicationLogAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
