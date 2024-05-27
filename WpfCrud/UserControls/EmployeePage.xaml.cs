using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfCrud.Models;
using WpfCrud.Services;

namespace WpfCrud.UserControls
{
    public partial class EmployeePage : UserControl
    {
        private Employee selectedEmployee = new Employee();
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        public EmployeePage()
        {
            InitializeComponent();
            GetEmployees();
        }

        // Gets all the current employees
        private void GetEmployees()
        {
            employees = DbServices.employeesDbService.Select();
            EmployeesGridView.ItemsSource = employees;
        }

        // When an employee is clicked on in the grid view, this method is called and assigns the data to both the selectedEmployee object and the UI fields
        private void EmployeesGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesGridView.SelectedItem is Employee selected)
            {
                selectedEmployee = selected;
                FirstNameTextBox.Text = selectedEmployee.FirstName;
                LastNameTextBox.Text = selectedEmployee.LastName;
                TitleTextBox.Text = selectedEmployee.Title;
                SalaryTextBox.Text = selectedEmployee.Salary.ToString();
            }
        }

        // Gets all the current employees
        private void ReadEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            GetEmployees();
        }

        // This method updates the selectedEmployee object with user entered data
        private void UpdateSelectedEmployee()
        {
            selectedEmployee.FirstName = FirstNameTextBox.Text;
            selectedEmployee.LastName = LastNameTextBox.Text;
            selectedEmployee.Title = TitleTextBox.Text;
            if (int.TryParse(SalaryTextBox.Text, out int salary))
            {
                selectedEmployee.Salary = salary;
            }
        }

        // This method attempts to create an employee
        private void CreateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedEmployee();

            if (DbServices.employeesDbService.Insert(selectedEmployee))
            {
                MessageBox.Show("Employee Created!");
            }
            else
            {
                MessageBox.Show("An error occurred while creating your employee.");
            }
        }

        // This method attempts to update an employee
        private void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedEmployee();

            if (DbServices.employeesDbService.Update(selectedEmployee))
            {
                MessageBox.Show("Employee Updated!");
            }
            else
            {
                MessageBox.Show("An error occurred while updating your employee.");
            }
        }

        // This method attempts to delete an employee
        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedEmployee();

            if (DbServices.employeesDbService.Delete(selectedEmployee))
            {
                MessageBox.Show("Employee Deleted!");
            }
            else
            {
                MessageBox.Show("An error occurred while deleting your employee.");
            }
        }
    }
}
