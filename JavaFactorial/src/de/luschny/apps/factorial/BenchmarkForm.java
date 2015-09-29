// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.apps.factorial;

import com.jgoodies.forms.layout.CellConstraints;
import com.jgoodies.forms.layout.FormLayout;
import de.luschny.apps.LoggedTextBox;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.io.IOException;

public class BenchmarkForm extends JFrame implements PropertyChangeListener {

    private static final long serialVersionUID = 1L;
    private final FactorialTest test;

    public BenchmarkForm() {
        test = new FactorialTest();

        initComponents();

        // set defaults
        checkBoxArray[Candidate.IndexOfReference].setSelected(true);
        logToFileCheckBox.setSelected(true);
        verboseCheckBox.setSelected(true);

        initHandler();
    }

    @SuppressWarnings("unchecked" )
    private void initComponents() {
        checkBoxArray = new JCheckBox[21];
        JMenuBar menuBar = new JMenuBar();
        JMenu benchMenu = new JMenu();
        recommededMenu = new JMenuItem();
        JMenuItem parallelMenu = new JMenuItem();
        primeMenu = new JMenuItem();
        simpleMenu = new JMenuItem();
        JMenuItem lameMenu = new JMenuItem();
        JMenu ectMenu = new JMenu();
        sanityMenu = new JMenuItem();
        JMenuItem aboutMenu = new JMenuItem();
        JLabel algoLabel = new JLabel();
        logToFileCheckBox = new JCheckBox();
        showValueCheckBox = new JCheckBox();
        verboseCheckBox = new JCheckBox();
        JLabel startLabel = new JLabel();
        startField = new JTextField();
        JLabel stepLabel = new JLabel();
        stepBox = new JComboBox();
        cancelButton = new JButton();
        JLabel lenghtLabel = new JLabel();
        lenghtSpinner = new JSpinner();
        benchmarkButton = new JButton();
        JPanel algoSelection = new JPanel();
        JScrollPane scrollPane = new JScrollPane();
        textArea = new JTextArea();
        infoLabel = new JLabel();
        progressBar = new JProgressBar();
        CellConstraints cc = new CellConstraints();

        // ======== benchForm ========

        setTitle("Factorial Algorithm Benchmark");
        setFont(new Font("Verdana", Font.BOLD, 16));
        Container benchFormContentPane = getContentPane();
        benchFormContentPane.setLayout(new FormLayout(
                "6dlu, $lcgap, 4dlu, $lcgap, 82dlu, 10dlu, 6dlu, center:46dlu, $lcgap, 42dlu, $lcgap, 18dlu, $lcgap, center:58dlu, 6dlu, left:12dlu",
                "2dlu, $lgap, default, $lgap, top:176dlu, $lgap, default, $lgap, 16dlu, $lgap, 18dlu, $lgap, 16dlu, $lgap, 18dlu, $lgap, 4dlu"));
        ((FormLayout) benchFormContentPane.getLayout()).setRowGroups(new int[][]{
                    {9, 11, 13}});

        JCheckBox checkBox = new JCheckBox();

        Font font = benchMenu.getFont().deriveFont(benchMenu.getFont().getStyle() | Font.BOLD,
                benchMenu.getFont().getSize() - 1f);
        Font labelFont = algoLabel.getFont().deriveFont(algoLabel.getFont().getStyle() | Font.BOLD,
                algoLabel.getFont().getSize() - 1f);
        Font checkFont = checkBox.getFont().deriveFont(checkBox.getFont().getStyle() | Font.BOLD,
                checkBox.getFont().getSize() - 1f);
        Font buttonFont = cancelButton.getFont().deriveFont(cancelButton.getFont().getStyle() | Font.BOLD);

        // ======== benchMenu ========

        benchMenu.setText("Benchmark");
        benchMenu.setFont(font);

        // ---- recommendedMenu ----
        recommededMenu.setText("Recommended");
        recommededMenu.setFont(font);
        recommededMenu.addActionListener((ActionEvent e) -> toptenMenuActionPerformed(e));

        benchMenu.add(recommededMenu);

        // ---- primeMenu ----
        primeMenu.setText("Prime");
        primeMenu.setFont(font);
        primeMenu.addActionListener((ActionEvent e) -> primeMenuActionPerformed(e));

        benchMenu.add(primeMenu);

        // ---- parallelMenu ----
        parallelMenu.setText("Parallel");
        parallelMenu.setFont(font);
        parallelMenu.addActionListener((ActionEvent e) -> parallelMenuActionPerformed(e));

        benchMenu.add(parallelMenu);

        // ---- simpleMenu ----
        simpleMenu.setText("Simple");
        simpleMenu.setFont(font);
        simpleMenu.addActionListener((ActionEvent e) -> simpleMenuActionPerformed(e));

        benchMenu.add(simpleMenu);

        // ---- lameMenu ----
        lameMenu.setText("Lame");
        lameMenu.setFont(font);
        lameMenu.addActionListener((ActionEvent e) -> lameMenuActionPerformed(e));

        benchMenu.add(lameMenu);

        menuBar.add(benchMenu);

        // ======== ectMenu ========
        ectMenu.setText("Ecetera");
        ectMenu.setFont(font);

        // ---- sanityMenu ----
        sanityMenu.setText("Sanity Check");
        sanityMenu.setFont(font);
        sanityMenu.addActionListener((ActionEvent e) -> sanityMenuActionPerformed(e));

        ectMenu.add(sanityMenu);
        ectMenu.addSeparator();

        // ---- aboutMenu ----
        aboutMenu.setText("About");
        aboutMenu.setFont(font);
        aboutMenu.addActionListener((ActionEvent e) -> aboutMenuActionPerformed(e));

        ectMenu.add(aboutMenu);
        menuBar.add(ectMenu);

        setJMenuBar(menuBar);

        // ---- algoLabel ----
        algoLabel.setText("Algorithm");
        algoLabel.setFont(labelFont);
        benchFormContentPane.add(algoLabel, new CellConstraints(5, 3, 1, 1, CellConstraints.DEFAULT,
                CellConstraints.DEFAULT, new Insets(0, 12, 0, 0)));

        // ---- logToFileCheckBox ----
        logToFileCheckBox.setText("Log To File");
        logToFileCheckBox.setFont(checkFont);
        benchFormContentPane.add(logToFileCheckBox, new CellConstraints(8, 3, 3, 1, CellConstraints.LEFT,
                CellConstraints.DEFAULT, new Insets(0, 8, 0, 0)));

        // ---- showValueCheckBox ----
        showValueCheckBox.setText("Show Value");
        showValueCheckBox.setFont(checkFont);
        benchFormContentPane.add(showValueCheckBox, cc.xywh(10, 3, 3, 1, CellConstraints.RIGHT, CellConstraints.DEFAULT));

        // ---- verboseCheckBox ----
        verboseCheckBox.setText("Verbose");
        verboseCheckBox.setFont(checkFont);
        benchFormContentPane.add(verboseCheckBox, new CellConstraints(14, 3, 1, 1, CellConstraints.CENTER,
                CellConstraints.DEFAULT, new Insets(0, 4, 0, 0)));

        // ---- startLabel ----
        startLabel.setText("Start");
        startLabel.setFont(labelFont);
        benchFormContentPane.add(startLabel, cc.xywh(8, 9, 1, 1, CellConstraints.RIGHT, CellConstraints.DEFAULT));

        // ---- startField ----
        startField.setToolTipText("Enter start value of n");
        startField.setText("8000");
        benchFormContentPane.add(startField, cc.xy(10, 9));

        // ---- stepLabel ----
        stepLabel.setText("Stepfactor");
        stepLabel.setFont(labelFont);
        benchFormContentPane.add(stepLabel, cc.xywh(8, 11, 1, 1, CellConstraints.RIGHT, CellConstraints.DEFAULT));

        // ---- stepBox ----
        stepBox.setEditable(true);
        DefaultComboBoxModel dcbm = new DefaultComboBoxModel(new String[]{"1.5", "2.0", "2.5", "3.0"});
        stepBox.setModel(dcbm);
        stepBox.setSelectedIndex(1);
        benchFormContentPane.add(stepBox, cc.xy(10, 11));

        // ---- cancelButton ----
        cancelButton.setText("Cancel");
        cancelButton.setEnabled(false);
        cancelButton.setBackground(SystemColor.activeCaption);
        cancelButton.setForeground(SystemColor.window);
        cancelButton.setFont(buttonFont);
        cancelButton.addActionListener((ActionEvent e) -> cancelButtonActionPerformed(e));

        benchFormContentPane.add(cancelButton, cc.xywh(14, 11, 1, 1, CellConstraints.FILL, CellConstraints.DEFAULT));

        // ---- lenghtLabel ----
        lenghtLabel.setText("Length");
        lenghtLabel.setFont(labelFont);
        benchFormContentPane.add(lenghtLabel, cc.xywh(8, 13, 1, 1, CellConstraints.RIGHT, CellConstraints.DEFAULT));

        // ---- lenghtSpinner ----
        lenghtSpinner.setModel(new SpinnerListModel(new String[]{"1", "2", "3", "4", "5", "6", "7", "8", "9"}));
        benchFormContentPane.add(lenghtSpinner, cc.xy(10, 13));

        // ---- benchmarkButton ----
        benchmarkButton.setText("Benchmark!");
        benchmarkButton.setFont(buttonFont);
        benchmarkButton.addActionListener((ActionEvent e) -> benchmarkButtonActionPerformed(e));
        benchFormContentPane.add(benchmarkButton, cc.xywh(14, 13, 1, 1, CellConstraints.FILL, CellConstraints.DEFAULT));

        // ======== AlgoSelection ========

        algoSelection.setPreferredSize(new Dimension(47, 550));
        algoSelection.setBackground(SystemColor.window);
        algoSelection.setLayout(new FormLayout("left:default:grow", "21*(fill:default)"));

        for (int i = 0; i < checkBoxArray.length; i++) {
            checkBox = new JCheckBox();
            checkBox.setText(FactorialTest.getNames()[i]);
            checkBox.setBackground(SystemColor.window);
            checkBox.setMargin(new Insets(3, 4, 1, 0));
            algoSelection.add(checkBox, cc.xy(1, i + 1));
            checkBox.setFont(checkFont);
            checkBoxArray[i] = checkBox;
        }

        benchFormContentPane.add(algoSelection, cc.xywh(3, 5, 3, 9));

        // ---- textArea ----
        textArea.setLineWrap(true);
        textArea.setEditable(false);
        textArea.setDisabledTextColor(SystemColor.windowText);
        textArea.setBackground(SystemColor.window);
        textArea.setFont(new Font("Courier New", Font.PLAIN, 12));

        // ======== scrollPane ========
        scrollPane.setViewportView(textArea);
        benchFormContentPane.add(scrollPane, cc.xywh(7, 5, 9, 3));

        // ---- infoLabel ----
        infoLabel.setText("(c) 2008 Peter Luschny");
        infoLabel.setFont(labelFont);
        benchFormContentPane.add(infoLabel, cc.xywh(3, 15, 3, 1));
        benchFormContentPane.add(progressBar, cc.xywh(7, 15, 9, 1));
        pack();
        setLocationRelativeTo(getOwner());
    }
    private JCheckBox[] checkBoxArray;
    private JMenuItem recommededMenu;
    private JMenuItem primeMenu;
    private JMenuItem simpleMenu;
    private JMenuItem sanityMenu;
    private JCheckBox logToFileCheckBox;
    private JCheckBox showValueCheckBox;
    private JCheckBox verboseCheckBox;
    private JTextField startField;
    private JComboBox stepBox;
    private JButton cancelButton;
    private JSpinner lenghtSpinner;
    private JButton benchmarkButton;
    private JTextArea textArea;
    private JLabel infoLabel;
    private JProgressBar progressBar;
    private LoggedTextBox winsole;
    private BenchmarkWorker worker;

