#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace View.ViewModels;
public partial class OwnerViewModel : ObservableObject
{
    private readonly DBContext context;

    [ObservableProperty]
    private IEnumerable<Owner> owners;

    [ObservableProperty]
    private Owner selectedItem;

    public OwnerViewModel(DBContext context)
    {
        this.context = context;

        Owners = context.Owners.ToList();
    }
}