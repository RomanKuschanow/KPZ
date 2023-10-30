#nullable disable
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace View.ViewModels;
public partial class FeedbackViewModel : ObservableObject
{
    private readonly DBContext context;

    [ObservableProperty]
    private IEnumerable<Feedback> feedbacks;

    [ObservableProperty]
    private Feedback selectedItem;

    public FeedbackViewModel(DBContext context)
    {
        this.context = context;

        Feedbacks = context.Feedbacks.ToList();
    }
}
