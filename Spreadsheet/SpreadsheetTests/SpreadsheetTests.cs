/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS5 Tests
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        //********UNIQUE TESTS***********\\

        [TestMethod]
        public void EmptyConstructorTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
        }

        [TestMethod]
        public void EmptyCellTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("a1"));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullNameTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "5");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFormulaCellTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", null);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullStringCellTest()
        {
            string s = null;
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", s);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NotValidlCellTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("59.90", "5");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NotValidlCellTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1-", "5");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NotValidlCellTest3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1+5", "5");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NotValidlCellTest4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("5a1", "5");
        }

        [TestMethod]
        public void GetCellDoubleTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "5");
            Assert.AreEqual(5.0, sheet.GetCellContents("a1"));
            
        }

        [TestMethod]
        public void GetCellDoubleTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "1e2");
            Assert.AreEqual(1e2, sheet.GetCellContents("a1"));
            Assert.AreEqual(100.0, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void GetCellDoubleTest3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "6");
            Assert.AreEqual(6.0, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void GetCellDoubleTest4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "-100");
            Assert.AreEqual(-100.0, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void GetCellStringTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "-100");
            Assert.AreEqual(-100.0, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void GetCellStringTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "-100");
            Assert.AreEqual(-100.0, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void FileSaveTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string f1 = ("=S3 + B2");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("D2", "3.0");
            sheet.SetContentsOfCell("S3", "3.0");
            sheet.SetContentsOfCell("B2", "3.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("B3", "3.0");
            sheet.SetContentsOfCell("F1", "3.0");
            sheet.SetContentsOfCell("F2", "=A1+D2");
            sheet.SetContentsOfCell("F1", "hello");
            sheet.Save("spreadsheet.txt");
        }

        [TestMethod()]
        public void SaveTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5.0");
            s.Save("save1.txt");
            Assert.AreEqual(s.GetCellContents("A1"), 5.0);
            
        }

        [TestMethod]
        public void FileReadSaveTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string f1 = ("=S3 + B2");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("D2", "3.0");
            sheet.SetContentsOfCell("S3", "3.0");
            sheet.SetContentsOfCell("B2", "3.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("B3", "3.0");
            sheet.SetContentsOfCell("F1", "3.0");
            sheet.SetContentsOfCell("F2", "=A1+D2");
            sheet.SetContentsOfCell("F1", "hello");
            sheet.Save("spreadsheet.txt");

            AbstractSpreadsheet s = new Spreadsheet("spreadsheet.txt", x => true, x => x.ToUpper(), "default");

        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void FileReadErrorTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string f1 = ("!!!");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("D2", "3.0");
            sheet.SetContentsOfCell("S3", "3.0");
            sheet.SetContentsOfCell("B2", "3.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("B3", "3.0");
            sheet.SetContentsOfCell("F1", "3.0");
            sheet.SetContentsOfCell("F2", "=A1+D2");
            sheet.SetContentsOfCell("F1", "hello");
            sheet.Save("spreadsheet.txt");

            AbstractSpreadsheet s = new Spreadsheet("spreadsheet.txt", x => true, x => x.ToUpper(), "default");
            sheet.SetContentsOfCell("F10", "hello");
            AbstractSpreadsheet s1 = new Spreadsheet(x => true, x => x.ToUpper(), "default");
            sheet.Save("spreadsheet.txt");
            AbstractSpreadsheet ss = new Spreadsheet("1//213af/\\//}{{}.///save.txt", s => true, s => s, "");

        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void FileSaveErrorTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string f1 = ("=S3 + B2");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("D2", "3.0");
            sheet.SetContentsOfCell("S3", "3.0");
            sheet.SetContentsOfCell("B2", "3.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("B3", "3.0");
            sheet.SetContentsOfCell("F1", "3.0");
            sheet.SetContentsOfCell("F2", "=A1+D2");
            sheet.SetContentsOfCell("F1", "hello");
            sheet.Save("1//213af/\\//}{{}.///");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void NullFileSaveTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string f1 = ("=S3 + B2");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("D2", "3.0");
            sheet.SetContentsOfCell("S3", "3.0");
            sheet.SetContentsOfCell("B2", "3.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("B3", "3.0");
            sheet.SetContentsOfCell("F1", "3.0");
            sheet.Save(null);
        }

        [TestMethod]
        public void SetCellDoubleTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> tempList = new List<string>(); 
            tempList = (List<string>)sheet.SetContentsOfCell("a1", "-100");
            Assert.AreEqual(1, tempList.Count);
        }

       [TestMethod]
        public void SetCellTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> tempList = new List<string>(); 
            sheet.SetContentsOfCell("a1", "10");
            sheet.SetContentsOfCell("a1", "100");
            sheet.SetContentsOfCell("a1", "c2");
            sheet.SetContentsOfCell("c1", "a1");
            sheet.SetContentsOfCell("a1", "b3");
            sheet.SetContentsOfCell("A1", "Q3");
            sheet.SetContentsOfCell("B1", "a1");
            tempList = (List<string>)sheet.SetContentsOfCell("B1", "0");

            foreach (string item in tempList)
            {
                Console.Write(item + ", ");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> tempList = new List<string>();
            string f1 = ("=A1 + B2");
            string f2 = ("=A1 + B3");
            string f3 = ("=B1 + A1");
            string f4 = ("=C1 + A1");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("A1", f2);
            sheet.SetContentsOfCell("A1", f3);
            sheet.SetContentsOfCell("A1", f4);
            tempList = (List<string>)sheet.SetContentsOfCell("A1", "0");

            foreach (string item in tempList)
            {
                Console.Write(item + ", ");
            }
        }

        [TestMethod]
        public void SetCellTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            IList<string> tempList = new List<string>();
            string f1 = ("=A1 * 2");
            string f2 = ("=B1 + A1");

            sheet.SetContentsOfCell("B1", f1);
            sheet.SetContentsOfCell("C1", f2);
            tempList = (List<string>)sheet.SetContentsOfCell("A1", "0");

            foreach (string item in tempList)
            {
                Console.Write(item + ", ");
            }

            Console.WriteLine();

            IList<string> copy = new List<string> {"A1", "B1", "C1"};
            int counter = 0;
            foreach (string x in tempList) 
            {
                
                Console.WriteLine(x + " " + copy[counter] + " " + x.Equals(copy[counter]));
                Assert.AreEqual(x, copy[counter]);
                counter++;
            }
            
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FormulaFormatExceptionTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> tempList = new List<string>();
            string f1 = ("=S3 + B@#2");
            string f2 = ("=D2 + B3");
            string f3 = ("=B1 + F1");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("C1", f2);
            sheet.SetContentsOfCell("D1", f3);
            sheet.SetContentsOfCell("Z1", "");
            sheet.SetContentsOfCell("X1", "");

            AbstractSpreadsheet sheetCopy = new Spreadsheet();

        }

        [TestMethod]
        
        public void GetNoneEmptyCellsTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> tempList = new List<string>();
            string f1 = ("=S3 + B2");
            string f2 = ("=D2 + B3");
            string f3 = ("=B1 + F1");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("C1", f2);
            sheet.SetContentsOfCell("D1", f3);
            sheet.SetContentsOfCell("Z1", "");
            sheet.SetContentsOfCell("X1", "");

            AbstractSpreadsheet sheetCopy = new Spreadsheet();

            //Assert.IsFalse((f1.ToString()) == (f2.ToString()));
            IEnumerator<string> enum1 = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            IEnumerator<string> enum2 = sheetCopy.GetNamesOfAllNonemptyCells().GetEnumerator();

            Assert.IsTrue(enum1.MoveNext());
            Assert.IsFalse(enum2.MoveNext());

        }

        [TestMethod]
        public void FormulaTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");
            List<string> tempList = new List<string>();
            string f1 = ("=S3 + B2");
            string f2 = ("=D2 + B3");
            string f3 = ("=B1 + F1");

            sheet.SetContentsOfCell("A1", f1);
            sheet.SetContentsOfCell("C1", f2);
            sheet.SetContentsOfCell("D1", f3);
            sheet.SetContentsOfCell("Z1", "");
            sheet.SetContentsOfCell("X1", "");
            Console.WriteLine(sheet.GetCellValue("A1")); 

            AbstractSpreadsheet sheetCopy = new Spreadsheet();
        }

        [TestMethod]

        public void GetTest1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
    
            IList<string> tempList = new List<string>();
            IList<string> tempList2 = new List<string>();

            tempList.Add("ab");
            tempList.Add("a2");
            tempList.Add("a3");
            tempList.Add("ac");

            tempList2.Add("ab");
            tempList2.Add("a2");
            tempList2.Add("a3");
            tempList2.Add("ac");

            CollectionAssert.AreEqual(tempList.ToList(), tempList2.ToList());
        }

        [TestMethod()]
        public void TestFormulaError()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", ("=A2+A3"));
            Assert.IsInstanceOfType(s.GetCellValue("A1"), typeof(FormulaError));
        }


        [TestMethod()]
        public void TestChanged()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual(s.Changed, false);
            s.SetContentsOfCell("A1", ("5.0"));
            Assert.AreEqual(s.Changed, true);
            Assert.AreEqual(s.GetCellValue("A1"), 5.0);
        }

        [TestMethod()]
        public void TestNonExsistantCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual(s.GetCellValue("Z1"), string.Empty);
        }

        [TestMethod()]
        public void TestOverloadedConstructor1()
        {
            AbstractSpreadsheet s = new Spreadsheet( x => true, x => x.ToUpper(), "default");
            Assert.AreEqual(s.GetCellValue("Z1"), string.Empty);
        }

        [TestMethod()]
        public void TestOverloadedConstructor2()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");
            s.Save("save.txt");
            AbstractSpreadsheet s1 = new Spreadsheet("save.txt" ,x => true, x => x.ToUpper(), "save.txt");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestVersionError()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x.ToUpper(), "default");
            s.Save("save.txt");
            AbstractSpreadsheet s1 = new Spreadsheet("save.txt", x => true, x => x.ToUpper(), "save.txt");
        }

        [TestMethod()]
        public void TestGetVersion()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");
            s.Save("save.txt");
            AbstractSpreadsheet s1 = new Spreadsheet("save.txt", x => true, x => x.ToUpper(), "save.txt");
            Assert.AreEqual("save.txt", s1.GetSavedVersion("save.txt"));
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestEmptyNullXML()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x.ToUpper(), null);
            AbstractSpreadsheet s1 = new Spreadsheet("", x => true, x => x.ToUpper(), "");
        }

        /*********STRESS TESTS*********/
        [TestMethod()]
        public void HeavyFormulaTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");

            int n = 100;
            for (int i = 0; i < n; i++)
            {
                sheet.SetContentsOfCell("A" + i, "" + i);
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellContents("A" + i));
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellValue("A" + i));
            }
        }

        [TestMethod(), Timeout(2000)]
        public void HeavyFormulaTest2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");

            int n = 1000;
            for (int i = 0; i < n; i++)
            {
                sheet.SetContentsOfCell("A" + i, "" + i);
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellContents("A" + i));
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellValue("A" + i));
            }
        }

        [TestMethod(), Timeout(2000)]
        public void HeavyFormulaTest3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");

            int n = 10000;
            for (int i = 0; i < n; i++)
            {
                sheet.SetContentsOfCell("A" + i, "" + i);
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellContents("A" + i));
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellValue("A" + i));
            }
        }

        [TestMethod(), Timeout(2000)]
        public void HeavyFormulaTest4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(x => true, x => x.ToUpper(), "save.txt");

            int n = 100000;
            for (int i = 0; i < n; i++)
            {
                sheet.SetContentsOfCell("A" + i, "" + i);
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellContents("A" + i));
            }

            for (int i = 0; i < n; i++)
            {

                Assert.AreEqual((double)i, sheet.GetCellValue("A" + i));
            }
        }
    }
}