    private void initHandler() {

        test.selectedAlgo = new boolean[checkBoxArray.length];

        try {
            winsole = new LoggedTextBox(textArea, "FactorialBench");
            winsole.setLogToFile(true);
            BenchmarkApplication.printAppAndSysProps(winsole.getPrintStream());
        } catch (IOException ioe) {
            JOptionPane.showMessageDialog(this, ioe.getMessage() + "\n", "Logging to file not possible.",
                    JOptionPane.ERROR_MESSAGE);
            return;
        }

        worker = new BenchmarkWorker(winsole, this);

    }

    // Eigelesen werden muss die Liste der ausgewaehlten Algos
    // und die Werte von Stepfactor und BenchLength.
    // Keine Validierung notwendig: Startwert und Stepfactor sind valid.
    private boolean getParams() {
        String start = startField.getText();
        try {
            test.benchStart = Integer.parseInt(start);
        } catch (NumberFormatException nfe) {
            JOptionPane.showMessageDialog(this, "Start is not a valid integer.\n" + start, "Invalid Argument Error",
                    JOptionPane.ERROR_MESSAGE);
            return false;
        }
        if (test.benchStart < 0) {
            JOptionPane.showMessageDialog(this, "Start must be a positive integer.\n" + start,
                    "Invalid Argument Error", JOptionPane.ERROR_MESSAGE);
            return false;
        }
        if (test.benchStart > FactorialTest.benchMax) {
            JOptionPane.showMessageDialog(this, "Start must be <= 9000000 because n! is huge.\n",
                    "Invalid Argument Error", JOptionPane.ERROR_MESSAGE);
            return false;
        }

        String length = lenghtSpinner.getValue().toString();
        try {
            test.benchLength = Integer.parseInt(length);
        } catch (NumberFormatException nfe) {
            JOptionPane.showMessageDialog(this, "Length l is not a valid integer: " + length + "\n",
                    "Invalid Argument Error", JOptionPane.ERROR_MESSAGE);
            return false;
        }

        String step = stepBox.getSelectedItem().toString();
        switch (step) {
            case "1.5":
                test.stepFactor = 1.5;
                break;
            case "2.0":
                test.stepFactor = 2.0;
                break;
            case "2.5":
                test.stepFactor = 2.5;
                break;
            default:
                test.stepFactor = 3.0;
                break;
        }

        winsole.logToFile = logToFileCheckBox.isSelected();
        test.showFullValue = showValueCheckBox.isSelected();
        test.verbose = verboseCheckBox.isSelected();

        // Die Referenz im Benchmark ist immer gewaehlt (per Muffti)
        checkBoxArray[Candidate.IndexOfReference].setSelected(true);

        int c = 0, i = 0;
        for (JCheckBox algo : checkBoxArray) {
            boolean check = algo.isSelected();
            if (check) {
                c++;
            }
            test.selectedAlgo[i++] = check;
        }

        test.cardSelected = c;
        return true;
    }

