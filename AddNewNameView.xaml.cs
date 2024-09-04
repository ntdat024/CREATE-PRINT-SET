using System;
using System.Linq;
using System.Windows;

namespace NDT_RevitAPI
{
    /// <summary>
    /// Interaction logic for AddNewNameView.xaml
    /// </summary>
    public partial class AddNewNameView : Window
    {
        private string SPECIAL_CHARS = @"\:{}[];<>?`~";
        public string NewName { get; set; }
        public AddNewNameView()
        {
            InitializeComponent();
            tb_SetName.Focus();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string input = tb_SetName.Text;
            if (CheckInput(input))
            {
                NewName = input;
                DialogResult = true;
            }
            else tb_SetName.Focus();
        }

        private bool CheckInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show($"Name cannot empty!", "Message");
                return false;
            }

            var array = SPECIAL_CHARS.ToCharArray();
            foreach (char c in array)
            {
                if (input.ToCharArray().Contains(c))
                {
                    MessageBox.Show($"Name cannot contain any of special characters:\n{SPECIAL_CHARS}", "Message");
                    return false;
                }
            }

            return true;
        }
    }
}
