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
    [Route(ServiceNameEmailLog.ServiceName)]
    public class EmailLogController : ApiController
    {
        private readonly IEmailLogAppService _EmailLogAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;

        public EmailLogController(IEmailLogAppService EmailLogAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _EmailLogAppService = EmailLogAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<IEnumerable<EmailLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAll)]
        public async Task<IEnumerable<EmailLogViewModel>> Get()
        {
            var res = await _EmailLogAppService.GetAll();
            foreach (var item in res)
            {
                if (_BackgroundClearTaskSettings.UseBase64Encoding)
                {
                    item.Status = item.Status.DecodeBase64();
                    item.ToEmail = item.ToEmail.DecodeBase64();
                }
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<PaginatedResult<EmailLogViewModel>>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllPaged)]
        public async Task<IActionResult> GetAllPaged([FromQuery] EmailLogFilter filter)
           => CustomResponse(await _EmailLogAppService.GetAllPaged(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<ExportViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetAllExported)]
        public async Task<IActionResult> Export([FromQuery] EmailLogFilter filter)
           => CustomResponse(await _EmailLogAppService.GetAllExported(filter).ConfigureAwait(false));

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<EmailLogViewModel>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.GetById)]
        public async Task<EmailLogViewModel> Get(Guid id)
        {
            return await _EmailLogAppService.GetById(id);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Add)]
        public async Task<IActionResult> Post([FromBody] EmailLogViewModel EmailLogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            if (_BackgroundClearTaskSettings.UseBase64Encoding)
            {
                EmailLogViewModel.Status = EmailLogViewModel.Status.EncodeBase64();
                EmailLogViewModel.ToEmail = EmailLogViewModel.ToEmail.EncodeBase64();
            }

            await _EmailLogAppService.Add(EmailLogViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Update)]
        public async Task<IActionResult> Put([FromBody] EmailLogViewModel EmailLogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _EmailLogAppService.Update(EmailLogViewModel);
            return CustomResponse(true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GenericSuccessResponse<>), StatusCodes.Status200OK)]
        [Route(ServiceNameCommon.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _EmailLogAppService.Remove(id);
            return CustomResponse(true);
        }
    }
}
