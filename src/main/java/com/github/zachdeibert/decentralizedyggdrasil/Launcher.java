package com.github.zachdeibert.decentralizedyggdrasil;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.Serializable;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Arrays;

public class Launcher implements Serializable {
	public static final Launcher VANILLA = new Launcher("Minecraft",
			"http://s3.amazonaws.com/Minecraft.Download/launcher/Minecraft.jar");
	public static final Launcher FTB = new Launcher("FTB", "http://ftb.cursecdn.com/FTB2/launcher/FTB_Launcher.jar");
	public static final Launcher TECHNIC = new Launcher("Technic",
			"http://launcher.technicpack.net/launcher4/349/TechnicLauncher.jar", "-blockReboot");

	private static final long serialVersionUID = 1666394930637848062L;
	public final String name;
	public final File path;
	private final URL url;
	private final String[] args;

	public boolean isInstalled() {
		return path.exists();
	}

	public void install() throws IOException {
		try (InputStream in = url.openStream(); OutputStream out = new FileOutputStream(path)) {
			byte[] buffer = new byte[4096];
			for (int len = in.read(buffer); len > 0; len = in.read(buffer)) {
				out.write(buffer, 0, len);
			}
		}
	}

	public void launch(File keystore) throws IOException {
		String[] cmd = new String[4 + args.length];
		System.arraycopy(new String[] { "java", "-Djavax.net.ssl.trustStore=".concat(keystore.getAbsolutePath()),
				"-jar", path.getAbsolutePath() }, 0, cmd, 0, 4);
		System.arraycopy(args, 0, cmd, 4, args.length);
		Process proc = Runtime.getRuntime().exec(cmd);
		Thread stdout = new Thread() {
			@Override
			public void run() {
				try (InputStream in = proc.getInputStream()) {
					byte[] buffer = new byte[4096];
					for (int len = in.read(buffer); len > 0; len = in.read(buffer)) {
						System.out.write(buffer, 0, len);
					}

				} catch (IOException ex) {
					ex.printStackTrace();
				}
			}
		};
		stdout.start();
		Thread stderr = new Thread() {
			@Override
			public void run() {
				try (InputStream in = proc.getErrorStream()) {
					byte[] buffer = new byte[4096];
					for (int len = in.read(buffer); len > 0; len = in.read(buffer)) {
						System.err.write(buffer, 0, len);
					}
				} catch (IOException ex) {
					ex.printStackTrace();
				}
			}
		};
		stderr.start();
		try {
			proc.waitFor();
			stdout.join();
			stderr.join();
		} catch (InterruptedException ex) {
			throw new IOException(ex);
		}
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + Arrays.hashCode(args);
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		result = prime * result + ((path == null) ? 0 : path.hashCode());
		result = prime * result + ((url == null) ? 0 : url.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) {
			return true;
		}
		if (obj == null) {
			return false;
		}
		if (!(obj instanceof Launcher)) {
			return false;
		}
		Launcher other = (Launcher) obj;
		if (!Arrays.equals(args, other.args)) {
			return false;
		}
		if (name == null) {
			if (other.name != null) {
				return false;
			}
		} else if (!name.equals(other.name)) {
			return false;
		}
		if (path == null) {
			if (other.path != null) {
				return false;
			}
		} else if (!path.equals(other.path)) {
			return false;
		}
		if (url == null) {
			if (other.url != null) {
				return false;
			}
		} else if (!url.equals(other.url)) {
			return false;
		}
		return true;
	}

	@Override
	public String toString() {
		return name;
	}

	public Launcher() {
		name = null;
		path = null;
		url = null;
		args = new String[0];
	}

	public Launcher(String name, File path) {
		this.name = name;
		this.path = path;
		url = null;
		args = new String[0];
	}

	private Launcher(String name, String url, String... args) {
		this.name = name;
		path = new File(name.concat(".jar"));
		try {
			this.url = new URL(url);
		} catch (MalformedURLException ex) {
			throw new RuntimeException(ex);
		}
		this.args = args;
	}
}
