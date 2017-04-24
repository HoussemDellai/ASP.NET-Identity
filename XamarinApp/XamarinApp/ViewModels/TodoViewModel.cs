using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Helpers;
using XamarinApp.Models.Entities;
using XamarinApp.Platform;
using XamarinApp.Services;

namespace XamarinApp.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        private List<Todo> _todos;
        private readonly ToDoServices _todoServices;
        private Todo _todo = new Todo();
        private NavigationService _navigationService;

        public Todo Todo
        {
            get { return _todo; }
            set
            {
                _todo = value;
                OnPropertyChanged();
            }
        }

        public List<Todo> Todos
        {
            get { return _todos; }
            set
            {
                _todos = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTodoCommand
        {
            get { return new Command(async () => await PostTodoAsync()); }
        }

        public ICommand NavigateToAddTodoPageCommand
        {
            get
            {
                return new Command(async() => await _navigationService.NavigateToAddTodoPageAsync());
            }
        }

        private async Task PostTodoAsync()
        {
            await _todoServices.PostTodoAsync(_todo, UserSettings.AccessToken);
        }

        public TodoViewModel()
        {
            _todoServices = new ToDoServices();
            _navigationService = new NavigationService();

            Task.Run(async () => await DoawnloadDataAsync());
        }

        private async Task DoawnloadDataAsync()
        {
            var todos = await _todoServices.GeTodosAsync(UserSettings.AccessToken).ConfigureAwait(false);

            Todos = new List<Todo>(todos);
        }
    }
}
