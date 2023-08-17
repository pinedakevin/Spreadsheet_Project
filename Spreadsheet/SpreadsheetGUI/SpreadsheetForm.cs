/*
 * Cristian Tapieroo & Kevin Pineda
 * u1367608 & u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS6
 */

using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{

    public partial class SpreadsheetForm : Form
    {
        private AbstractSpreadsheet spreadsheet;

        /// <summary>
        /// Constructor for the spreadsheet form. Creates a new spreadsheet based on set
        /// requirements for a spreadsheet. Displays a spreadsheet panel when opening the program. 
        /// </summary>
        public SpreadsheetForm()
        {
            InitializeComponent();

            // Intializing spreadsheet with requirements based on the spreadsheet panel.
            spreadsheet = new Spreadsheet(s => Regex.IsMatch(s, "^[A-Z][1-9][0-9]?$"), s => s.ToUpper(), "ps6");

            SpreadsheetPanel.SelectionChanged += SpreadsheetDisplay;
            SpreadsheetPanel.SetSelection(0, 0);

            // When spreadsheet is initially opened, it will always start at A1
            // Initializing A1 when first Initalizing a Spreadsheet Form
            NameBox.Text = "A1";
            this.AcceptButton = EvalButton;
        }

        // Overloaded Constructor when you open a new file. 
        public SpreadsheetForm(string fileName)
        {
            InitializeComponent();

            // Intializing spreadsheet with requirements based on the spreadsheet panel.
            spreadsheet = new Spreadsheet(fileName, s => Regex.IsMatch(s, "^[A-Z][1-9][0-9]?$"), s => s.ToUpper(), "ps6");

            SpreadsheetPanel.SelectionChanged += SpreadsheetDisplay;

            SpreadsheetPanel.SetSelection(0, 0);

            // When spreadsheet is initially opened, it will always start at A1
            // Initializing A1 when first Initalizing a Spreadsheet Form
            NameBox.Text = "A1";
            this.AcceptButton = EvalButton;
        }

        /// <summary>
        /// Displays a spreadsheetpanel with set columns and rows. 
        /// </summary>
        /// <param name="ss"></param>
        private void SpreadsheetDisplay(SpreadsheetPanel ss)
        {
            SpreadsheetPanel.GetSelection(out int col, out int row);
            SpreadsheetPanel.GetValue(col, row, out string val);

            // Grabs the name of the cell using the helper method.
            string name = CellCoordToName(col, row);

            // Sets the text of the NameBox to the name of the cell and display.
            NameBox.Text = name;
            ValueBox.Text = val;

            // Gets the contents of the cell and places it into an object type
            object contents = spreadsheet.GetCellContents(name);

            // Checks if the contents is a formula, if so then it will pass the
            // formula using the correct format to the contentsbox. Else it will
            // set the ContentsBox to the tostring of contents. 
            if (contents is Formula formula)
            {
                ContentsBox.Text = "=" + formula.ToString();
            }
            else
            {
                ContentsBox.Text = contents.ToString();
            }
        }

        /// <summary>
        /// Provides a new empty spreadsheet in a new window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuEmptySpreadsheet_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            SpreadsheetApplicationContext.getAppContext().RunForm(new SpreadsheetForm());
        }

        /// <summary>
        /// Close menu item that closes a spreadsheet. Checks if
        /// the spreadsheet was changed and if so it will prompt
        /// the user if they want to save before closing. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (spreadsheet.Changed)
                {
                    DialogResult dialog = MessageBox.Show("Do you want to save your file before closing it", "Choose", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        SaveHelper();
                        Close();
                    }

                    // If user choose cancel, than it will cancel the open operation. 
                    else if (dialog == DialogResult.No)
                    {
                        Close();
                    }
                }
                else
                {
                    Close();
                }
            }
            catch (SpreadsheetReadWriteException srwe)
            {
                MessageBox.Show("A problem was encountered when saving your file: " + srwe.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Provides the user an option to view a help box on how to use the spreadsheet
        /// program. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAbout_Click(object sender, EventArgs e)
        {
            string text = "Edit a Cell - \n\t" + "Choose a any cell outside of the edge names to edit a cell.\n " +
                "To edit a cell use the available textbox and input any words or numbers to the cell. You can also formulate cells " +
                "using '=' and cell name (i.e A1, B2) as well as mathmatic symbol (+-*/()).";
            string text2 = "\n\n File - \n\t" + "New - \n\t"
                + "\t Creates a new spreadsheet and overwrites the existing spreadsheet." + "\n\t Open -" + "\n\t\t Opens a spreadsheet from the users specified directory."
                + "\n\t Save - \n\t\t" + "Saves a the current spreadsheet to the users specified directory." + "\n\t Close - "
                + "\n\t\t Closes the spreadsheet.";
            string text3 = "\n\nExtra Features - \n\t" + "For the extra features we changed the colors of the spreadsheet to mimic a darktheme and " +
                    "ask the user if closing a spreadsheet that has been modified using the X button on the top right of the window.";

            MessageBox.Show(text + text2 + text3 + "\n\n\n MUST USE THE MOUSE TO CHOOSE CELLS!", "Help Screen", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Open menu item that opens a new spreadsheet from the users
        /// directory of choice. Prompts the user if they want to save 
        /// their spreadsheet if it detects that a spreadsheet has been
        /// modified. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuOpen_Click(object sender, EventArgs e)
        {
            try
            {
                // Checks if the spreadsheet has been changed, If it has than a prompt will appear to the user
                // asking them if they want to save the file or cancel the operation. 
                if (spreadsheet.Changed)
                {
                    DialogResult dialog = MessageBox.Show("Do you want to save your file before opening a new one?\n Your file will be overwritten!", "Warning",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (dialog == DialogResult.Yes)
                    {
                        SaveHelper();
                    }
                    // If user choose cancel, than it will cancel the open operation. 
                    else if (dialog == DialogResult.Cancel)
                    {
                        return;
                    }
                }
            }
            catch (SpreadsheetReadWriteException srwe)
            {
                MessageBox.Show("A problem was encountered when saving your file: " + srwe.Message, "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

            try
            {
                SpreadsheetPanel.Clear();

                var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = ".sprd files (*.sprd)|*.sprd|All files (*.*)|*.*";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // recreate spreadsheet from saved file
                        // still need to add safety feature that opens the spreadsheet in different window if working 
                        // on different one.
                        spreadsheet = new Spreadsheet(openFileDialog.FileName, s => Regex.IsMatch(s, "^[A-Z][1-9][0-9]?$"), s => s.ToUpper(), "ps6");

                        IEnumerable<string> cells = spreadsheet.GetNamesOfAllNonemptyCells();
                        foreach (string cell in cells)
                        {
                            CellNameToCoords(cell, out int col, out int row);
                            SpreadsheetPanel.SetValue(col, row, spreadsheet.GetCellValue(cell).ToString());
                        }
                    }
                }
                MessageBox.Show("File opened successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SpreadsheetReadWriteException srwe)
            {
                MessageBox.Show("A problem was encountered when opening your file: " + srwe.Message, "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Allows the user to save the file in their directory
        /// of choice. Checks if there are any issues when saving 
        /// the file. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveHelper();
            }
            catch (SpreadsheetReadWriteException srwe)
            {
                MessageBox.Show("A problem was encountered when saving your file: " + srwe.Message, "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets the name of a cell and transfers the location
        /// into a column and row based on the spreadsheet. 
        /// 
        /// <para>
        /// Helper method for SpreadsheetPanel class
        /// <para/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void CellNameToCoords(string name, out int col, out int row)
        {
            char letter = name[0];
            col = letter - 'A';
            row = Int32.Parse(name.Substring(1)) - 1;
        }

        /// <summary>
        /// Button that allows the user to evaluate and set a cell
        /// in a spreadsheet. Provides a message if there are any issues
        /// when setting a cell. 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvalButton_Click(object sender, EventArgs e)
        {
            // Intializing the column and row of the cell and getting
            // the name of the cell(i.e A1,B1,etc.)
            SpreadsheetPanel.GetSelection(out int col, out int row);
            string name = CellCoordToName(col, row);
            IList<string> list = new List<string>();

            // Need to catch Circular Exception or a FormulaFormatException
            // and wrapping SetContentsOfCell to catch exception. 
            try
            {
                // Grabs the name of the cell(ex: A1,B5,etc.) and the contents of the cell from
                // the already set information in DisplaySelection method. 
                list = spreadsheet.SetContentsOfCell(NameBox.Text, ContentsBox.Text);

                // Checks each cell in the list of cells that is made when calling SetContentsOfCell.
                // This is to be used to update the setvalue of the spreadsheetpanel. 
                foreach (string cell in list)
                {
                    SpreadsheetPanel.SetValue(col, row, spreadsheet.GetCellValue(cell).ToString());
                }

                // Gets the value of the cell and places it into an object type
                object value = spreadsheet.GetCellValue(name);

                // Checks if the value of the cell is an error, if it is
                // an error that it will create the ValueText.Box to be
                // the reason of that error. Else it will make the value.ToString as the text.
                if (value is FormulaError error)
                {
                    ValueBox.Text = "!" + error.Reason;
                    SpreadsheetPanel.SetValue(col, row, ValueBox.Text);
                }
                else
                {
                    ValueBox.Text = value.ToString();
                }

            }
            catch (FormulaFormatException formulaException)
            {
                MessageBox.Show("There was an issue with your input: " + formulaException.Message, "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (CircularException)
            {
                MessageBox.Show("There was an issue with your input due to a circular issue when calculating cells.",
                    "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Recalculating cells to set them in the spreadsheetpanel and update
            // each cell if a cell it depends on is changed. 
            foreach (string cellName in list)
            {
                object val = spreadsheet.GetCellValue(cellName);
                CellNameToCoords(cellName, out int col1, out int row1);

                // Checks if the value of the cell is an error, if it is
                // an error than it will set the reason of that error as the value. 
                if (val is FormulaError error)
                {
                    SpreadsheetPanel.SetValue(col1, row1, "!" + error.Reason);
                }
                else
                {
                    SpreadsheetPanel.SetValue(col1, row1, val.ToString());
                }
            }
        }

        /// <summary>
        /// Prompts the user if they want to save if a spreadsheet has been edited.
        /// Gives the user the option to save, stop the operation, or simply close. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //this code keeps the window form open while the user is prompted to be saved
                if (spreadsheet.Changed)
                {
                    e.Cancel = true;
                    DialogResult dialog = MessageBox.Show("Do you want to save your file before closing it", "Choose", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        SaveHelper();
                        Close();
                    }
                    // If spreadsheet doesnt change before closing it just close it
                    e.Cancel = false;
                }
            }
            catch (SpreadsheetReadWriteException srwe)
            {
                MessageBox.Show("A problem was encountered when saving your file: " + srwe.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Uses the column and row of the selected cell and 
        /// recovers the name of the cell using the algorithim.
        /// Algorithim adjusts the values into char depending
        /// on the location of the row and column. Once it is found
        /// it returns a string of that cell in uppercase. 
        /// 
        /// <para>
        /// Helper method for SpreadsheetPanel class
        /// <para/>
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string CellCoordToName(int col, int row)
        {
            return $"{(char)('A' + col)}{(row + 1)}";
        }

        /// <summary>
        /// Creates a savefiledialog to be used whenever
        /// the file needs to be saved.
        /// </summary>
        private void SaveHelper()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "sprd files (*.sprd)|*.sprd|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                spreadsheet.Save(saveFileDialog1.FileName);
            }
        }
    }
}
