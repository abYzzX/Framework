﻿using System;
using System.Threading.Tasks;

namespace Abp.MudBlazorUi.Models;

public class DialogFromOption<TCreateOrUpdateInput> where TCreateOrUpdateInput : class, new()
{
    public TCreateOrUpdateInput Model { get; set; } = new();

    public Func<TCreateOrUpdateInput, Task> OnSubmit { get; set; } = default!;

    public Action OnCancel { get; set; } = default!;
}
