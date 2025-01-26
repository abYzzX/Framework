using Abp.MudBlazorUi.Application.Contracts.AuditLogs;
using Abp.MudBlazorUi.Models;
using AutoMapper;
using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;

namespace Abp.MudBlazorUi;

public class AbpMudBlazorUiAutoMapperProfile : Profile
{
    public AbpMudBlazorUiAutoMapperProfile()
    {
        CreateMap<ProfileDto, PersonalInfoModel>()
            .MapExtraProperties()
            .Ignore(x => x.PhoneNumberConfirmed)
            .Ignore(x => x.EmailConfirmed);

        CreateMap<PersonalInfoModel, UpdateProfileDto>().MapExtraProperties();
        CreateMap<AuditLog, AuditLogDto>();
    }
}
