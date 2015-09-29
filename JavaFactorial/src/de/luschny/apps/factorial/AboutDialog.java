package de.luschny.apps.factorial;

import com.jgoodies.forms.layout.CellConstraints;
import com.jgoodies.forms.layout.FormLayout;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.io.IOException;
import java.net.URI;

public class AboutDialog extends JDialog {

    private static final long serialVersionUID = 1L;
    private static final String url = "http://www.luschny.de/math/factorial/FastFactorialFunctions.htm";

    public AboutDialog(Frame owner) {
        super(owner);
        initComponents();
    }

    public AboutDialog(Dialog owner) {
        super(owner);
        initComponents();
    }

    @SuppressWarnings( "deprecation" )
    private void initComponents() {
        JPanel dialogPane = new JPanel();
        JPanel contentPane = new JPanel();
        JLabel label9 = new JLabel();
        JSeparator separator1 = new JSeparator();
        JLabel label1 = new JLabel();
        JLabel label7 = new JLabel();
        JLabel label3 = new JLabel();
        JLabel label2 = new JLabel();
        JLabel label8 = new JLabel();
        JButton button1 = new JButton();
        CellConstraints cc = new CellConstraints();

        // ======== this ========
        setTitle("About Factorial Benchmark");
        Container contentPane2 = getContentPane();
        contentPane2.setLayout(new BorderLayout());

        // ======== dialogPane ========
        {
            dialogPane.setLayout(new BorderLayout());

            // ======== contentPane ========
            {
                contentPane.setLayout(new FormLayout("default, left:10dlu, default",
                        "3*(default), $pgap, fill:10dlu, 2*(default), $lgap, default, $lgap, pref, $lgap, default"));

                // ---- label9 ----
                label9.setText("Visit the homepage: Fast Factorial Functions");
                label9.setFont(new Font("Tahoma", Font.PLAIN, 12));
                label9.setHorizontalAlignment(SwingConstants.LEFT);
                label9.setForeground(Color.red);
                contentPane.add(label9, cc.xy(1, 2));
                contentPane.add(separator1, cc.xy(1, 3));

                // ---- label1 ----
                label1.setText("Version");
                label1.setFont(new Font("Tahoma", Font.PLAIN, 12));
                contentPane.add(label1, cc.xywh(1, 5, 1, 1, CellConstraints.RIGHT, CellConstraints.DEFAULT));

                // ---- label7 ----
                label7.setText("2010");
                label7.setFont(new Font("Tahoma", Font.PLAIN, 12));
                contentPane.add(label7, cc.xy(3, 5));
                contentPane.add(label3, cc.xywh(1, 6, 1, 1, CellConstraints.CENTER, CellConstraints.DEFAULT));

                // ---- label2 ----
                label2.setText("Author");
                label2.setFont(new Font("Tahoma", Font.PLAIN, 12));
                label2.setHorizontalAlignment(SwingConstants.LEFT);
                contentPane.add(label2, cc.xywh(1, 7, 1, 1, CellConstraints.RIGHT, CellConstraints.DEFAULT));

                // ---- label8 ----
                label8.setText("Peter Luschny");
                label8.setFont(new Font("Tahoma", Font.PLAIN, 12));
                contentPane.add(label8, cc.xy(3, 7));

                // ---- button1 ----
                button1.setText("OK");
                button1.addActionListener((ActionEvent e) -> {
                    openURI(url);
                    okButtonActionPerformed(e);
                });
                contentPane.add(button1, cc.xy(3, 11));
            }
            dialogPane.add(contentPane, BorderLayout.CENTER);
        }
        contentPane2.add(dialogPane, BorderLayout.CENTER);
        pack();
        setLocationRelativeTo(getOwner());
    }

    private void okButtonActionPerformed(ActionEvent e) {
        this.setVisible(false);
    }

    private static void openURI(String uriText) {
        if (Desktop.isDesktopSupported()) {
            URI uri = URI.create(uriText);
            try {
                Desktop.getDesktop().browse(uri);
            } catch (IOException e) {
                // e.printStackTrace();
            }
        }
    }

}