    // Disable the input controls until the asynchronous benchmark is done.
    private void enableControls(boolean en) {
        lenghtSpinner.setEnabled(en);
        startField.setEnabled(en);
        stepBox.setEnabled(en);

        verboseCheckBox.setEnabled(en);
        logToFileCheckBox.setEnabled(en);
        showValueCheckBox.setEnabled(en);

        recommededMenu.setEnabled(en);
        primeMenu.setEnabled(en);
        simpleMenu.setEnabled(en);
        sanityMenu.setEnabled(en);

        for (JCheckBox algo : checkBoxArray) {
            algo.setEnabled(en);
        }
    }

    // Die AlgorithmCheckedListBox entsprechend einstellen
    private void simpleMenuActionPerformed(ActionEvent e) {
        boolean[] SimpleAlgos = FactorialTest.getSimpleAlgos();
        int i = 0;
        for (JCheckBox algo : checkBoxArray) {
            algo.setSelected(SimpleAlgos[i++]);
        }
    }

    // Die AlgorithmCheckedListBox entsprechend einstellen
    private void primeMenuActionPerformed(ActionEvent e) {
        boolean[] PrimeAlgos = FactorialTest.getPrimeAlgos();
        int i = 0;
        for (JCheckBox algo : checkBoxArray) {
            algo.setSelected(PrimeAlgos[i++]);
        }
    }

