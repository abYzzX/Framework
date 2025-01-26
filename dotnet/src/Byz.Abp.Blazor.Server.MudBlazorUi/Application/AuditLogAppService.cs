using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.MudBlazorUi.Application.Contracts.AuditLogs;
using Abp.MudBlazorUi.Application.Contracts.CommonDtos;
using Abp.MudBlazorUi.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging;
using Volo.Abp.Domain.Repositories;

namespace Abp.MudBlazorUi.Application;

public class AuditLogAppService
    : CrudAppService<
        AuditLog,
        AuditLogDto,
        Guid,
        GetAuditLogsInput,
        EmptyCreateDto,
        EmptyUpdateDto
    >,
        IAuditLogAppService
{
    public AuditLogAppService(IRepository<AuditLog, Guid> repository)
        : base(repository)
    {
        GetPolicyName = MudBlazorUiPermissions.AuditLogs.Default;
        GetListPolicyName = MudBlazorUiPermissions.AuditLogs.Default;
    }

    protected override async Task<IQueryable<AuditLog>> CreateFilteredQueryAsync(
        GetAuditLogsInput input
    )
    {
        if (input.Sorting.IsNullOrEmpty())
        {
            input.Sorting = $"{nameof(AuditLog.ExecutionTime)} desc";
        }

        var query = await base.CreateFilteredQueryAsync(input);

        if (!string.IsNullOrEmpty(input.Filter))
        {
            query = query.Where(input.Filter);
        }

        return query;
    }
}
