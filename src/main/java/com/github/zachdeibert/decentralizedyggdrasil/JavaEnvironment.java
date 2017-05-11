package com.github.zachdeibert.decentralizedyggdrasil;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.PrintWriter;

public class JavaEnvironment {
	public static final char[] KEYSTORE_PASS = "changeit".toCharArray();
	public static final File FAKE_BIN = new File("bin");

	public static File findDefaultKeystore() throws IOException {
		File home = new File(System.getProperty("java.home"));
		if (!home.exists()) {
			throw new FileNotFoundException();
		}
		File test = new File(new File(new File(home, "lib"), "security"), "cacerts");
		if (test.exists()) {
			return test;
		} else {
			test = new File(new File(new File(new File(home, "jre"), "lib"), "security"), "cacerts");
			if (test.exists()) {
				return test;
			} else {
				throw new FileNotFoundException();
			}
		}
	}

	private static String which(String cmd) throws IOException {
		for (String dir : cmd.split(File.pathSeparator)) {
			File file = new File(dir, cmd);
			if (file.exists()) {
				return file.getAbsolutePath();
			}
		}
		throw new FileNotFoundException(cmd);
	}

	public static void createJavaWrapper(File keystore) {
		FAKE_BIN.mkdirs();
		File shell = new File(FAKE_BIN, "java");
		try (OutputStream stream = new FileOutputStream(shell); PrintWriter ps = new PrintWriter(stream)) {
			ps.println("#!/bin/sh");
			ps.println();
			ps.printf("%s -Djavax.net.ssl.trustStore=%s $*\n", which("java"), keystore.getAbsolutePath());
		} catch (IOException ex) {
			ex.printStackTrace();
		}
		shell.setExecutable(true);
		File batch = new File(FAKE_BIN, "java");
		try (OutputStream stream = new FileOutputStream(batch); PrintWriter ps = new PrintWriter(stream)) {
			ps.println("@echo off");
			ps.println();
			ps.printf("%s -Djavax.net.ssl.trustStore=%s %%*\n", which("java.exe"), keystore.getAbsolutePath());
		} catch (IOException ex) {
			ex.printStackTrace();
		}
	}
}
