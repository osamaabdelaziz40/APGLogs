using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Application.EventSourcedNormalizers;
using APGLogs.Application.Interfaces;
using APGLogs.Application.ViewModels;
using APGLogs.Constant;
using APGLogs.Domain.Interfaces;
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
    [Route(ServiceNamePortalSessionAudit.ServiceName)]
    public class PortalSessionAuditController : ApiController
    {
        private readonly IPortalSessionAuditAppService _PortalSessionAuditAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;

        public PortalSessionAuditController(IPortalSessionAuditAppService PortalSessionAuditAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _PortalSessionAuditAppService = PortalSessionAuditAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<PortalSessionAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<PortalSessionAuditViewModel>> Get()
        {
            var res = await _PortalSessionAuditAppService.GetAll();
            foreach (var item in res)
            {
                if (_BackgroundClearTaskSettings.UseBase64Encoding)
                {
                    item.UserName = item.UserName.DecodeBase64();
                    item.IPAddress = item.IPAddress.DecodeBase64();
                } 
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PortalSessionAuditViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<PortalSessionAuditViewModel> Get(Guid id)
        {
            return await _PortalSessionAuditAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] PortalSessionAuditViewModel PortalSessionAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            if (_BackgroundClearTaskSettings.UseBase64Encoding)
            {
                PortalSessionAuditViewModel.IPAddress = PortalSessionAuditViewModel.IPAddress.EncodeBase64();
                PortalSessionAuditViewModel.UserName = PortalSessionAuditViewModel.UserName.EncodeBase64();
            }
            await _PortalSessionAuditAppService.Add(PortalSessionAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] PortalSessionAuditViewModel PortalSessionAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _PortalSessionAuditAppService.Update(PortalSessionAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _PortalSessionAuditAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
