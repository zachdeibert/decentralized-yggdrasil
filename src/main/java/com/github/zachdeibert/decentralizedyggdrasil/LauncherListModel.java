package com.github.zachdeibert.decentralizedyggdrasil;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.swing.AbstractListModel;

public class LauncherListModel extends AbstractListModel<Launcher> {
	private static final long serialVersionUID = 3708835313787267906L;
	private final List<Launcher> launchers;

	public boolean isInstalled(Launcher launcher) {
		return launchers.contains(launcher) && launcher.isInstalled();
	}

	public boolean install(Launcher launcher) {
		if (!launcher.isInstalled()) {
			try {
				launcher.install();
			} catch (IOException ex) {
				ex.printStackTrace();
				return false;
			}
		}
		launchers.add(launcher);
		fireIntervalAdded(this, launchers.indexOf(launcher), launchers.indexOf(launcher));
		return true;
	}

	public void remove(Launcher launcher) {
		int index = launchers.indexOf(launcher);
		if (index >= 0) {
			launchers.remove(index);
			fireIntervalRemoved(this, index, index);
		}
	}

	@Override
	public Launcher getElementAt(int index) {
		return launchers.get(index);
	}

	@Override
	public int getSize() {
		return launchers.size();
	}

	public LauncherListModel() {
		launchers = new ArrayList<Launcher>();
	}
}
