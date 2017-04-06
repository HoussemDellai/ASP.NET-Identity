using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Views;

namespace XamarinApp.Platform
{
    public class NavigationService
    {
        private Page GetCurrentPage()
        {
            return Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
        }

        public async Task NavigateToSigninPage()
        {
            var currentPage = GetCurrentPage();
            await currentPage.Navigation.PushModalAsync(new SigninPage());
        }

        public async Task NavigateBack()
        {
            var currentPage = GetCurrentPage();
            await currentPage.Navigation.PopModalAsync();
        }
    }
}
