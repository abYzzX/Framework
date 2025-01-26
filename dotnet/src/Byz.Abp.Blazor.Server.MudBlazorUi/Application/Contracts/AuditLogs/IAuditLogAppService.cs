using System;
using Abp.MudBlazorUi.Application.Contracts.CommonDtos;
using Volo.Abp.Application.Services;

namespace Abp.MudBlazorUi.Application.Contracts.AuditLogs;

public interface IAuditLogAppService
    : ICrudAppService<AuditLogDto, Guid, GetAuditLogsInput, EmptyCreateDto, EmptyUpdateDto>
{ }
