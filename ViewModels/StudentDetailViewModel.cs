using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab2_Solodovnyk.Models;

namespace Lab2_Solodovnyk.ViewModels;

public class StudentDetailViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private Guid _id;
    private string _fullName = string.Empty;
    private string _group = string.Empty;
    private double _averageScore;

    public StudentDetailViewModel()
    {
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        SaveCommand = new Command(async () => await SaveAsync());
    }

    public string FullName
    {
        get => _fullName;
        private set => SetField(ref _fullName, value);
    }

    public string Group
    {
        get => _group;
        private set => SetField(ref _group, value);
    }

    public double AverageScore
    {
        get => _averageScore;
        set => SetField(ref _averageScore, Math.Round(value, 1));
    }

    public ICommand GoBackCommand { get; }
    public ICommand SaveCommand { get; }

    // Отримання студента, переданого зі сторінки списку
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("SelectedStudent", out var value) && value is Student student)
        {
            _id = student.Id;
            FullName = student.FullName;
            Group = student.Group;
            AverageScore = student.AverageScore;
        }
    }

    // Повернення назад з оновленим студентом
    private async Task SaveAsync()
    {
        var updated = new Student
        {
            Id = _id,
            FullName = FullName,
            Group = Group,
            AverageScore = AverageScore
        };

        var parameters = new Dictionary<string, object>
        {
            { "UpdatedStudent", updated }
        };

        await Shell.Current.GoToAsync("..", parameters);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string? propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}