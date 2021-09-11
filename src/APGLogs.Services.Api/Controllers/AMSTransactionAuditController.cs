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
    [Route(ServiceNameAMSTransactionAudit.ServiceName)]
    public class AMSTransactionAuditController : ApiController
    {
        private readonly IAMSTransactionAuditAppService _AMSTransactionAuditAppService;

        public AMSTransactionAuditController(IAMSTransactionAuditAppService AMSTransactionAuditAppService)
        {
            _AMSTransactionAuditAppService = AMSTransactionAuditAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<AMSTransactionAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<AMSTransactionAuditViewModel>> Get()
        {
            var res = await _AMSTransactionAuditAppService.GetAll();
            foreach (var item in res)
            {
                item.AuditMessage = item.AuditMessage.DecodeBase64();
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<AMSTransactionAuditViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] AMSTransactionAuditFilter filter)
           => CustomResponse(await _AMSTransactionAuditAppService.GetAllPaged(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] AMSTransactionAuditFilter filter)
           => CustomResponse(await _AMSTransactionAuditAppService.GetAllExported(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<AMSTransactionAuditViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<AMSTransactionAuditViewModel> Get(Guid id)
        {
            return await _AMSTransactionAuditAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] AMSTransactionAuditViewModel AMSTransactionAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            AMSTransactionAuditViewModel.AuditMessage = AMSTransactionAuditViewModel.AuditMessage.EncodeBase64();
            
            await _AMSTransactionAuditAppService.Add(AMSTransactionAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] AMSTransactionAuditViewModel AMSTransactionAuditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _AMSTransactionAuditAppService.Update(AMSTransactionAuditViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _AMSTransactionAuditAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
