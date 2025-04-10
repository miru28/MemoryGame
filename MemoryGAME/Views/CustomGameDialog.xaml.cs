using System.Windows;
using System.Windows.Controls;

namespace MemoryGAME.Views
{
    public partial class CustomGameDialog : Window
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public CustomGameDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Rows = int.Parse(((ComboBoxItem)RowsComboBox.SelectedItem).Content.ToString());
            Columns = int.Parse(((ComboBoxItem)ColumnsComboBox.SelectedItem).Content.ToString());

            // Ensure the product is even
            if ((Rows * Columns) % 2 != 0)
            {
                MessageBox.Show("The total number of cards must be even.",
                    "Invalid Dimensions", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}