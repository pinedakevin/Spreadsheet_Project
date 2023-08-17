/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS1
 */

///THis is a branching test
///wafewa
/////Second branching test


using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static FormulaEvaluator.Evaluator;

namespace FormulaEvaluatorTestor
{
    /// <summary>
    /// Testing program to be uses with FormulaEvaluator.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Testing inputs to outputs using only integers
            Console.WriteLine("Testing integer arguments");
            Console.WriteLine("---------------------------");

            Test("1 + 2 + 3", SimpleLookup, 6);
            Test("1 + 2 * 3", SimpleLookup, 1 + 2 * 3);
            Test("1 + 2 / 3", SimpleLookup, 1 + 2 / 3);
            Test("1 + 2 / 3 - 2", SimpleLookup, 1 + 2 / 3 - 2);
            Test("(1 + (1 + (1 + 1)))", SimpleLookup, 4);
            Test("(3 * 3)+(5 - 4)", SimpleLookup, 10);
            Test("4+ 4 /2", SimpleLookup, (4 + 4 / 2));
            Test("4+ 4 /2*2", SimpleLookup, (4 + 4 / 2 * 2));
            Test("4+(4+4)", SimpleLookup, 4 + (4 + 4));
            Test("4+(4+4)-2", SimpleLookup, 4 + (4 + 4) - 2);
            Test("(2+8) *(15-5)", SimpleLookup, (2 + 8) * (15 - 5));
            Test("8*(15 -10)", SimpleLookup, 8 * (15 - 10));
            Test("(2+2) + (2+8) * (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) * (15 - 5));
            Test("(2+2) + (2+8) + (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) + (15 - 5));
            Test("(2+2) + (2+8) / (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) / (15 - 5));
            Test("(2+2) * (2+8) / (15-5) ", SimpleLookup, (2 + 2) * (2 + 8) / (15 - 5));
            Test("(2+2) / (2+8) + (15-5) ", SimpleLookup, (2 + 2) / (2 + 8) + (15 - 5));
            Test("(2+2) * (2+8) + (15-5) ", SimpleLookup, (2 + 2) * (2 + 8) + (15 - 5));
            Test("(2+2) - (2+8) - (15-5) ", SimpleLookup, (2 + 2) - (2 + 8) - (15 - 5));
            Test("(2+2) + (2+8) - (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) - (15 - 5));

            ////Testing inputs to outputs using only integers
            Console.WriteLine("\nTesting variable arguments");
            Console.WriteLine("---------------------------");

            Test("(1+ A1)", SimpleLookup, 2);
            Test("(1+ B2)", SimpleLookup, 3);
            Test("(1-1)/(1+1)", SimpleLookup, 0);
            Test("1 + B2 + 3", SimpleLookup, 6);
            Test("1 + B2 * 3", SimpleLookup, 1 + 2 * 3);
            Test("A1 + B2 / 3", SimpleLookup, 1 + 2 / 3);
            Test("1 + B2 / C3 - B2", SimpleLookup, 1 + 2 / 3 - 2);
            Test("1 + A1 + 3", SimpleLookup, 1 + 1 + 3);
            Test("1 + B2 + 3", SimpleLookup, 1 + 2 + 3);
            Test("(1+ B2)", SimpleLookup, 1 + 2);
            Test("(3 * 3)+(A1 - 1)", SimpleLookup, (3 * 3) + (1 - 1));
            Test("4+ B2 /2", SimpleLookup, (4 + 2 / 2));
            Test("4+ 4 /B2*2", SimpleLookup, (4 + 4 / 2 * 2));
            Test("4+(B2+4)", SimpleLookup, 4 + (2 + 4));
            Test("4+(4+4)-B2", SimpleLookup, 4 + (4 + 4) - 2);
            Test("(C3+8) *(15-5)", SimpleLookup, (3 + 8) * (15 - 5));
            Test("B2*(15 -10)", SimpleLookup, 2 * (15 - 10));
            Test("(2+B2) + (2+8) * (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) * (15 - 5));
            Test("(B2+2) + (2+8) + (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) + (15 - 5));
            Test("(2+2) + (B2+8) / (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) / (15 - 5));
            Test("(2+B2) * (B2+8) / (15-5) ", SimpleLookup, (2 + 2) * (2 + 8) / (15 - 5));
            Test("(B2+B2) / (B2+8) + (15-5) ", SimpleLookup, (2 + 2) / (2 + 8) + (15 - 5));
            Test("(2+2) * (B2+8) + (15-5) ", SimpleLookup, (2 + 2) * (2 + 8) + (15 - 5));
            Test("(2+2) - (B2+8) - (15-5) ", SimpleLookup, (2 + 2) - (2 + 8) - (15 - 5));
            Test("(B2+2) + (2+8) - (15-5) ", SimpleLookup, (2 + 2) + (2 + 8) - (15 - 5));
            Test("1 + b2 + 3", SimpleLookup, 5);
            Test("1 + b2 * 3", SimpleLookup, 1 + 1 * 3);
            Test("a1 + b2 / 3", SimpleLookup, 1 + 2 / 3);
            Test("1 + Ab2 + 3", SimpleLookup, 5);
            Test(" 200* (( 50 + 100 - 100 / 5) - (3 + 2 * 2) + 2 ) / ( (((5 * 3))) + 27 / 9 + 5)"
                , SimpleLookup, 200 * ((50 + 100 - 100 / 5) - (3 + 2 * 2) + 2) / ((((5 * 3))) + 27 / 9 + 5));
            Test(" 200* (( 50 + 100 - 100 / 5) - (3 + 2 * 2) + 2 )", SimpleLookup, 200 * ((50 + 100 - 100 / 5) - (3 + 2 * 2) + 2));
            Test("( (((5 * 3))) + 27 / 9 + 5)", SimpleLookup, ((((5 * 3))) + 27 / 9 + 5));
            Test("(4)*(4)", SimpleLookup, (4) * (4));
            Test("2*(2+2)/(1+1)+2 ", SimpleLookup, 2 * (2 + 2) / (1 + 1) + 2);


            //Testing exceptions
            Console.WriteLine("\nTesting Excemptions");
            Console.WriteLine("---------------------------");
            TestException("4+(2B2+4)", SimpleLookup);
            TestException("4B+(B2+4)", SimpleLookup);
            TestException("4+(-B2+B4)", SimpleLookup);
            TestException("4+(B2+-9))", SimpleLookup);
            TestException("4 + BB", SimpleLookup);
            TestException("(4)(4)", SimpleLookup);
            TestException("4 + B*2", SimpleLookup);
            TestException("4 ++++ 1", SimpleLookup);
            TestException("32.0 - 1", SimpleLookup);
            TestException("()()", SimpleLookup);
            TestException("(1+2, 1+2)", SimpleLookup);
            TestException("(1+2) / (1-1)", SimpleLookup);
            TestException("1+2) / 1+1)", SimpleLookup);
            TestException("im just a string", SimpleLookup);
            TestException("4B+(z2+4)", NoVariableLookup);
            TestException("6/I0", SimpleLookup);
            TestException("6", SimpleLookup);
            TestException("B2", SimpleLookup);
            TestException("-(6/6)", SimpleLookup);
            TestException("-6*6", SimpleLookup);
            TestException("-6", SimpleLookup);
            TestException("2*(2+2)/(1+1)+2) ", SimpleLookup);

            Console.WriteLine("\n" + (double)(4.0 * 3.0 - 8.0 / 2.0 + 4.0 * (8.0 - 9.0 * 2.0) / 14.0 * 1.0));
            //string pattern = ".024+sA_2";
            ////string testString = "4 + x 23";
            //////string[] testArr = StringSplit(testString);
            //Console.WriteLine(Regex.IsMatch(pattern, "^[A-Za-z_0-9.0-9()/*+-]+$"));
            ////Console.WriteLine(1e6);
            ////foreach (string s in testArr)
            ////{
            ////    Console.WriteLine(s);
            ////}
            ////Console.WriteLine(testArr.Length);
            ////Console.WriteLine(Regex.IsMatch(pattern, "^(([A-Za-z_]+)([A-Za-z_0-9]+))$"));

        }
        /// <summary> 
        /// Tests the Evaluator class by providing the param's. Inputs a string 
        /// and lookup delegate method. Outputs expected outcome. If evaluator
        /// solution is the expected output, then it will pass. Otherwise it will fail
        /// and provide the output of evaluator.
        /// 
        /// This test was made with help from Ella Moskun from 
        /// help session 08/27/2021.
        /// </summary>
        /// <param name="formula">string formula in the infix notation</param>
        /// <param name="lookup">lookup method delegate</param>
        /// <param name="expected">output that provides what Evaluator class should output</param>
        private static void Test(string formula, Lookup lookup, int expected)
        {
            int actual = Evaluate(formula, lookup);
            if (actual == expected)
                Console.WriteLine($"Pass: {formula} evlauated to {expected}");
            else
                Console.WriteLine($"Fail:{formula} evaluted to {actual} insted of {expected}");
        }

        /// <summary>
        /// Tests the Evaluator class by providing the parameters. Inputs a string
        /// infix expression. Outputs a result determined by if an exception was thrown or
        /// if one did not throw. All inputs should throw and inputs should be incorrect.
        /// For sake of space when outputting in the console, the exception
        /// 
        /// This test was made with help from Ella Moskun from 
        /// help session 08/27/2021.
        /// </summary>
        /// <param name="formula">string formula in the infix notation</param>
        /// <param name="lookup">lookup method delegate</param>
        private static void TestException(string formula, Lookup lookup)
        {
            try
            {
                Evaluate(formula, lookup);
                Console.WriteLine("Fail: " + formula + " did not throw");
            }
            catch (ArgumentException ax)
            {
                Console.WriteLine($"Pass: {formula} threw an argument exception: {ax.Message}");
            }
            catch (DivideByZeroException de)
            {
                Console.WriteLine($"Pass: {formula} threw a dividebyzero exception: {de.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail {formula} threw the wrong kind of exception: {ex.Message}");
            }
        }

        /// <summary>
        /// A method to lookup a variable and return a given number. If the variable does not have
        /// a uniqure integer than it will return 1 for any variable.
        /// </summary>
        /// <param name="v">String representing a variable token from the Evaluator class</param>
        /// <returns>An intger value</returns>
        private static int SimpleLookup(string v)
        {
            switch (v)
            {
                case "A1":
                    return 1;
                case "B2":
                    return 2;
                case "C3":
                    return 3;
                case "AB3":
                    return 4;
                case "I0":
                    return 0;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// A method to lookup a variable and return a given number. If the variable does not have
        /// a uniqure integer than it will throw an argument exception.
        /// </summary>
        /// <param name="v">String representing a variable token from the Evaluator class</param>
        /// <returns>An integer value</returns>
        private static int NoVariableLookup(string v)
        {
            switch (v)
            {
                case "A1":
                    return 1;
                case "B2":
                    return 2;
                case "C3":
                    return 3;
                case "AB3":
                    return 4;
                default:
                    throw new ArgumentException(); ;
            }
        }
    }
}
