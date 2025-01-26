using Volo.Abp.Application.Dtos;

namespace Abp.MudBlazorUi.Application.Contracts.CommonDtos;

public class PagedFilterAndSortedResultRequestDto : ExtensiblePagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
