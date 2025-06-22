using HotelManagement.Business;
using HotelManagement.Models;
using khaiWPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace khaiWPF

{
    public partial class MainWindow : Window
    {
        private bool isAdmin;
        private Customer currentCustomer;
        private CustomerService customerService;
        private RoomService roomService;
        private BookingService bookingService;

        public MainWindow(bool isAdmin, Customer customer = null)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            this.currentCustomer = customer;

            customerService = new CustomerService();
            roomService = new RoomService();
            bookingService = new BookingService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            if (isAdmin)
            {
                lblWelcome.Text = "Chào mừng, Quản trị viên";
                tabCustomers.Visibility = Visibility.Visible;
                tabRooms.Visibility = Visibility.Visible;
                tabBookings.Visibility = Visibility.Visible;
                tabReports.Visibility = Visibility.Visible;
                tabBookingHistory.Visibility = Visibility.Collapsed;
                tabProfile.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblWelcome.Text = $"Chào mừng, {currentCustomer.CustomerFullName}";
                tabCustomers.Visibility = Visibility.Collapsed;
                tabRooms.Visibility = Visibility.Collapsed;
                tabBookings.Visibility = Visibility.Collapsed;
                tabReports.Visibility = Visibility.Collapsed;
                tabBookingHistory.Visibility = Visibility.Visible;
                tabProfile.Visibility = Visibility.Visible;
                LoadCustomerProfile();
                LoadBookingHistory();
            }
        }

        private void LoadData()
        {
            if (isAdmin)
            {
                LoadCustomers();
                LoadRooms();
                LoadBookings();
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = customerService.GetAllCustomers();
                dgCustomers.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách hàng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRooms()
        {
            try
            {
                var rooms = roomService.GetAllRooms();
                dgRooms.ItemsSource = rooms;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phòng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBookings()
        {
            try
            {
                var bookings = bookingService.GetAllBookings();
                dgBookings.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đặt phòng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBookingHistory()
        {
            try
            {
                var bookings = bookingService.GetBookingsByCustomer(currentCustomer.CustomerID);
                dgBookingHistory.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử đặt phòng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCustomerProfile()
        {
            if (currentCustomer != null)
            {
                txtProfileFullName.Text = currentCustomer.CustomerFullName;
                txtProfileTelephone.Text = currentCustomer.Telephone;
                txtProfileEmail.Text = currentCustomer.EmailAddress;
                dpProfileBirthday.SelectedDate = currentCustomer.CustomerBirthday;
                txtProfilePassword.Password = currentCustomer.Password;
            }
        }

        // Customer Management Events
        private void btnSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearchCustomer.Text.Trim();
                var customers = customerService.SearchCustomers(searchTerm);
                dgCustomers.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefreshCustomer_Click(object sender, RoutedEventArgs e)
        {
            txtSearchCustomer.Text = "";
            LoadCustomers();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerDialog dialog = new CustomerDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadCustomers();
            }
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selectedCustomer)
            {
                CustomerDialog dialog = new CustomerDialog(selectedCustomer);
                if (dialog.ShowDialog() == true)
                {
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để chỉnh sửa.", "Thông tin",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selectedCustomer)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa khách hàng '{selectedCustomer.CustomerFullName}'?",
                                           "Xác nhận Xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        customerService.DeleteCustomer(selectedCustomer.CustomerID);
                        LoadCustomers();
                        MessageBox.Show("Khách hàng đã được xóa thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông tin",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Room Management Events
        private void btnSearchRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearchRoom.Text.Trim();
                var rooms = roomService.SearchRooms(searchTerm);
                dgRooms.ItemsSource = rooms;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phòng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefreshRoom_Click(object sender, RoutedEventArgs e)
        {
            txtSearchRoom.Text = "";
            LoadRooms();
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            RoomDialog dialog = new RoomDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadRooms();
            }
        }

        private void btnEditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is RoomInformation selectedRoom)
            {
                RoomDialog dialog = new RoomDialog(selectedRoom);
                if (dialog.ShowDialog() == true)
                {
                    LoadRooms();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng để chỉnh sửa.", "Thông tin",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is RoomInformation selectedRoom)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa phòng '{selectedRoom.RoomNumber}'?",
                                           "Xác nhận Xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        roomService.DeleteRoom(selectedRoom.RoomID);
                        LoadRooms();
                        MessageBox.Show("Phòng đã được xóa thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa phòng: {ex.Message}", "Lỗi",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng để xóa.", "Thông tin",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Booking Management Events
        private void btnSearchBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearchBooking.Text.Trim();
                var bookings = bookingService.SearchBookings(searchTerm);
                dgBookings.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm đặt phòng: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefreshBooking_Click(object sender, RoutedEventArgs e)
        {
            txtSearchBooking.Text = "";
            LoadBookings();
        }

        private void btnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            BookingDialog dialog = new BookingDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadBookings();
            }
        }

        private void btnEditBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                BookingDialog dialog = new BookingDialog(selectedBooking);
                if (dialog.ShowDialog() == true)
                {
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đặt phòng để chỉnh sửa.", "Thông tin",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa đặt phòng ID '{selectedBooking.BookingID}'?",
                                           "Xác nhận Xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        bookingService.DeleteBooking(selectedBooking.BookingID);
                        LoadBookings();
                        MessageBox.Show("Đặt phòng đã được xóa thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa đặt phòng: {ex.Message}", "Lỗi",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đặt phòng để xóa.", "Thông tin",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Profile Management
        private void btnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateProfileInput())
                {
                    currentCustomer.CustomerFullName = txtProfileFullName.Text.Trim();
                    currentCustomer.Telephone = txtProfileTelephone.Text.Trim();
                    currentCustomer.CustomerBirthday = dpProfileBirthday.SelectedDate.Value;
                    currentCustomer.Password = txtProfilePassword.Password.Trim();

                    customerService.UpdateCustomer(currentCustomer);
                    MessageBox.Show("Hồ sơ đã được cập nhật thành công.", "Thành công",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật hồ sơ: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateProfileInput()
        {
            if (string.IsNullOrWhiteSpace(txtProfileFullName.Text))
            {
                MessageBox.Show("Họ và tên là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtProfileFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProfileTelephone.Text))
            {
                MessageBox.Show("Số điện thoại là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtProfileTelephone.Focus();
                return false;
            }

            if (!dpProfileBirthday.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày sinh là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpProfileBirthday.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProfilePassword.Password))
            {
                MessageBox.Show("Mật khẩu là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtProfilePassword.Focus();
                return false;
            }

            return true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận Đăng xuất",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn cả ngày bắt đầu và ngày kết thúc.", "Lỗi xác thực",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpEndDate.SelectedDate.Value < dpStartDate.SelectedDate.Value)
                {
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu.", "Lỗi xác thực",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var report = bookingService.GetBookingStatistics(dpStartDate.SelectedDate.Value, dpEndDate.SelectedDate.Value);
                txtTotalBookings.Text = $"Tổng đặt phòng: {report.TotalBookings}";
                txtTotalRevenue.Text = $"Tổng doanh thu: {report.TotalRevenue:C}";
                dgReportBookings.ItemsSource = report.Bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}