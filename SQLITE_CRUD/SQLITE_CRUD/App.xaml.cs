using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SQLITE_CRUD
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            if (Application.Current.Properties.ContainsKey("TEMA"))
            {
                string tema = (string)Application.Current.Properties["TEMA"];
                if (tema == "Azul")
                {
                    DependencyService.Get<ITemas>().TemaAzul();
                }
                else if (tema == "Verde")
                {
                    DependencyService.Get<ITemas>().TemaVerde();
                }
                else if (tema == "Vermelho")
                {
                    DependencyService.Get<ITemas>().TemaVermelho();
                }
                else if (tema == "Roxo")
                {
                    DependencyService.Get<ITemas>().TemaRoxo();
                }

                else if (tema == "Claro")
                {
                    DependencyService.Get<ITemas>().TemaClaro();
                }
            }
            else
            {
                DependencyService.Get<ITemas>().TemaAzul();
                Application.Current.Properties["TEMA"] = "Azul";

            }

            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
