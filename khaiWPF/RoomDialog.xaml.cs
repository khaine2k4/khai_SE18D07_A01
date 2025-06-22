using HotelManagement.Business;
using HotelManagement.Models;
using System;
using System.Linq;
using System.Windows;

namespace khaiWPF

{
    public partial class RoomDialog : Window
    {
        private RoomService roomService;
        private RoomInformation room;
        private bool isEditMode;

        public RoomDialog(RoomInformation room = null)
        {
            InitializeComponent();
            roomService = new RoomService();
            this.room = room;
            isEditMode = room != null;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            LoadRoomTypes();

            if (isEditMode)
            {
                lblTitle.Text = "Chỉnh sửa Phòng";
                LoadRoomData();
            }
            else
            {
                lblTitle.Text = "Thêm Phòng Mới";
                room = new RoomInformation();
            }
        }

        private void LoadRoomTypes()
        {
            var roomTypes = roomService.GetAllRoomTypes();
            cmbRoomType.ItemsSource = roomTypes;
        }

        private void LoadRoomData()
        {
            txtRoomNumber.Text = room.RoomNumber;
            txtRoomDescription.Text = room.RoomDescription;
            txtMaxCapacity.Text = room.RoomMaxCapacity.ToString();
            txtPricePerDate.Text = room.RoomPricePerDate.ToString();
            cmbRoomType.SelectedValue = room.RoomTypeID;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    room.RoomNumber = txtRoomNumber.Text.Trim();
                    room.RoomDescription = txtRoomDescription.Text.Trim();
                    room.RoomMaxCapacity = int.Parse(txtMaxCapacity.Text.Trim());
                    room.RoomPricePerDate = decimal.Parse(txtPricePerDate.Text.Trim());
                    room.RoomTypeID = (int)cmbRoomType.SelectedValue;

                    if (isEditMode)
                    {
                        roomService.UpdateRoom(room);
                        MessageBox.Show("Phòng đã được cập nhật thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        roomService.AddRoom(room);
                        MessageBox.Show("Phòng đã được thêm thành công.", "Thành công",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu phòng: {ex.Message}", "Lỗi",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            // Xác thực số phòng
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                MessageBox.Show("Số phòng là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRoomNumber.Focus();
                return false;
            }

            if (txtRoomNumber.Text.Trim().Length > 50)
            {
                MessageBox.Show("Số phòng không được vượt quá 50 ký tự.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRoomNumber.Focus();
                return false;
            }

            // Kiểm tra tính duy nhất của số phòng
            var existingRooms = roomService.GetAllRooms();
            if (!isEditMode || (isEditMode && txtRoomNumber.Text.Trim() != room.RoomNumber))
            {
                if (existingRooms.Any(r => r.RoomNumber.Equals(txtRoomNumber.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Số phòng đã tồn tại.", "Lỗi xác thực",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtRoomNumber.Focus();
                    return false;
                }
            }

            // Xác thực mô tả phòng
            if (string.IsNullOrWhiteSpace(txtRoomDescription.Text))
            {
                MessageBox.Show("Mô tả phòng là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRoomDescription.Focus();
                return false;
            }

            if (txtRoomDescription.Text.Trim().Length > 220)
            {
                MessageBox.Show("Mô tả phòng không được vượt quá 220 ký tự.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtRoomDescription.Focus();
                return false;
            }

            // Xác thực sức chứa tối đa
            if (string.IsNullOrWhiteSpace(txtMaxCapacity.Text))
            {
                MessageBox.Show("Sức chứa tối đa là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMaxCapacity.Focus();
                return false;
            }

            if (!int.TryParse(txtMaxCapacity.Text.Trim(), out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Sức chứa tối đa phải là số dương.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMaxCapacity.Focus();
                return false;
            }

            // Xác thực giá mỗi ngày
            if (string.IsNullOrWhiteSpace(txtPricePerDate.Text))
            {
                MessageBox.Show("Giá mỗi ngày là bắt buộc.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPricePerDate.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPricePerDate.Text.Trim(), out decimal price) || price <= 0)
            {
                MessageBox.Show("Giá mỗi ngày phải là số dương.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPricePerDate.Focus();
                return false;
            }

            // Xác thực loại phòng
            if (cmbRoomType.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại phòng.", "Lỗi xác thực",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbRoomType.Focus();
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