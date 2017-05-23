using System;
using System.Drawing;
using System.Windows.Forms;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Ui {
    public partial class PasswordDialog : Form {
        public string Password {
            get {
                return textBox1.Text;
            }

            set {
                textBox1.Text = value;
            }
        }

        public PasswordDialog() {
            InitializeComponent();
            PasswordDialog_ClientSizeChanged(this, null);
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            Close();
        }

        private void PasswordDialog_ClientSizeChanged(object sender, EventArgs e) {
            label1.MaximumSize = new Size((sender as Control).ClientSize.Width - label1.Left, 10000);
        }
    }
}
