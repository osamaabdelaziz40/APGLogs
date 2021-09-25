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
    [Route(ServiceNameAMSBalanceAudit.ServiceName)]
    public class AMSBalanceAuditController : ApiController
    {
        private readonly IAMSBalanceAuditAppService _AMSBalanceAuditAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public AMSBalanceAuditController(IAMSBalanceAuditAppService AMSBalanceAuditAppService , 
            IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _AMSBalanceAuditAppService = AMSBalanceAuditAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<AMSBalanceAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<AMSBalanceAuditViewModel>> Get()
        {
            var res = await _AMSBalanceAuditAppService.GetAll();
            foreach (var item in res)
            {
                item.AuditMessage = _BackgroundClearTaskSettings.UseBase64Encoding ? item.AuditMessage.DecodeBase64(): item.AuditMessage;
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<AMSBalanceAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] AMSBalanceAuditFilter filter)
           => CustomResponse(await _AMSBalanceAuditAppService.GetAllPaged(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] AMSBalanceAuditFilter filter)
           => CustomResponse(await _AMSBalanceAuditAppService.GetAllExported(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<AMSBalanceAuditViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<AMSBalanceAuditViewModel> Get(Guid id)
        {
            return await _AMSBalanceAuditAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] AMSBalanceAuditViewModel AMSBalanceAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            AMSBalanceAuditViewModel.AuditMessage = _BackgroundClearTaskSettings.UseBase64Encoding?AMSBalanceAuditViewModel.AuditMessage.EncodeBase64(): AMSBalanceAuditViewModel.AuditMessage;
            
            await _AMSBalanceAuditAppService.Add(AMSBalanceAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] AMSBalanceAuditViewModel AMSBalanceAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _AMSBalanceAuditAppService.Update(AMSBalanceAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _AMSBalanceAuditAppService.Remove(id);
            return CustomResponse(true);
        }

    }
}
