package com.github.zachdeibert.decentralizedyggdrasil;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.prefs.Preferences;

import javax.swing.AbstractListModel;

public class LauncherListModel extends AbstractListModel<Launcher> {
	private static final long serialVersionUID = 3708835313787267906L;
	private final List<Launcher> launchers;
	private final Preferences prefs;

	public boolean isInstalled(Launcher launcher) {
		return launchers.contains(launcher) && launcher.isInstalled();
	}

	private void save() {
		try (ByteArrayOutputStream buffer = new ByteArrayOutputStream()) {
			try (ObjectOutputStream stream = new ObjectOutputStream(buffer)) {
				stream.writeObject(launchers);
			}
			prefs.putByteArray("ser", buffer.toByteArray());
		} catch (Exception ex) {
			ex.printStackTrace();
		}
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
		save();
		return true;
	}

	public void remove(Launcher launcher) {
		int index = launchers.indexOf(launcher);
		if (index >= 0) {
			launchers.remove(index);
			fireIntervalRemoved(this, index, index);
			save();
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
		prefs = Preferences.userNodeForPackage(getClass());
		byte[] data = prefs.getByteArray("ser", new byte[0]);
		if (data.length > 0) {
			try (ByteArrayInputStream buffer = new ByteArrayInputStream(data);
					ObjectInputStream stream = new ObjectInputStream(buffer)) {
				@SuppressWarnings("unchecked")
				List<Launcher> list = (List<Launcher>) stream.readObject();
				launchers.addAll(list);
			} catch (Exception ex) {
				ex.printStackTrace();
			}
		}
	}
}
