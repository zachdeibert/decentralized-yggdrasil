package com.github.zachdeibert.decentralizedyggdrasil;

import java.awt.EventQueue;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.KeyStore;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;

public class Main {
	private static final char[] KEYSTORE_PASS = "changeit".toCharArray();

	private static File findDefaultKeystore() throws IOException {
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

	public static void main(String[] args) throws Exception {
		File keystore = new File("keystore.jks");
		KeyStore ks = KeyStore.getInstance("JKS");
		try (FileInputStream stream = new FileInputStream(findDefaultKeystore())) {
			ks.load(stream, KEYSTORE_PASS);
		}
		try (FileInputStream stream = new FileInputStream(args[0])) {
			CertificateFactory cf = CertificateFactory.getInstance("X.509");
			Certificate cert = cf.generateCertificate(stream);
			ks.setCertificateEntry("authserver.mojang.com", cert);
		}
		try (FileOutputStream stream = new FileOutputStream(keystore)) {
			ks.store(stream, KEYSTORE_PASS);
		}
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					new EnvironmentPrompt(keystore).setVisible(true);
				} catch (Exception ex) {
					ex.printStackTrace();
				}
			}
		});
	}
}
