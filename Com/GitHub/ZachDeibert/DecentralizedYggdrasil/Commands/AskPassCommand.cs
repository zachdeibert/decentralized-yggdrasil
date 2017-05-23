using System;
using System.Windows.Forms;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Ui;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class AskPassCommand : ICommand {
		public string Name {
			get {
				return "askpass";
			}
		}

		public void Run(string[] args) {
            PasswordDialog dialog = new PasswordDialog();
			if (dialog.ShowDialog() == DialogResult.OK) {
				Console.WriteLine(dialog.Password);
			}
		}
	}
}

