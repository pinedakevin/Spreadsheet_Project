/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS5
 */

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS
{
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters,
    /// followed by one or more digits AND it satisfies the predicate IsValid.
    /// For example, "A15", "a15", "XY032", and "BC7" are cell names so long as they
    /// satisfy IsValid.  On the other hand, "Z", "X_", and "hello" are not cell names,
    /// regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized with the Normalize method before it is used by or saved in 
    /// this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    /// the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // Represents the input and value of the given cell
        private readonly Dictionary<string, Cell> cells;

        // Use for the dependency of each cell
        private readonly DependencyGraph dg;

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get;
            protected set;
        }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.  The variable
        /// equality test should be used thoughout to determine whether two variables are
        /// equal.
        /// </summary>
        public Spreadsheet(string filename, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            Changed = false;

            //Checks the file version and the version to see if they match  
            string fileVersion = GetSavedVersion(filename);
            if (fileVersion != version)
            {
                throw new SpreadsheetReadWriteException("Filename versions do not match.");
            }

            //reads the file name and any information. 
            ReadXML(filename);
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            return ReadXML(filename);
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new SpreadsheetReadWriteException("Your file does not exist.");
            }

            try
            {
                // We want some non-default settings for our XML writer.
                // Specifically, use indentation to make it more readable.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach (string name in cells.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", name);

                        if (cells[name].Contents is double d)
                        {
                            writer.WriteElementString("contents", d.ToString());
                        }
                        else if (cells[name].Contents is Formula f)
                        {
                            writer.WriteElementString("contents", "=" + f.ToString());
                        }
                        else
                        {
                            string text = cells[name].Contents.ToString();
                            writer.WriteElementString("contents", text);
                        }
                        //end of cell
                        writer.WriteEndElement();
                    }
                    // Ends spreadsheet
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new SpreadsheetReadWriteException("Did not write correctly due to issues " +
                    "opening, writing, or closing the file. Exception: " + e.Message);
            }

            Changed = false;
        }

        /// <summary>
        /// Read contents of XML File that represents a spreadsheet. 
        /// </summary>
        /// <param name="filename">Represents the Name of the file passed</param>
        /// <returns></returns>
        private string ReadXML(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new SpreadsheetReadWriteException("Did not read correctly due to being null or empty.");
            }

            string currVersion = string.Empty;
            string name = string.Empty;
            string content = string.Empty;

            try
            {
                // Create an XmlReader inside this block, and automatically Dispose() it at the end.
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    currVersion = reader["version"];
                                    break;
                                case "name":
                                    reader.Read();
                                    name = reader.Value;
                                    break;
                                case "contents":
                                    reader.Read();
                                    content = reader.Value;
                                    break;
                            }

                            if (name != string.Empty && content != string.Empty)
                            {
                                SetContentsOfCell(name, content);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("Did not read correctly due to issues " +
                             "opening, reading, or closing the file. Exception Message: " + e.Message);
            }
            return currVersion;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            return GetCellHelper(ref name, true);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            return GetCellHelper(ref name, false);
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            CellChecker(name);
            name = Normalize(name);

            if (content is null)
            {
                throw new ArgumentNullException();
            }

            if (double.TryParse(content, out double number))
            {
                return SetCellContents(name, number);
            }
            else if (content.StartsWith("="))
            {
                string formula = content.Substring(1, content.Length - 1);

                Formula f = new Formula(formula, Normalize, IsValid);

                return SetCellContents(name, f);
            }
            Changed = true;

            return SetCellContents(name, content);
        }

        /// <summary>
        /// The contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, double number)
        {
            name = Normalize(name);
            IList<string> cellContents = SetCellOperation(name, number);
            Changed = true;
            return cellContents;
        }

        /// <summary>
        /// The contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, string text)
        {
            name = Normalize(name);
            IList<string> cellContents = SetCellOperation(name, text);

            if (cells[name].Contents.Equals(string.Empty))
            {
                cells.Remove(name);
            }

            Changed = true;
            return cellContents;
        }

        /// <summary>
        /// If changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula. The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            name = Normalize(name);
            IList<string> cellContents = SetCellOperation(name, formula);
            Changed = true;
            return cellContents;
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            CellChecker(name);
            return dg.GetDependents(name);
        }

        /// <summary>
        /// Class that represents a cell of an object type. Cell will be
        /// a double, string, or forumla due to the set type in spreadsheet. 
        /// Has a secondary field that captures the reevaluated contents of a cell
        /// to be used in operations that require a reevaluated cell. 
        /// </summary>
        private class Cell
        {
            public object Contents
            { get; private set; }

            public object EvaluatedContents
            { get; set; }

            public Cell(object c, Func<string, double> lookup)
            {
                if (c is Formula f)
                {
                    EvaluatedContents = f.Evaluate(lookup);
                    Contents = c;
                }
                else
                {
                    Contents = c;
                    EvaluatedContents = c;
                }
            }
        }

        /// <summary>
        /// Checks the value of an evaluated cell. If cell is not a double
        /// it will throw an argument exception to allow formula error to be set
        /// by Formula Class. 
        /// 
        /// <para>
        /// Helper method for Spreadsheet class
        /// <para/>
        /// <returns></returns>
        private double Lookup(string name)
        {
            if (cells.ContainsKey(name) && cells[name].EvaluatedContents is double d)
            {
                return d;
            }
            else
            {
                throw new ArgumentException("value is not a double.");
            }
        }

        /// <summary>
        /// Checks if a cell's contents are null or is not a vaild
        /// variable (per the vaild variable rules) will throw an
        /// InvalidNameException. 
        /// 
        /// <para>
        /// Helper method for Spreadsheet class
        /// <para/>
        /// </summary>
        /// <param name="name">Represents the Name of a cell</param>
        private void CellChecker(string name)
        {
            if (name is null || !IsValidVariable(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Receives a string that is not one of the accepted strings for the algorithim 
        /// Checks if the variable is alphabetical lower/uppercase followed 
        /// by numbers. Returns a boolean of true if checks are passed.
        /// 
        /// <para>
        /// Helper method for Spreadsheet class
        /// <para/>
        /// </summary>
        /// <param name="token">Represents a variable token of type string</param>
        /// <returns></returns>
        private bool IsValidVariable(string token)
        {
            return Regex.IsMatch(token, "^([A-Za-z]+)([0-9]+)$");
        }

        /// <summary>
        /// Checks if the dictionary contains a cell. If it does not then it will
        /// return an empty string. If found it will provide either the value or
        /// the contents dependent on the users option.
        /// 
        /// <para>
        /// Helper method for GetCell methods
        /// <para/>
        /// </summary>
        /// <param name="name">Represents the name of a cell</param>
        /// <param name="getValue">Represents the option to be used</param>
        /// <returns></returns>
        private object GetCellHelper(ref string name, bool getValue)
        {
            CellChecker(name);
            name = Normalize(name);

            if (!cells.ContainsKey(name))
            {
                return string.Empty;
            }
            if (getValue)
            {
                return cells[name].EvaluatedContents;
            }
            else
            {
                return cells[name].Contents;
            }
        }

        /// <summary>
        /// Creates an operation to set a cell based on the contents of
        /// the dictionary cells. Once all initial cells are created, reevaluates and recalculates
        /// cells dependent on the new value that it holds.
        /// 
        /// <para>
        /// Helper method for SetCellOperation Methods.
        /// <para/>
        /// </summary>
        /// <param name="name">Represents the Name of a cell</param>
        /// <param name="obj">Represents the object type of the value of a cell</param>
        /// <returns></returns>
        private IList<string> SetCellOperation(string name, Object obj)
        {
            IList<string> cellContents = new List<string>();

            //used for formulas to initiate the dependees and calculate the cells of formulas.
            //If not a formula, it will replace the dependees with a new list that is dependet
            //to name. Addes the recalculated contents to the final cell content list.  
            if (obj is Formula f)
            {
                dg.ReplaceDependees(name, f.GetVariables());
                cellContents = new List<string>(GetCellsToRecalculate(name));
            }
            else
            {
                dg.ReplaceDependees(name, new List<string>());
                cellContents = new List<string>(GetCellsToRecalculate(name));
            }

            //adds a new key/val pair to the dictionary if there is none
            //if it exists in the dictionary, a new cell is added to the name(variable)
            if (!cells.ContainsKey(name))
            {
                cells.Add(name, new Cell(obj, Lookup));
            }
            else
            {
                cells[name] = new Cell(obj, Lookup);
            }

            //Reevaluating the contents of the formula when being changed
            foreach (string item in cellContents)
            {
                if (cells[item].Contents is Formula formula)
                {
                    cells[item].EvaluatedContents = formula.Evaluate(Lookup);
                }
            }

            return cellContents;
        }
    }
}
