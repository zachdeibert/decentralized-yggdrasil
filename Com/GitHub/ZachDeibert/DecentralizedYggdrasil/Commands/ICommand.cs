using System;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public interface ICommand {
		string Name {
			get;
		}

		void Run(string[] args);
	}
}

