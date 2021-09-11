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
    [Route(ServiceNameShadowBalanceAudit.ServiceName)]
    public class ShadowBalanceAuditController : ApiController
    {
        private readonly IShadowBalanceAuditAppService _ShadowBalanceAuditAppService;

        public ShadowBalanceAuditController(IShadowBalanceAuditAppService ShadowBalanceAuditAppService)
        {
            _ShadowBalanceAuditAppService = ShadowBalanceAuditAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<ShadowBalanceAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<ShadowBalanceAuditViewModel>> Get()
        {
            var res = await _ShadowBalanceAuditAppService.GetAll();
            foreach (var item in res)
            {
                item.AuditMessage = item.AuditMessage.DecodeBase64();
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<ShadowBalanceAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] ShadowBalanceAuditFilter filter)
           => CustomResponse(await _ShadowBalanceAuditAppService.GetAllPaged(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] ShadowBalanceAuditFilter filter)
           => CustomResponse(await _ShadowBalanceAuditAppService.GetAllExported(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ShadowBalanceAuditViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<ShadowBalanceAuditViewModel> Get(Guid id)
        {
            return await _ShadowBalanceAuditAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] ShadowBalanceAuditViewModel ShadowBalanceAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            ShadowBalanceAuditViewModel.AuditMessage = ShadowBalanceAuditViewModel.AuditMessage.EncodeBase64();
            
            await _ShadowBalanceAuditAppService.Add(ShadowBalanceAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] ShadowBalanceAuditViewModel ShadowBalanceAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _ShadowBalanceAuditAppService.Update(ShadowBalanceAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _ShadowBalanceAuditAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
