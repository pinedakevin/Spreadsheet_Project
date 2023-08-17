/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS3 Tests
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;
using System.Linq;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        //Normalizor for tests
        public string Normalize(string s) 
        {
            return s.ToUpper();
        }

        //Validator for tests
        public bool isValid(string s) 
        {
            return true;
        }
        /// <summary>
        /// Test a single number.
        /// </summary>
        [TestMethod()]
        public void TestSingleNumber()
        {
            Formula f = new Formula("5.0");
            Assert.AreEqual(5.0, f.Evaluate(s => 0));
        }
        /// <summary>
        /// Test a single number.
        /// </summary>
        [TestMethod()]
        public void TestScientificNumber()
        {
            Formula f = new Formula("1e3");
            Assert.AreEqual(1e3, f.Evaluate(s => 0));
        }
        /// <summary>
        /// Tests a single variable.
        /// </summary>
        [TestMethod()]
        public void TestSingleVariable()
        {
            Formula f = new Formula("_1");
            Assert.AreEqual(13.0, f.Evaluate(s => 13));
        }
        /// <summary>
        /// Tests Addition with a variable.
        /// </summary>
        [TestMethod()]
        public void TestAdditionWVariable()
        {
            Formula f = new Formula("2+_1");
            Assert.AreEqual(12.0, f.Evaluate(s => 10));
        }
        /// <summary>
        /// Tests with parenthesis on the left side.
        /// Following order of operations
        /// </summary>
        [TestMethod()]
        public void TestParenFirst()
        {
            Formula f = new Formula("(1.0*1.0)-5/5");
            Assert.AreEqual(0.0, f.Evaluate(s => 0));
        }
        /// <summary>
        /// Tests with parenthesis on the right side.
        /// Following order of operations.
        /// </summary>
        [TestMethod()]
        public void TestParenLast()
        {
            Formula f = new Formula("2+3.0*(3.0+5.0)");
            Assert.AreEqual(26.0, f.Evaluate(s => 0));
        }
        /// <summary>
        /// Tests a complex order of operations.
        /// </summary>
        [TestMethod()]
        public void TestOrderOfOps()
        {
            Formula f = new Formula("2+3.0*5+(3+4*8.0)*5.0+2.0");
            Assert.AreEqual(194.0, f.Evaluate(s => 0));
        }
        /// <summary>
        /// Tests a complex order of operations with division
        /// mixed in.
        /// </summary>
        [TestMethod()]
        public void TestOrderOps2()
        {
            Formula f = new Formula("2+3/5.0+(3+4.0/8.0)/5+2");
            Assert.AreEqual((2.0 + 3.0 / 5.0 + (3.0 + 4.0 / 8.0) / 5.0 + 2.0), f.Evaluate(s => 0));
        }

        /// <summary>
        /// Tests if a division by zero returns the object
        /// of FormulaError with the correct message.
        /// </summary>
        [TestMethod()]
        public void TestDivideByZero()
        {
            Formula f = new Formula("1/0");
            Assert.IsInstanceOfType(f.Evaluate(x => 0), typeof(FormulaError));
            object error = f.Evaluate(x => 0);
            if (error is FormulaError formErr)
            {
                string errorReason = formErr.Reason;
                string typedReason = "You cannot divide by zero. " +
                            "Or your formula resulted in a divide by zero.";
                Assert.AreEqual(typedReason, errorReason);
            }
        }
        /// <summary>
        /// Tests a complex order of operations with variables.
        /// </summary>
        [TestMethod()]
        public void TestOrderOpsWVariable()
        {
            Formula f = new Formula("x_1*5.0 - 5 / 5 + 5 * (5 - 5.0 * 5) / 5.0 * x_2");
            Assert.AreEqual((4.0 * 5.0 - 5.0 / 5.0 + 5.0 * (5.0 - 5.0 * 5.0) / 5.0 * 2.0)
                , f.Evaluate(s => (s == "x_2") ? 2 : 4));
        }
        /// <summary>
        /// Tests a complex order of operations with variables that
        /// differ in complexity.
        /// </summary>
        [TestMethod()]
        public void TestOrderOpsWVariable2()
        {
            Formula f = new Formula("y1*3.0-8.0/2+4/(8.0-9.0*2)/14.0*x7");
            Assert.AreEqual((4.0 * 3.0 - 8.0 / 2.0 + 4.0 / (8.0 - 9.0 * 2.0) / 14.0 * 1.0), f.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        /// <summary>
        /// Tests a complex order of operations with variables
        /// and a series of parenthesis leading to the right.
        /// </summary>
        [TestMethod()]
        public void TestAllVarMixed()
        {
            Formula f = new Formula("a_1+(b_2+(c_3+(d_4+(e_5+(f_6+g_7)))))");
            Assert.AreEqual(70.0, f.Evaluate(s => 10));
        }
        /// <summary>
        /// Tests a complex order of operations with variables
        /// and a series of parenthesis leading to the left.
        /// </summary>
        [TestMethod()]
        public void TestAllVarMixed2()
        {
            Formula f = new Formula("(((((((F_6+E_5)+D_4)+c_3)+b_2)+A_1)-G_7)-h_134134)");
            Assert.AreEqual(8.0, f.Evaluate(s => 2));
        }
        /// <summary>
        /// Tests the GetVariable method and ensures
        /// that the correct variables are given.
        /// </summary>
        [TestMethod()]
        public void TestGetVariables()
        {
            Formula f = new Formula("y1*3-8/2.0+4/(8-9.0*2.0)/14.0*x7");
            f.GetVariables();
        }
        /// <summary>
        /// Tests the GetHashCode method to give the same hash code
        /// to formulas that are the same. 
        /// </summary>
        [TestMethod()]
        public void TestGetHashCode1()
        {
            Formula f1 = new Formula("xx+xx");
            Formula f2 = new Formula("xx+xx");
            Assert.AreEqual(true, f1.GetHashCode() == f2.GetHashCode());

            Formula f3 = new Formula("xx+xx", x => x.ToUpper(), x => true);
            Formula f4 = new Formula("xx+xx", x => x.ToUpper(), x => true);
            Assert.AreEqual(true, f3.GetHashCode() == f4.GetHashCode());

            Formula f5 = new Formula("xx+xx", Normalize, isValid);
            Formula f6 = new Formula("xx+xx", Normalize, isValid);
            Assert.AreEqual(true, f5.GetHashCode() == f6.GetHashCode());
        }
        /// <summary>
        /// Tests the GetHashCode method to give the same hash code
        /// to formulas that are the same, after normalization and validation,
        /// and formulas that differ should not have the same hashcode. 
        /// </summary>
        [TestMethod()]
        public void TestGetHashCode2()
        {
            Formula f1 = new Formula("xx+XX");
            Formula f2 = new Formula("xx+xx");
            Assert.AreEqual(false, f1.GetHashCode() == f2.GetHashCode());

            Formula f3 = new Formula("xx+XX", x => x.ToUpper(), x => true);
            Formula f4 = new Formula("xx+xx", x => x.ToUpper(), x => true);
            Assert.AreEqual(true, f3.GetHashCode() == f4.GetHashCode());

            Formula f5 = new Formula("xx+XX", Normalize, isValid);
            Formula f6 = new Formula("xx+xx", Normalize, isValid);
            Assert.AreEqual(true, f5.GetHashCode() == f6.GetHashCode());
        }
        /// <summary>
        /// Tests the ToString Method to return the same string for
        /// formulas that are the same. 
        /// </summary>
        [TestMethod()]
        public void TestToString1()
        {
            string formula = "xx+xx";
            Formula f1 = new Formula("xx+xx");
            Assert.AreEqual(true, f1.ToString().Equals(formula));
            Assert.AreEqual(true, formula.Equals(f1.ToString()));
        }
        /// <summary>
        /// Tests the ToString Method to return the different strings for
        /// formulas that are not the same. 
        /// </summary>
        [TestMethod()]
        public void TestToString2()
        {
            string formula = "xv+xx";
            Formula f1 = new Formula("xx+xx");
            Assert.AreEqual(false, f1.ToString().Equals(formula));
            Assert.AreEqual(false, formula.Equals(f1.ToString()));
        }
        /// <summary>
        /// Tests the ToString Method to return the same string for
        /// formulas that are the same after normalization and validation. 
        /// </summary>
        [TestMethod()]
        public void TestToString3()
        {
            Formula f1 = new Formula("xx+xx", x => x.ToUpper(), x => true);
            Formula f2 = new Formula("XX+XX", x => x.ToUpper(), x => true);
            Assert.AreEqual(true, f1.ToString().Equals(f2.ToString()));

            Formula f3 = new Formula("xx+xx", Normalize, isValid);
            Formula f4 = new Formula("XX+XX", Normalize, isValid);
            Assert.AreEqual(true, f3.ToString().Equals(f4.ToString()));
        }
        /// <summary>
        /// Tests the equals operator to give true if two strings
        /// are the same formula with validation and normalization
        /// or without.
        /// </summary>
        [TestMethod()]
        public void TestEqualsOperator()
        {
            Formula f1 = new Formula("bb+bb");
            Formula f2 = new Formula("bb+bb");
            Assert.AreEqual(true, f1 == f2);

            Formula f3 = new Formula("bb+bb", x => x.ToUpper(), x => true);
            Formula f4 = new Formula("bb+bb", x => x.ToUpper(), x => true);
            Assert.AreEqual(true, f3 == f4);

            Formula f5 = new Formula("bb+bb", Normalize, isValid);
            Formula f6 = new Formula("bb+bb", Normalize, isValid);
            Assert.AreEqual(true, f5 == f6);
        }
        /// <summary>
        /// Tests the equals operator to give true if two strings
        /// are the same formula after validation and normalization
        /// </summary>
        [TestMethod()]
        public void TestEqualsOperator2()
        {
            Formula f1 = new Formula("bb+bb", x => x.ToUpper(), x => true);
            Formula f2 = new Formula("BB+BB", x => x.ToUpper(), x => true);
            Assert.AreEqual(true, f1 == f2);

            Formula f3 = new Formula("bb+bb", Normalize, isValid);
            Formula f4 = new Formula("BB+BB", Normalize, isValid);
            Assert.AreEqual(true, f3 == f4);
        }
        /// <summary>
        /// Tests the equals operator to give false if two strings
        /// are not the same formula after validation and normalization
        /// or without.
        /// </summary>
        [TestMethod()]
        public void TestEqualsOperator3()
        {
            Formula f1 = new Formula("bcb+bb");
            Formula f2 = new Formula("bb+bb");
            Assert.AreEqual(false, f1 == f2);

            Formula f3 = new Formula("bcb+bb", x => x.ToUpper(), x => true);
            Formula f4 = new Formula("BB+BB", x => x.ToUpper(), x => true);
            Assert.AreEqual(false, f3 == f4);

            Formula f5 = new Formula("bcb+bb", Normalize, isValid);
            Formula f6 = new Formula("BB+BB", Normalize, isValid);
            Assert.AreEqual(false, f5 == f6);
        }
        /// <summary>
        /// Tests the does not equals operator to give true if two strings
        /// are the a different formula with validation and normalization
        /// or without.
        /// </summary>
        [TestMethod()]
        public void TestDNEOperator1()
        {
            Formula f1 = new Formula("a3+aa");
            Formula f2 = new Formula("a2+aa");
            Assert.AreEqual(true, f1 != f2);

            Formula f3 = new Formula("a2+aa", x => x.ToUpper(), x => true);
            Formula f4 = new Formula("a3+aa", x => x.ToUpper(), x => true);
            Assert.AreEqual(true, f1 != f2);

            Formula f5 = new Formula("a3+aa", Normalize, isValid);
            Formula f6 = new Formula("a2+aa", Normalize, isValid);
            Assert.AreEqual(true, f1 != f2);
        }
        /// <summary>
        /// Tests the does not equals operator to give false if two strings
        /// are the same formula with validation and normalization
        /// or without.
        /// </summary>
        [TestMethod()]
        public void TestDNEOperator2()
        {
            Formula f1 = new Formula("aa+aa");
            Formula f2 = new Formula("aa+aa");
            Assert.AreEqual(false, f1 != f2);

            Formula f3 = new Formula("aas+aa", x => x.ToUpper(), x => true);
            Formula f4 = new Formula("aas+aa", x => x.ToUpper(), x => true);
            Assert.AreEqual(false, f3 != f4);

            Formula f5 = new Formula("aas+aa", Normalize, isValid);
            Formula f6 = new Formula("aas+aa", Normalize, isValid);
            Assert.AreEqual(false, f5 != f6);
        }

        /// <summary>
        /// Tests the equals method to work correctly
        /// with two formulas that are the same. 
        /// </summary>
        [TestMethod()]
        public void TestEquals1()
        {
            Formula f1 = new Formula("xx+xx");
            Formula f2 = new Formula("xx+xx");
            Assert.AreEqual(true, f1.Equals(f2));
        }
        /// <summary>
        /// Tests the equals method to work correctly
        /// with two formulas that are not the same. 
        /// </summary>
        [TestMethod()]
        public void TestEquals2()
        {
            Formula f1 = new Formula("xfx+xx");
            Formula f2 = new Formula("xx+xx");
            Assert.AreEqual(false, f1.Equals(f2));
        }
        /// <summary>
        /// Tests the equals method to work correctly
        /// with two formulas that are different objects. 
        /// </summary>
        [TestMethod()]
        public void TestEquals3()
        {
            Formula f1 = new Formula("xx+xx");
            object fake = new object();
            Assert.AreEqual(false, f1.Equals(fake));
        }
        /// <summary>
        /// Tests the equals method to work correctly
        /// with the left formula is null. 
        /// </summary>
        [TestMethod()]
        public void TestEquals4()
        {
            Formula f1 = new Formula("xx+xx");
            Formula f2 = null;
            Assert.AreEqual(false, f1.Equals(f2));
        }
        /// <summary>
        /// Tests the equals method to work correctly
        /// with the right formula is null. 
        /// </summary>
        [TestMethod()]
        public void TestEquals5()
        {
            Formula f1 = null;
            Formula f2 = new Formula("xx+xx");
            Assert.AreEqual(false, f1 == f2);
        }

        //*********Exception Tests********************

        /// <summary>
        /// Tests if an invalid token and provides the 
        /// correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestVariable()
        {
            Formula f = new Formula("10+_z5%");
            f.Evaluate(s => 0);
        }

        /// <summary>
        /// Tests if an empty operator and provides the 
        /// correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOnlyOp()
        {
            Formula f = new Formula("+");
            f.Evaluate(s => 0);

        }
        /// <summary>
        /// Tests if an extra operator exists and provides the 
        /// correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnbalancedOp()
        {
            Formula f = new Formula("1000+100+");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if a variable is invalid during an operation and provides the 
        /// correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestAddIllegaldVariable()
        {
            Formula f = new Formula("5+0yy___08");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if the parenthesis are unbalanced and provides the 
        /// correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnbalancedParen()
        {
            Formula f = new Formula("100+100*100)");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if the an operator follows a parenthesis  and provides 
        /// the correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenUnbalancedOp()
        {
            Formula f = new Formula("100+100+(100)100");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if there is not token and provides the correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            Formula f = new Formula("");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if a variable is invalid and provides the 
        /// correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestIllegalVariable()
        {
            Formula f = new Formula("0yy___08");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if a variable is invalid during an operation and provides the 
        /// correct FormulaFormatException.
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestIllegalVariable2()
        {
            Formula f = new Formula("2+_$#2X1");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if a variable is invalid during an operation and provides the 
        /// correct FormulaFormatException.
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestIllegalVariable3()
        {
            Formula f = new Formula("2+x_2.0");
            f.Evaluate(s => 0);
            List<string> temp = f.GetVariables().ToList();

            foreach (string s in temp) 
            {
                System.Console.WriteLine(s);
            }
        }
        /// <summary>
        /// Tests if an number, variable, or an opending parenthesis follows an operator 
        /// and provides the correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMultiOperators()
        {
            Formula f = new Formula("2+++++");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if the the closing parenthesis is balanced and 
        /// provides the correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnbalancedRightParen()
        {
            Formula f = new Formula("1 + 1) / 1 + 1)");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if the the opening parenthesis is balanced and 
        /// provides the correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestUnbalancedLeftParen()
        {
            Formula f = new Formula("(1 + 1) / ((1 + 1");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// Tests if the validator and valid variable is not correct and
        /// provides the correct FormulaFormatException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidVarandValidator()
        {
            Formula f = new Formula("Q_Q + x_x", x => "0", x => true);
        }

    }
}
