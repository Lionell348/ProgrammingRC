using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


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
        private TextBox ModuleCodeTextBox; 
        private TextBox ModuleClassHoursTextBox;
        private TextBox ModuleNameTextBox;
        private TextBox ModuleCreditsTextBox;
        private TextBox HoursStudiedTextBox;
        private ListBox ModuleListBox;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
           
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

        private void RecordStudyHours_Click(object sender, RoutedEventArgs e)
        {
            Module selectedModule = GetSelectedModule();

            DateTime selectedDate = DateTime.Now; 

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
}
