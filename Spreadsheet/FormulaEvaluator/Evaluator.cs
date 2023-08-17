/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS1
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// A class that evaluates a given infix expression and outputs the result.
    /// Can have a vailid variable during computation and express it throught the 
    /// equation.
    /// </summary>
    public static class Evaluator
    {
        private static string[] tokens;
        private static Stack<string> operatorStack;
        private static Stack<int> valueStack;

        /// <summary>
        /// Delegate to lookup variable values
        /// </summary>
        /// <param name="v">The name of the variable</param>
        /// <returns> The value of the named variable, it it exists, 
        /// otherwise throws an ArgumentException</returns>
        public delegate int Lookup(String v);

        /// <summary>
        /// Evaluates an infix formula expression
        /// 
        /// Throws an Argument exception if exp cannot be evaluated due to bad format, 
        /// division by zero, nonexistant variables, incorrect variables, or unary negatives.
        /// 
        /// O(n) time complexity.
        /// </summary>
        /// <param name="exp">An infix formula represented as a string, ...</param>
        /// <param name="variableEvaluator">A method to use to lookup variable values</param>
        /// <returns>The interger result of evaluation</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            operatorStack = new Stack<string>();
            valueStack = new Stack<int>();

            //Formatting and runnings test exemptions before algorithim.
            tokens = StringSplit(exp);
            FormatCheck(tokens);

            //Iterating through all the tokens that are received and processing algorithim  
            int counter = 0;
            foreach (string token in tokens)
            {
                counter++;
                //Token is a left parenthesis
                if (token == "(")
                {
                    operatorStack.Push(token);
                }

                //Token is a right parenthesis
                RightParanthesisOperation(token);

                //Token is an integer
                IntegerTokenOperation(token);

                //Token is a variable
                if (token != "-" && token != "+" && token != "/" && token != "*" && token != "("
                    && token != ")" && int.TryParse(token, out int x) == false)
                {
                    if (IsVariable(token) == true)
                    {
                        IntegerTokenOperation(variableEvaluator(token).ToString());
                    }
                }


                //Token is an operator
                if (token == "-" || token == "+" || token == "/" || token == "*")
                {
                    if (token == "*" || token == "/")
                    {
                        operatorStack.Push(token);
                    }

                    if (token == "-" || token == "+")
                    {
                        AddSubOperation();
                        operatorStack.Push(token);
                    }
                }

                //If there are still operends in the stack after any multipication or division
                //computes the operends to complete the eq. Follows order of operations.
                if (counter == tokens.Length)
                {
                    AddSubOperation();
                }
            }
            return valueStack.Pop();
        }

        /// <summary>
        /// Recives a token that is a right parenthesis string. Follows operations 
        /// to ensure order of operations are handled corretly. Checks the top of the stack
        /// for different case scenerios and processes the algorthim based on said checks.
        /// 
        /// Helper method for Evaluate method.
        /// </summary>
        /// <param name="token">Represents a string right parenthesis</param>
        private static void RightParanthesisOperation(string token)
        {
            if (token == ")")
            {
                if (operatorStack.IsOnTop("+"))
                {
                    valueStack.Push((int)PushResult(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                    operatorStack.Pop();
                    MultiDivdOperation();
                }
                else if (operatorStack.IsOnTop("-"))
                {
                    valueStack.Push((int)PushResult(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                    operatorStack.Pop();
                    MultiDivdOperation();
                }
                else if (operatorStack.IsOnTop("("))
                {
                    operatorStack.Pop();
                    MultiDivdOperation();
                }
            }
        }

        /// <summary>
        /// Represents a multipication and division operation to be used
        /// when the checks are valid. Distinguishes between both operations.
        /// 
        /// Helper method for Evaluate method.
        /// </summary>
        private static void MultiDivdOperation()
        {
            if (operatorStack.Count != 0)
            {
                if (operatorStack.IsOnTop("*"))
                {
                    valueStack.Push((int)PushResult(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                }
                else if (operatorStack.IsOnTop("/"))
                {
                    valueStack.Push((int)PushResult(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                }
            }
        }

        /// <summary>
        /// Represents an addition and subtraction operation to be used
        /// when the checks are valid. Distinguishes between both operations.
        /// Recives a string of + or -.
        /// 
        /// Helper method for Evaluate method.
        /// </summary>
        /// <param name="op">Represents a string operator token from an infix expression</param>
        private static void AddSubOperation()
        {
            if (operatorStack.Count != 0)
            {
                if (operatorStack.IsOnTop("+"))
                {
                    valueStack.Push((int)PushResult(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                }
                else if (operatorStack.IsOnTop("-"))
                {
                    valueStack.Push((int)PushResult(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                }
            }
        }

        /// <summary>
        /// Checks if the token is an integer. Then determines which operation to use
        /// depending on the top of the operator stack. This follows the order of 
        /// operations.
        /// 
        /// Helper method to Evaluate method.
        /// </summary>
        /// <param name="token">Represents a string token that results in an integer</param>
        private static void IntegerTokenOperation(string token)
        {
            if (int.TryParse(token, out int x) == true)
            {
                if (operatorStack.Count != 0)
                {
                    if (operatorStack.IsOnTop("*") || operatorStack.IsOnTop("/"))
                    {
                        if (operatorStack.IsOnTop("*"))
                        {
                            valueStack.Push((int)PushResult((int.Parse(token)), valueStack.Pop(), operatorStack.Pop()));
                        }
                        else
                        {
                            valueStack.Push((int)PushResult((int.Parse(token)), valueStack.Pop(), operatorStack.Pop()));
                        }
                    }
                    else
                    {
                        valueStack.Push(int.Parse(token));
                    }
                }
                else
                {
                    valueStack.Push(int.Parse(token));
                }
            }
        }

        /// <summary>
        /// Preforms operations based on the operator it recieves
        /// Recives an operator of type string and two integers that
        /// represent the two needed operations.
        /// </summary>
        /// <param name="x">Represents a operand</param>
        /// <param name="y">Represents a operand</param>
        /// <param name="c">Represents a operator</param>
        /// <returns></returns>
        private static object PushResult(int x, int y, string c)
        {
            switch (c)
            {
                case "-":
                    return (int)(y - x);
                case "*":
                    return (int)(y * x);
                case "/":
                    if (x == 0)
                    {
                        throw new ArgumentException("You cannot divide by zero or your " +
                            "formula resulted in a division by zero.");
                    }
                    return (int)(y / x);
                default:
                    return (int)(y + x);
            }
        }

        /// <summary>
        /// Receives a string and transforms it into a array of type string.
        /// Deletes any whitespace to preform operations with better accuarcy.
        /// Splits each individual string into its own element and keeps the original
        /// format the string had. Checks if the string contains valid tokens for the infix
        /// notation using LegalTokenCheck method.
        /// </summary>
        /// <param name="exp">Represents a string of any kind</param>
        /// <returns></returns>
        private static string[] StringSplit(string exp)
        {
            exp = Regex.Replace(exp, @"\s", string.Empty);
            tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            tokens = tokens.Where(x => x != string.Empty).ToArray();

            return tokens;
        }

        /// <summary>
        /// Receives a string that is not one of the accepted strings for the algorithim 
        /// after all checks for invalid tokens. Checks if the variable is alphabetical  
        /// lower/uppercase followed by integers. If it is not valid an exception is thrown.
        /// Returns a boolean of true if checks are passed. Checks if a varible is a number that 
        /// is not an integer.
        /// </summary>
        /// <param name="token">Represents a variable token of type string</param>
        /// <returns></returns>
        private static bool IsVariable(string token)
        {
            if (token != "-" && token != "+" && token != "/" && token != "*" && token != "("
                && token != ")" && int.TryParse(token, out int x) == false)
            {
                return Regex.IsMatch(token, "^(([A-Za-z]+)([0-9]+))$");
            }
            return false;
            
        }

        /// <summary>
        /// Receives a array of type string and checks its format for an infix expression.
        /// Checks all individual tokens and processes a simple algorithm to determine if it
        /// is in infix notation. Also checks if any unaray operators are in the expression.
        /// Throws an exception if it is found fail any of the checks. 
        /// O(n) time complexity.
        /// </summary>
        /// <param name="tokens">represents a string array of an expression</param>
        private static void FormatCheck(string[] tokens)
        {
            int parenRightCounter = 0;
            int parenLeftCounter = 0;
            int parenTot;
            int counter = 0;
            string tempToken = string.Empty;

            if (tokens.Length == 0) 
            {
                throw new ArgumentException();
            }

            foreach (string token in tokens)
            {
                if (token == "(")
                {
                    parenRightCounter++;
                }
                else if (token == ")")
                {
                    parenLeftCounter++;
                }

                if (!Regex.IsMatch(token, "^[A-Za-z0-9()/*+-]+$"))
                {
                    throw new ArgumentException("You must use only legal tokens of +,-,*,/,(,), " +
                    "or alphanumeric characters.");
                }
                if (counter == 0)
                {
                    if (int.TryParse(token, out int a) || IsVariable(token) || token == "(")
                    { }
                    else 
                    {
                        throw new ArgumentException("A test for me!!!!");
                    }
                }

                if (counter != 0 && (tempToken == "(" || tempToken == "+" || tempToken == "-" || tempToken == "/"
                      || tempToken == "*"))
                {
                    if (int.TryParse(token, out int y) || IsVariable(token) || token == "(")
                    { }
                    else
                    {
                        throw new ArgumentException("A test for me!!!!");
                    }
                }
                if (counter != 0 && (int.TryParse(tempToken, out int x) || IsVariable(tempToken) || tempToken == ")"))
                {
                    if (token == ")" || token == "+" || token == "-" || token == "/"
                    || token == "*")
                    { }
                    else
                    {
                        throw new ArgumentException("extra following rule");
                    }
                }

                counter++;
                if (counter == tokens.Length)
                {
                    if (int.TryParse(token, out int b) || IsVariable(token) || token == ")")
                    { }
                    else
                    {
                        throw new ArgumentException("A test for me!!!!");
                    }
                }
                tempToken = token;
            }
            parenTot = parenLeftCounter + parenRightCounter;

            //If parenthesis is not modulo 0, then it means you have an extra parenthesis.
            if (parenTot % 2 != 0)
            {
                InvalidFormatThrow();
            }
            //Checking the individual left and righ parenthesis to catch errors if they are not balanced
            if (parenLeftCounter < parenRightCounter || parenRightCounter < parenLeftCounter)
            {
                InvalidFormatThrow();
            }

        }

        /// <summary>
        /// Method to provide an argument exception for FormatCheck with
        /// the necessary message. 
        /// </summary>
        private static void InvalidFormatThrow()
        {
            throw new ArgumentException("The input is an invalid format or you are using a unaray operator. " +
                 "Please follow correct infix notation and/or not use a unary operator.");
        }
    }

    /// <summary>
    /// Class that provides extensions to the Stack Collections.Stack 
    /// and be used in this solution.
    /// </summary>
    static class StackExtensions
    {
        /// <summary>
        /// Creates a simple method that takes a string and checks if it is
        /// at the top of the stack. 
        /// </summary>
        /// <param name="s">string representing the stack</param>
        /// <param name="x">representing a token operator</param>
        /// <returns></returns>
        public static bool IsOnTop(this Stack<string> s, string x)
        {
            return s.Peek() == x;
        }
    }
}