    // Die AlgorithmCheckedListBox entsprechend einstellen
    private void parallelMenuActionPerformed(ActionEvent e) {
        boolean[] ParallelAlgos = FactorialTest.getParallelAlgos();
        int i = 0;
        for (JCheckBox algo : checkBoxArray) {
            algo.setSelected(ParallelAlgos[i++]);
        }
    }

    // Die AlgorithmCheckedListBox entsprechend einstellen
    private void toptenMenuActionPerformed(ActionEvent e) {
        boolean[] TopTenAlgos = FactorialTest.getRecommendedAlgos();
        int i = 0;
        for (JCheckBox algo : checkBoxArray) {
            algo.setSelected(TopTenAlgos[i++]);
        }
    }

    private void lameMenuActionPerformed(ActionEvent e) {
        boolean[] LameAlgos = FactorialTest.getLameAlgos();
        int i = 0;
        for (JCheckBox algo : checkBoxArray) {
            algo.setSelected(LameAlgos[i++]);
        }
    }

    private void sanityMenuActionPerformed(ActionEvent e) {
        doTheBenchmark(e);
    }

    // Kleine Infobox anzeigen
    private void aboutMenuActionPerformed(ActionEvent e) {
        AboutDialog dlg = new AboutDialog(this);
        dlg.setVisible(true);
    }

