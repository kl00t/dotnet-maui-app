using System.Diagnostics;
using ToDoMauiClient.DataServices;
using ToDoMauiClient.Models;

namespace ToDoMauiClient.Pages;

[QueryProperty(nameof(ToDo), "ToDo")]
public partial class ManageToDoPage : ContentPage
{
	private readonly IRestDataService _restDataService;
	ToDo _toDo;
	bool _isNew;

	public ToDo ToDo
	{ 
		get => _toDo; 
		set
		{
			_isNew = ManageToDoPage.IsNew(value);
			_toDo = value;
			OnPropertyChanged();
		}
	}

	public ManageToDoPage(IRestDataService restDataService)
	{
		InitializeComponent();

		_restDataService = restDataService;

		BindingContext = this;
	}

	static bool IsNew(ToDo toDo)
	{
		if (toDo.Id == 0)
		{ 
			return true;
        }

        return false;
	}

	async void OnCancelButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}

	async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		await _restDataService.DeleteToDoAsync(ToDo.Id);
        await Shell.Current.GoToAsync("..");
    }

	async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		if(_isNew)
		{
			Debug.WriteLine("Add New Item.");
            await _restDataService.AddToDoAsync(ToDo);
        }
		else
		{
            Debug.WriteLine("Update Item.");
            await _restDataService.UpdateToDoAsync(ToDo);
        }

        await Shell.Current.GoToAsync("..");
    }
}