using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Models;

namespace View.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private readonly DBContext context;

    [ObservableProperty]
    private StatementViewModel statementViewModel;
    [ObservableProperty]
    private FeedbackViewModel feedbackViewModel;
    [ObservableProperty]
    private OwnerViewModel ownerViewModel;
    [ObservableProperty]
    private PlaceViewModel placeViewModel;

    public MainViewModel()
    {
        context = new();

        context.Database.Migrate();

        StatementViewModel = new(context);
        FeedbackViewModel = new(context);
        OwnerViewModel = new(context);
        PlaceViewModel = new(context);
    }
}