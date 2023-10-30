#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace View.ViewModels;
public partial class StatementViewModel : ObservableObject
{
    private readonly DBContext context;

    [ObservableProperty]
    private IEnumerable<Statement> statements;

    [ObservableProperty]
    private Statement selectedItem;

    [ObservableProperty]
    private IEnumerable<string> states = new List<string>() 
    {
        "У черзі",
        "В обробці",
        "Схвалено",
        "Відхилено"
    };

    public StatementViewModel(DBContext context)
    {
        this.context = context;

        Statements = context.Statements.ToList();
    }

    [RelayCommand]
    private void StateChanged()
    {
        context.Update(SelectedItem);
        context.SaveChanges();
    }
}