    private void cancelButtonActionPerformed(ActionEvent e) {
        infoLabel.setText("Cancelation is in progress!");

        // Cancel the asynchronous operation.
        worker.cancelAsync();

        // Disable the Cancel button.
        cancelButton.setEnabled(false);
    }

    // Ignorieren?
    void exitMenuActionPerformed(ActionEvent e) {
    }

    void infoMenuActionPerformed(ActionEvent e) {
    }

    private void benchmarkButtonActionPerformed(ActionEvent e) {
        doTheBenchmark(e);
    }

    // /////////////////////////////////////////////////////
    // Zentraler Flaschenhals: Benchmark-Schleife geht los
    // Ueber BenchLength von StartWert by times Stepfactor
    // Ueber alle ausgewaelhten Algorithmen
    private void doTheBenchmark(ActionEvent e) {
        enableControls(false);

        test.sanityTest = e.getActionCommand().equalsIgnoreCase("Sanity Check");

        // Get the values from the this. If they are valid ...
        if (test.sanityTest || getParams()) {
            // Reset the text in the result label.
            infoLabel.setText("Benchmark is running!");

            // Disable the Start button until the benchmark is done.
            benchmarkButton.setEnabled(false);

            // Enable the Cancel button while
            // the benchmark runs.
            cancelButton.setEnabled(true);

            // Start the asynchronous background task.
            worker.execute(test);
        } else {
            enableControls(true);
        }
    }

    // Hier kommen die Nachrichten waehrend des
    // Verlaufs des Hintergrundausfuehrung herein.
    @Override
    public void propertyChange(PropertyChangeEvent event) {
        if ("progress".equals(event.getPropertyName())) {
            progressBar.setValue((Integer) event.getNewValue());
        }
    }

    // This event handler deals with the results of the benchmark.
    void benchmarkCompleted(WorkerCompletedEvent event) {
        String msg;
        if (event.cancelled) {
            // The user canceled the benchmark.
            msg = "Benchmark was canceled.";
        } else if (event.error != null) {
            // There was an error during the benchmark.
            msg = "An error occurred.";

            JOptionPane.showMessageDialog(this, event.error.toString(), "Error in worker thread",
                    JOptionPane.ERROR_MESSAGE);

            winsole.writeLine();
            winsole.writeLine("Error in worker thread.");
            winsole.writeLine(event.error.toString());
            winsole.writeLine();
        } else {
            // The benchmark completed normally.
            msg = "Benchmark completed";
        }

        infoLabel.setText(msg);

        enableControls(true);

        // Enable the Start button.
        benchmarkButton.setEnabled(true);

        // Disable the Cancel button.
        cancelButton.setEnabled(false);

        test.sanityTest = false;

        progressBar.setValue(0);
    }
}
