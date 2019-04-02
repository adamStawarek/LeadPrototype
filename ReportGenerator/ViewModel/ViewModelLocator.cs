using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;


namespace ReportGenerator.ViewModel
{

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get=>ServiceLocator.Current.GetInstance<MainViewModel>();
           
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}