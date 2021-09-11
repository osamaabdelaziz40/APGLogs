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
    [Route(ServiceNamePortalSessionAuditAction.ServiceName)]
    public class PortalSessionAuditActionController : ApiController
    {
        private readonly IPortalSessionAuditActionAppService _PortalSessionAuditActionAppService;

        public PortalSessionAuditActionController(IPortalSessionAuditActionAppService PortalSessionAuditActionAppService)
        {
            _PortalSessionAuditActionAppService = PortalSessionAuditActionAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<PortalSessionAuditActionViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<PortalSessionAuditActionViewModel>> Get()
        {
            var res = await _PortalSessionAuditActionAppService.GetAll();
            foreach (var item in res)
            {
                item.ActionName = item.ActionName.DecodeBase64();
                item.ActionPath = item.ActionPath.DecodeBase64();
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<PortalSessionAuditActionViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] PortalSessionAuditActionFilter filter)
           => CustomResponse(await _PortalSessionAuditActionAppService.GetAllPaged(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] PortalSessionAuditActionFilter filter)
           => CustomResponse(await _PortalSessionAuditActionAppService.GetAllExported(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PortalSessionAuditActionViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<PortalSessionAuditActionViewModel> Get(Guid id)
        {
            return await _PortalSessionAuditActionAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] PortalSessionAuditActionViewModel PortalSessionAuditActionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            PortalSessionAuditActionViewModel.ActionName = PortalSessionAuditActionViewModel.ActionName.EncodeBase64();
            PortalSessionAuditActionViewModel.ActionPath = PortalSessionAuditActionViewModel.ActionPath.EncodeBase64();
            
            await _PortalSessionAuditActionAppService.Add(PortalSessionAuditActionViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] PortalSessionAuditActionViewModel PortalSessionAuditActionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _PortalSessionAuditActionAppService.Update(PortalSessionAuditActionViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _PortalSessionAuditActionAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
