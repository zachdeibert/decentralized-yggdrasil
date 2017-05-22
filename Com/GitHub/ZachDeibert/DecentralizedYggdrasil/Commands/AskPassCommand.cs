using System;
using System.Drawing;
using System.Windows.Forms;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class AskPassCommand : ICommand {
		public string Name {
			get {
				return "askpass";
			}
		}

		public void Run(string[] args) {
			Form prompt = new Form() {
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = "We need to make changes to your computer",
				StartPosition = FormStartPosition.CenterScreen
			};
			Label label = new Label() {
				Left = 50,
				Top = 20,
				Text = "Please enter the password for your account:",
				Parent = prompt
			};
			TextBox input = new TextBox() {
				Left = 50,
				Top = 50,
				Width = 400,
				PasswordChar = '*',
				Parent = prompt
			};
			Button ok = new Button() {
				Text = "OK",
				Left = 350,
				Width = 100,
				Top = 70,
				DialogResult = DialogResult.OK,
				Parent = prompt
			};
			prompt.AcceptButton = ok;
			ok.Click += (sender, e) => prompt.Close();
			if (prompt.ShowDialog() == DialogResult.OK) {
				Console.WriteLine(input.Text);
			}
		}
	}
}

