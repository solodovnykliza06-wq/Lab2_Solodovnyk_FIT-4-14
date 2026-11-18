using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Lab2_Solodovnyk.Models;

namespace Lab2_Solodovnyk.ViewModels;

public class StudentViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private readonly Student _student = new();
    private Student? _selectedStudent;

    public StudentViewModel()
    {
        AddStudentCommand = new Command(AddStudent, CanAddStudent);
        OpenDetailsCommand = new Command<Student>(async (student) => await OpenDetailsAsync(student));
    }

    public string FullName
    {
        get => _student.FullName;
        set
        {
            if (_student.FullName != value)
            {
                _student.FullName = value;
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(Greeting));
                ((Command)AddStudentCommand).ChangeCanExecute();
            }
        }
    }

    public string Group
    {
        get => _student.Group;
        set
        {
            if (_student.Group != value)
            {
                _student.Group = value;
                OnPropertyChanged(nameof(Group));
                OnPropertyChanged(nameof(Greeting));
            }
        }
    }

    public double AverageScore
    {
        get => _student.AverageScore;
        set
        {
            if (_student.AverageScore != value)
            {
                _student.AverageScore = value;
                OnPropertyChanged(nameof(AverageScore));
                OnPropertyChanged(nameof(IsHighScore));
            }
        }
    }

    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            if (_selectedStudent != value)
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }
    }

    public string Greeting => $"Студент: {FullName}, група {Group}";

    public bool IsHighScore => AverageScore >= 4.0;

    public ObservableCollection<Student> Students { get; } = new();

    public ICommand AddStudentCommand { get; }
    public ICommand OpenDetailsCommand { get; }

    private void AddStudent()
    {
        Students.Add(new Student
        {
            FullName = FullName,
            Group = Group,
            AverageScore = AverageScore
        });

        FullName = string.Empty;
        Group = string.Empty;
    }

    private bool CanAddStudent() => !string.IsNullOrWhiteSpace(FullName);

    // Навігація на сторінку деталей (ініціюється командою ViewModel)
    private async Task OpenDetailsAsync(Student? student)
    {
        if (student is null) return;

        SelectedStudent = null; // скидаємо виділення, щоб можна було обрати того ж студента ще раз

        var parameters = new Dictionary<string, object>
        {
            { "SelectedStudent", student }
        };

        await Shell.Current.GoToAsync("studentdetail", parameters);
    }

    // Отримання оновленого студента після повернення зі сторінки деталей
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("UpdatedStudent", out var value) && value is Student updated)
        {
            for (int i = 0; i < Students.Count; i++)
            {
                if (Students[i].Id == updated.Id)
                {
                    Students[i] = updated; // заміна елемента оновлює CollectionView
                    break;
                }
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}