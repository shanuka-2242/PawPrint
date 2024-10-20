using PawPrint.Views;

namespace PawPrint
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(VerifyOwnershipView), typeof(VerifyOwnershipView));
            Routing.RegisterRoute(nameof(SignUpView), typeof(SignUpView));
            Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
        }
    }
}
