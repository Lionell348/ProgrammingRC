using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;

namespace RecipeApplication
{
    public partial class MainWindow : Window
    {
        private RecipeManager recipeManager;
        private List<Recipe> filteredRecipes;

        public MainWindow()
        {
            InitializeComponent();

            recipeManager = new RecipeManager();
            // Add some sample recipes for testing
            recipeManager.AddRecipe(new Recipe("Recipe 1"));
            recipeManager.AddRecipe(new Recipe("Recipe 2"));
            recipeManager.AddRecipe(new Recipe("Recipe 3"));

            recipeListBox.ItemsSource = recipeManager.Recipes;

            filteredRecipes = new List<Recipe>();
        }

        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            addRecipeWindow.ShowDialog();

            // After the dialog is closed, check if a recipe was added
            if (addRecipeWindow.DialogResult.HasValue && addRecipeWindow.DialogResult.Value)
            {
                Recipe newRecipe = addRecipeWindow.GetRecipe();
                recipeManager.AddRecipe(newRecipe);
                filteredRecipes.Add(newRecipe);
                recipeListBox.ItemsSource = filteredRecipes;
            }
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Recipe selectedRecipe = recipeListBox.SelectedItem as Recipe;
            if (selectedRecipe != null)
            {
                
                RecipeDetailsWindow recipeDetailsWindow = new RecipeDetailsWindow(selectedRecipe);
                recipeDetailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a recipe.");
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredientFilter = ingredientFilterTextBox.Text;
            string foodGroupFilter = foodGroupComboBox.Text;
            double maxCaloriesFilter = 0;
            double.TryParse(maxCaloriesTextBox.Text, out maxCaloriesFilter);

           
            filteredRecipes = recipeManager.FilterRecipes(ingredientFilter, foodGroupFilter, maxCaloriesFilter);

            // Update the recipe list box with the filtered recipes
            recipeListBox.ItemsSource = filteredRecipes;

            // Reset the selected recipe to avoid confusion
            recipeListBox.SelectedItem = null;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (filteredRecipes.Count > 0)
            {
                MenuWindow menuWindow = new MenuWindow(filteredRecipes);
                menuWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please filter the recipes first.");
            }
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            
            ingredientFilterTextBox.Clear();
            foodGroupComboBox.SelectedIndex = -1;
            maxCaloriesTextBox.Clear();

            filteredRecipes = new List<Recipe>(recipeManager.Recipes);
            recipeListBox.ItemsSource = filteredRecipes;
        }
    }

    public partial class AddRecipeWindow : Window
    {
        public AddRecipeWindow()
        {
            InitializeComponent();
        }

        public Recipe GetRecipe()
        {
            
            string recipeName = recipeNameTextBox.Text;
            return new Recipe(recipeName);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(recipeNameTextBox.Text))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog window without saving the recipe
            DialogResult = false;
            Close();
        }
    }

    public partial class RecipeDetailsWindow : Window
    {
        public RecipeDetailsWindow(Recipe recipe)
        {
            InitializeComponent();

            // Display the details of the selected recipe
            recipeNameLabel.Content = recipe.Name;
           
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
          
            Close();
        }
    }

    public partial class MenuWindow : Window
    {
        public MenuWindow(List<Recipe> recipes)
        {
            InitializeComponent();

            
            GenerateMenu(recipes);
            DisplayPieChart(recipes);
        }

        private void GenerateMenu(List<Recipe> recipes)
        {
            
            menuListBox.ItemsSource = recipes;
        }

        private void DisplayPieChart(List<Recipe> recipes)
        {
           
            Chart chart = new Chart();
            chart.Title = "Recipe Distribution by Food Group";

            
            PieSeries pieSeries = new PieSeries();
            pieSeries.Title = "Food Group";
            foreach (Recipe recipe in recipes)
            {
                pieSeries.Items.Add(new KeyValuePair<string, int>(recipe.FoodGroup, 1));
            }
            chart.Series.Add(pieSeries);

            // Add the chart to a container (e.g., Grid) in the window
            chartContainer.Children.Add(chart);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog window
            Close();
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public string FoodGroup { get; set; }

        public Recipe(string name)
        {
            Name = name;
        }
    }

    public class RecipeManager
    {
        public List<Recipe> Recipes { get; }

        public RecipeManager()
        {
            Recipes = new List<Recipe>();
        }

        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }

        public List<Recipe> FilterRecipes(string ingredientFilter, string foodGroupFilter, double maxCaloriesFilter)
        {
            
            return Recipes;
        }
    }
}
