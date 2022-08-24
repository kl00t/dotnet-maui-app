using System.Diagnostics;
using ToDoMauiClient.DataServices;
using ToDoMauiClient.Models;
using ToDoMauiClient.Pages;

namespace ToDoMauiClient
{
    public partial class MainPage : ContentPage
    {
        private readonly IRestDataService _restDataService;

        public MainPage(IRestDataService restDataService)
        {
            InitializeComponent();

            _restDataService = restDataService;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            collectionView.ItemsSource = await _restDataService.GetAllToDosAsync();
        }

        async void OnAddToDoClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Add Button Clicked.");
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(ToDo), new ToDo() }
            };

            await Shell.Current.GoToAsync(nameof(ManageToDoPage), navigationParameter);
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Item Changed Clicked.");
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(ToDo), e.CurrentSelection.FirstOrDefault() as ToDo }
            };

            await Shell.Current.GoToAsync(nameof(ManageToDoPage), navigationParameter);
        }
    }
}