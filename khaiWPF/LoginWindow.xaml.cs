using HotelManagement.Business;
using khaiWPF;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Windows;

namespace khaiWPF

{
    public partial class LoginWindow : Window
    {
        private CustomerService customerService;
        private IConfiguration configuration;

        public LoginWindow()
        {
            InitializeComponent();
            customerService = new CustomerService();
            //LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Please enter both email and password.";
                return;
            }

            // 👉 Hardcode admin login
            if (email == "admin@fpt.edu.vn" && password == "admin123")
            {
                MainWindow mainWindow = new MainWindow(isAdmin: true);
                mainWindow.Show();
                this.Close();
                return;
            }

            // Check customer account
            var customer = customerService.Authenticate(email, password);
            if (customer != null)
            {
                MainWindow mainWindow = new MainWindow(isAdmin: false, customer);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                lblMessage.Text = "Invalid email or password.";
            }
        }

    }
}