using HotelManagement.Business;
using HotelManagement.Models;
using System;
using System.Windows;

namespace khaiWPF


{
    public partial class BookingDialog : Window
    {
        private BookingService bookingService;
        private RoomService roomService;
        private CustomerService customerService;
        private Booking booking;
        private bool isEditMode;

        public BookingDialog(Booking booking = null)
        {
            InitializeComponent();
            bookingService = new BookingService();
            roomService = new RoomService();
            customerService = new CustomerService();
            this.booking = booking;
            isEditMode = booking != null;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            LoadCustomers();
            LoadRooms();

            if (isEditMode)
            {
                lblTitle.Text = "Chỉnh sửa Đặt phòng";
                LoadBookingData();
            }
            else
            {
                lblTitle.Text = "Thêm Đặt phòng Mới";
                booking = new Booking();
            }
        }

        private void LoadCustomers()
        {
            var customers = customerService.GetAllCustomers();
            cmbCustomer.ItemsSource = customers;
        }

        private void LoadRooms()
        {
            var rooms = roomService.GetAllRooms();
            cmbRoom.ItemsSource = rooms;
        }

        private void LoadBookingData()
        {
            cmbCustomer.SelectedValue = booking.CustomerID;
            cmbRoom.SelectedValue = booking.RoomID;
            dpCheckInDate.SelectedDate = booking.CheckInDate;
            dpCheckOutDate.SelectedDate = booking.CheckOutDate;
            txtTotalPrice.Text = booking.TotalPrice.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    booking.CustomerID = (int)cmbCustomer.SelectedValue;
                    booking.RoomID = (int)cmbRoom.SelectedValue;
                    booking.CheckInDate = dpCheckInDate.SelectedDate.Value;
                    booking.CheckOutDate = dpCheckOutDate.SelectedDate.Value;
                    booking.TotalPrice = CalculateTotalPrice();

                    if (isEditMode)
                    {
                        bookingService.UpdateBooking(booking);
                        MessageBox.Show("Đặt phòng đã được cập nhật thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        bookingService.AddBooking(booking);
                        MessageBox.Show("Đặt phòng đã được thêm thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu đặt phòng: {ex.Message}", "Lỗi",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private decimal CalculateTotalPrice()
        {
            var room = roomService.GetRoomById((int)cmbRoom.SelectedValue);
            var days = (dpCheckOutDate.SelectedDate.Value - dpCheckInDate.SelectedDate.Value).Days;
            return room.RoomPricePerDate * days;
        }

        private bool ValidateInput()
        {
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbCustomer.Focus();
                return false;
            }

            if (cmbRoom.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbRoom.Focus();
                return false;
            }

            if (!dpCheckInDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày nhận phòng là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckInDate.Focus();
                return false;
            }

            if (!dpCheckOutDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày trả phòng là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckOutDate.Focus();
                return false;
            }

            if (dpCheckInDate.SelectedDate.Value < DateTime.Today)
            {
                MessageBox.Show("Ngày nhận phòng không thể là ngày trong quá khứ.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckInDate.Focus();
                return false;
            }

            if (dpCheckOutDate.SelectedDate.Value <= dpCheckInDate.SelectedDate.Value)
            {
                MessageBox.Show("Ngày trả phòng phải sau ngày nhận phòng.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                dpCheckOutDate.Focus();
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