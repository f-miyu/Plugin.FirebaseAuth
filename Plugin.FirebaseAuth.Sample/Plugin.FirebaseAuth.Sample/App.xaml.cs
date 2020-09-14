using Prism;
using Prism.Ioc;
using Plugin.FirebaseAuth.Sample.ViewModels;
using Plugin.FirebaseAuth.Sample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Plugin.FirebaseAuth.Sample
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var types = GetType().Assembly.GetTypes()
                                 .Where(t => t.Namespace == "Plugin.FirebaseAuth.Sample.Views");

            foreach (var type in types)
            {
                containerRegistry.RegisterForNavigation(type, type.Name);
            }

            containerRegistry.RegisterForNavigation<NavigationPage>();
        }
    }
}
