using Lab2_Solodovnyk.Views;

namespace Lab2_Solodovnyk;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("studentdetail", typeof(StudentDetailPage));
    }
}