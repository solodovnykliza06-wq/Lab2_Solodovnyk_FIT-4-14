namespace Lab2_Solodovnyk.Views;

public partial class StudentDetailPage : ContentPage, IQueryAttributable
{
    public StudentDetailPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is IQueryAttributable viewModel)
            viewModel.ApplyQueryAttributes(query);
    }
}