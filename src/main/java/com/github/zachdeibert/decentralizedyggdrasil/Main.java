package com.github.zachdeibert.decentralizedyggdrasil;

import java.awt.EventQueue;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.security.KeyStore;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;

public class Main {
	public static void main(String[] args) throws Exception {
		File keystore = new File("keystore.jks");
		KeyStore ks = KeyStore.getInstance("JKS");
		try (FileInputStream stream = new FileInputStream(JavaEnvironment.findDefaultKeystore())) {
			ks.load(stream, JavaEnvironment.KEYSTORE_PASS);
		}
		try (FileInputStream stream = new FileInputStream(args[0])) {
			CertificateFactory cf = CertificateFactory.getInstance("X.509");
			Certificate cert = cf.generateCertificate(stream);
			ks.setCertificateEntry("authserver.mojang.com", cert);
		}
		try (FileOutputStream stream = new FileOutputStream(keystore)) {
			ks.store(stream, JavaEnvironment.KEYSTORE_PASS);
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
