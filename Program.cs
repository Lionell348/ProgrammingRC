using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace TimeManagementApp
{
    public class Module
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public int ClassHoursPerWeek { get; set; }
        public int SelfStudyHoursPerWeek { get; set; }
        public Dictionary<DateTime, int> StudyHoursRecord { get; set; }

        public Module()
        {
            StudyHoursRecord = new Dictionary<DateTime, int>();
        }
    }

    public partial class MainWindow : Window
    {
        private List<Module> modules = new List<Module>();
        private int numberOfWeeks;
        private DateTime startDate;
        private object ModuleCodeTextBox;
        private object ModuleClassHoursTextBox;
        private readonly object ModuleNameTextBox;

        public object ModuleCreditsTextBox { get; private set; }
        public object HoursStudiedTextBox { get; private set; }
        public object ModuleListBox { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void AddModule_Click(object sender, RoutedEventArgs e)
        {
            

            Module newModule = new Module
            {
                Code = ModuleCodeTextBox.Text,
                Name = ModuleNameTextBox.Text,
                Credits = int.Parse(ModuleCreditsTextBox.Text),
                ClassHoursPerWeek = int.Parse(ModuleClassHoursTextBox.Text),
                SelfStudyHoursPerWeek = 0,
            };

            modules.Add(newModule);

            

            RefreshModuleList();
        }

        private void CalculateSelfStudyHours_Click(object sender, RoutedEventArgs e)
        {
           

            foreach (Module module in modules)
            {
                module.SelfStudyHoursPerWeek = (module.Credits * 10) - module.ClassHoursPerWeek;
                

            }
        }

        private void RecordStudyHours_Click(object sender, RoutedEventArgs e, object DatePicker)
        {
            
            Module selectedModule = GetSelectedModule();

            
            DateTime selectedDate = DatePicker.SelectedDate ?? DateTime.Now; 

            int hoursStudied = int.Parse(HoursStudiedTextBox.Text);

            if (selectedModule != null)
            {
                if (selectedModule.StudyHoursRecord.ContainsKey(selectedDate))
                {
                    selectedModule.StudyHoursRecord[selectedDate] += hoursStudied;
                }
                else
                {
                    selectedModule.StudyHoursRecord[selectedDate] = hoursStudied;
                }

                UpdateRemainingSelfStudyHours(selectedModule);
            }
        }

        private Module GetSelectedModule()
        {
            throw new NotImplementedException();
        }

        private Module GetSelectedModule(object ModuleListBox)
        {
            int selectedIndex = ModuleListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < modules.Count)
            {
                return modules[selectedIndex];
            }

            return null;
        }


        private void UpdateRemainingSelfStudyHours(Module selectedModule)
        {
            int totalStudyHoursThisWeek = selectedModule.StudyHoursRecord
                .Where(pair => IsInCurrentWeek(pair.Key))
                .Sum(pair => pair.Value);

            int remainingSelfStudyHours = selectedModule.SelfStudyHoursPerWeek - totalStudyHoursThisWeek;

            RemainingSelfStudyHoursLabel.Content = $"Remaining Self-Study Hours This Week: {remainingSelfStudyHours} hours";
        }


        private bool IsInCurrentWeek(DateTime date)
        {
           
            int currentWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
            int selectedWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

            return currentWeekNumber == selectedWeekNumber;
        }

        private void RefreshModuleList()
        {
            ModuleListBox.ItemsSource = modules;
        }

    }

    internal class RemainingSelfStudyHoursLabel
    {
        public static string Content { get; internal set; }
    }

    internal class RoutedEventArgs
    {
    }

    public class Window
    {
    }
}
