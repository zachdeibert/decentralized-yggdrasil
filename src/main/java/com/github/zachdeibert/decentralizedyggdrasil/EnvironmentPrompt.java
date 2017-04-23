package com.github.zachdeibert.decentralizedyggdrasil;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.io.File;
import java.io.IOException;

import javax.swing.JButton;
import javax.swing.JFileChooser;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JPopupMenu;
import javax.swing.SpringLayout;
import javax.swing.border.EmptyBorder;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.filechooser.FileNameExtensionFilter;

public class EnvironmentPrompt extends JFrame {
	private static final long serialVersionUID = 5433394048671480707L;

	public EnvironmentPrompt(File keystore) {
		setTitle("Decentralized Yggdrasil Launcher");
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 450, 300);
		JPanel contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		SpringLayout sl_contentPane = new SpringLayout();
		JLabel lblSelectLauncherTo = new JLabel("Select Launcher to Launch:");
		sl_contentPane.putConstraint(SpringLayout.NORTH, lblSelectLauncherTo, 10, SpringLayout.NORTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.WEST, lblSelectLauncherTo, 10, SpringLayout.WEST, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, lblSelectLauncherTo, -227, SpringLayout.EAST, contentPane);
		contentPane.setLayout(sl_contentPane);
		contentPane.add(lblSelectLauncherTo);
		JList<Launcher> list = new JList<Launcher>();
		LauncherListModel launchers = new LauncherListModel();
		list.setModel(launchers);
		sl_contentPane.putConstraint(SpringLayout.NORTH, list, 10, SpringLayout.SOUTH, lblSelectLauncherTo);
		sl_contentPane.putConstraint(SpringLayout.WEST, list, 10, SpringLayout.WEST, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, list, -10, SpringLayout.EAST, contentPane);
		contentPane.add(list);
		JButton btnLaunch = new JButton("Launch");
		btnLaunch.setEnabled(false);
		btnLaunch.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				try {
					dispose();
					list.getSelectedValue().launch(keystore);
				} catch (IOException ex) {
					ex.printStackTrace();
					JOptionPane.showMessageDialog(EnvironmentPrompt.this, "Unable to launch launcher", "Launcher Error",
							JOptionPane.ERROR_MESSAGE);
				}
			}
		});
		sl_contentPane.putConstraint(SpringLayout.SOUTH, list, -10, SpringLayout.NORTH, btnLaunch);
		sl_contentPane.putConstraint(SpringLayout.SOUTH, btnLaunch, -10, SpringLayout.SOUTH, contentPane);
		sl_contentPane.putConstraint(SpringLayout.EAST, btnLaunch, -10, SpringLayout.EAST, contentPane);
		contentPane.add(btnLaunch);
		JButton btnAddLauncher = new JButton("Add");
		sl_contentPane.putConstraint(SpringLayout.WEST, btnAddLauncher, 10, SpringLayout.WEST, contentPane);
		sl_contentPane.putConstraint(SpringLayout.SOUTH, btnAddLauncher, 0, SpringLayout.SOUTH, btnLaunch);
		contentPane.add(btnAddLauncher);
		JButton btnDelete = new JButton("Remove");
		btnDelete.setEnabled(false);
		btnDelete.addActionListener(new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				launchers.remove(list.getSelectedValue());
			}
		});
		sl_contentPane.putConstraint(SpringLayout.WEST, btnDelete, 6, SpringLayout.EAST, btnAddLauncher);
		list.addListSelectionListener(new ListSelectionListener() {
			@Override
			public void valueChanged(ListSelectionEvent e) {
				btnLaunch.setEnabled(!list.isSelectionEmpty());
				btnDelete.setEnabled(!list.isSelectionEmpty());
			}
		});
		JPopupMenu popupMenu = new JPopupMenu();
		ActionListener selectJar = new ActionListener() {
			@Override
			public void actionPerformed(ActionEvent e) {
				JFileChooser chooser = new JFileChooser();
				chooser.setFileFilter(new FileNameExtensionFilter("Jar Files", "jar"));
				chooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
				if (chooser.showOpenDialog(EnvironmentPrompt.this) == JFileChooser.APPROVE_OPTION) {
					launchers.install(new Launcher(chooser.getSelectedFile().getName(), chooser.getSelectedFile()));
				}
			}
		};
		MouseListener popup = new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				if (e.getButton() == MouseEvent.BUTTON1) {
					popupMenu.show(e.getComponent(), e.getX(), e.getY());
				}
			}
		};
		if (!launchers.isInstalled(Launcher.VANILLA)) {
			JMenuItem mntmVanilla = new JMenuItem("Vanilla");
			popupMenu.add(mntmVanilla);
			mntmVanilla.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					if (launchers.install(Launcher.VANILLA)) {
						popupMenu.remove(mntmVanilla);
						if (popupMenu.getComponentCount() == 1) {
							btnAddLauncher.addActionListener(selectJar);
							btnAddLauncher.removeMouseListener(popup);
						}
					} else {
						JOptionPane.showMessageDialog(EnvironmentPrompt.this, "Unable to install launcher",
								"Installation Error", JOptionPane.ERROR_MESSAGE);
					}
				}
			});
		}
		if (!launchers.isInstalled(Launcher.FTB)) {
			JMenuItem mntmFtb = new JMenuItem("FTB");
			popupMenu.add(mntmFtb);
			mntmFtb.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					if (launchers.install(Launcher.FTB)) {
						popupMenu.remove(mntmFtb);
						if (popupMenu.getComponentCount() == 1) {
							btnAddLauncher.addActionListener(selectJar);
							btnAddLauncher.removeMouseListener(popup);
						}
					} else {
						JOptionPane.showMessageDialog(EnvironmentPrompt.this, "Unable to install launcher",
								"Installation Error", JOptionPane.ERROR_MESSAGE);
					}
				}
			});
		}
		if (!launchers.isInstalled(Launcher.TECHNIC)) {
			JMenuItem mntmTechnic = new JMenuItem("Technic");
			popupMenu.add(mntmTechnic);
			mntmTechnic.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					if (launchers.install(Launcher.TECHNIC)) {
						popupMenu.remove(mntmTechnic);
						if (popupMenu.getComponentCount() == 1) {
							btnAddLauncher.addActionListener(selectJar);
							btnAddLauncher.removeMouseListener(popup);
						}
					} else {
						JOptionPane.showMessageDialog(EnvironmentPrompt.this, "Unable to install launcher",
								"Installation Error", JOptionPane.ERROR_MESSAGE);
					}
				}
			});
		}
		if (popupMenu.getComponentCount() > 0) {
			JMenuItem mntmSelectJar = new JMenuItem("Select Jar...");
			popupMenu.add(mntmSelectJar);
			btnAddLauncher.addMouseListener(popup);
		} else {
			btnAddLauncher.addActionListener(selectJar);
		}
		sl_contentPane.putConstraint(SpringLayout.SOUTH, btnDelete, 0, SpringLayout.SOUTH, btnLaunch);
		contentPane.add(btnDelete);
	}
}
