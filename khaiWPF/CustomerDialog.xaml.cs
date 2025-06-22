using HotelManagement.Business;
using HotelManagement.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace khaiWPF

{
    public partial class CustomerDialog : Window
    {
        private CustomerService customerService;
        private Customer customer;
        private bool isEditMode;

        public CustomerDialog(Customer customer = null)
        {
            InitializeComponent();
            customerService = new CustomerService();
            this.customer = customer;
            isEditMode = customer != null;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            if (isEditMode)
            {
                lblTitle.Text = "Edit Customer";
                LoadCustomerData();
            }
            else
            {
                lblTitle.Text = "Add New Customer";
                customer = new Customer();
            }
        }

        private void LoadCustomerData()
        {
            txtFullName.Text = customer.CustomerFullName;
            txtTelephone.Text = customer.Telephone;
            txtEmail.Text = customer.EmailAddress;
            dpBirthday.SelectedDate = customer.CustomerBirthday;
            txtPassword.Password = customer.Password;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    customer.CustomerFullName = txtFullName.Text.Trim();
                    customer.Telephone = txtTelephone.Text.Trim();
                    customer.EmailAddress = txtEmail.Text.Trim();
                    customer.CustomerBirthday = dpBirthday.SelectedDate.Value;
                    customer.Password = txtPassword.Password.Trim();

                    if (isEditMode)
                    {
                        customerService.UpdateCustomer(customer);
                        MessageBox.Show("Customer updated successfully.", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        customerService.AddCustomer(customer);
                        MessageBox.Show("Customer added successfully.", "Success",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving customer: {ex.Message}", "Error",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            // Full Name validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full Name is required.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return false;
            }

            if (txtFullName.Text.Trim().Length > 50)
            {
                MessageBox.Show("Full Name cannot exceed 50 characters.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return false;
            }

            // Telephone validation
            if (string.IsNullOrWhiteSpace(txtTelephone.Text))
            {
                MessageBox.Show("Telephone is required.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTelephone.Focus();
                return false;
            }

            if (!Regex.IsMatch(txtTelephone.Text.Trim(), @"^\d{10,12}$"))
            {
                MessageBox.Show("Telephone must be 10-12 digits.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTelephone.Focus();
                return false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email Address is required.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            // Check email uniqueness
            var existingCustomers = customerService.GetAllCustomers();
            if (!isEditMode || (isEditMode && txtEmail.Text.Trim() != customer.EmailAddress))
            {
                if (existingCustomers.Any(c => c.EmailAddress.Equals(txtEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Email address already exists.", "Validation Error",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // Birthday validation
            if (!dpBirthday.SelectedDate.HasValue)
            {
                MessageBox.Show("Birthday is required.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpBirthday.Focus();
                return false;
            }

            if (dpBirthday.SelectedDate.Value > DateTime.Now.AddYears(-18))
            {
                MessageBox.Show("Customer must be at least 18 years old.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpBirthday.Focus();
                return false;
            }

            // Password validation
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Password is required.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Password.Trim().Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}