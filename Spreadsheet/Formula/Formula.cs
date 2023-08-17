// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
//
// API Implemented by Cristian Tapiero for CS3500, September 2021


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        // Member variable to access the tokens after being parsed in the constructor
        /// </summary>
        private List<string> normalizedTokens;

        /// <summary>
        /// Member variable that contains the tokens in string form
        /// </summary>
        private string stringForm;

        /// <summary>
        /// set of normalized variables, no allowed repetitions
        /// </summary>
        private HashSet<string> normalizedVars;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {

        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            string varPattern = "^[a-zA-Z_][a-zA-Z0-9_]*";

            IEnumerable<string> splitTokens = GetTokens(formula);
            normalizedTokens = new List<string>();
            normalizedVars = new HashSet<string>();

            int openingParen = 0;
            int closingParen = 0;
            int count = 0;
            string prevToken = "";
            StringBuilder sb = new StringBuilder();
            foreach (string token in splitTokens)
            {
                if (token == "(") openingParen++;
                if (token == ")") closingParen++;

                if (StartTokenRule(varPattern, count, token))
                {
                    throw new FormulaFormatException("incorrect way to start a formula");
                }

                if (ParenOpFollowingRule(varPattern, prevToken, token))
                {
                    throw new FormulaFormatException("variable, number or opening parenthesis is missing, make sure you add them");
                }

                if (ExtraFollowingRule(varPattern, prevToken, token))
                {
                    throw new FormulaFormatException("value after a number, a variable, or a closing parenthesis" +
                                                           " must be either an operator or a closing parenthesis.");
                }

                if (EndTokenRule(varPattern, splitTokens, count, token))
                {
                    throw new FormulaFormatException("incorrect way to end a formula, check your last value");
                }
                //normalizing variables
                if (Regex.IsMatch(token, varPattern))
                {
                    string normalizedVar = normalize(token);

                    if (Regex.IsMatch(normalizedVar, varPattern) && isValid(normalizedVar))
                    {
                        sb.Append(normalizedVar);
                        normalizedTokens.Add(normalizedVar);
                        normalizedVars.Add(normalizedVar);
                        prevToken = token;
                        count++;

                        if (count == normalizedTokens.Count())
                        {
                            stringForm = sb.ToString();
                        }
                        continue;
                    }

                    if (!(Regex.IsMatch(normalizedVar, varPattern)))
                    {
                        throw new FormulaFormatException("expression is not valid after being normalized");
                    }

                    if (!isValid(normalizedVar))
                    {
                        throw new FormulaFormatException("expression is not valid according to extra restrictions of validity");
                    }
                }
                // Normalizing numbers
                if (double.TryParse(token, out double tokenNumber))
                {
                    string parsedNumber = tokenNumber.ToString();
                    sb.Append(parsedNumber);
                    normalizedTokens.Add(parsedNumber);
                    prevToken = token;
                    count++;
                    if (count == normalizedTokens.Count())
                    {
                        stringForm = sb.ToString();
                    }
                    continue;
                }

                sb.Append(token);
                normalizedTokens.Add(token);
                prevToken = token;
                count++;

                if (count == normalizedTokens.Count())
                {
                    stringForm = sb.ToString();
                }
            }

            if (count == 0)
            {
                throw new FormulaFormatException("Formula needs to contain at least one token");
            }

            if (closingParen > openingParen)
            {
                throw new FormulaFormatException("Formula is missing an opening parenthesis");
            }

            if (openingParen != closingParen)
            {
                throw new FormulaFormatException("Formula is missing a closing parenthesis");
            }
        }
        /// <summary>
        /// Checks if first token of expression starts correctly
        /// </summary>
        /// <param name="varPattern">string pattern to compare variable</param>
        /// <param name="count">iteration number</param>
        /// <param name="token">token being checked for correctness</param>
        /// <returns></returns>
        private static bool StartTokenRule(string varPattern, int count, string token)
        {
            return count == 0 && !(Double.TryParse(token, out double startNumber)
                             || Regex.IsMatch(token, varPattern) || token == "(");

        }
        /// <summary>
        /// Checks if last token of expression starts correctly
        /// </summary>
        /// <param name="varPattern">string pattern to compare variable</param>
        /// <param name="splitTokens">IEnumerable to control iteration</param>
        /// <param name="count">iteration number</param>
        /// <param name="token">token being checked for correctness</param>
        /// <returns></returns>
        private static bool EndTokenRule(string varPattern, IEnumerable<string> splitTokens, int count, string token)
        {
            return count == splitTokens.Count() - 1 && (!(Double.TryParse(token, out double endNumber)
                                                 || Regex.IsMatch(token, varPattern) || token == ")"));
        }
        /// <summary>
        /// Checks if Any token that immediately follows an opening parenthesis or an operator is 
        /// either a number, a variable, or an opening parenthesis.
        /// </summary>
        /// <param name="varPattern">string pattern to compare variable</param>
        /// <param name="prevToken">previous token</param>
        /// <param name="token">token being checked for correctness</param>
        /// <returns></returns>
        private static bool ParenOpFollowingRule(string varPattern, string prevToken, string token)
        {
            return ((prevToken == "(" || IsOperator(prevToken)) && (!(Double.TryParse(token, out double Number)
                                                         || Regex.IsMatch(token, varPattern) || token == "(")));
        }
        /// <summary>
        /// Checks if Any token that immediately follows a number, a variable, or a closing parenthesis is
        /// either an operator or a closing parenthesis.
        /// </summary>
        /// <param name="varPattern">string pattern to compare variable</param>
        /// <param name="prevToken">previous token</param>
        /// <param name="token">token being checked for correctness</param>
        /// <returns></returns>
        private static bool ExtraFollowingRule(string varPattern, string prevToken, string token)
        {
            return (((Double.TryParse(prevToken, out double otherNumber) || Regex.IsMatch(prevToken, varPattern)
                                                || prevToken == ")")) && (!(token == ")" || IsOperator(token))));
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> valueStack = new Stack<double>();
            Stack<string> opStack = new Stack<string>();
            try
            {
                foreach (string t in normalizedTokens)
                {
                    string token = t.Trim();

                    if (Double.TryParse(token, out double Number))
                    {
                        DoubleOperation(valueStack, opStack, Number);
                    }

                    else if (Regex.IsMatch(token, "^[a-zA-Z]+[0-9]+$"))
                    {
                        DoubleOperation(valueStack, opStack, (double)lookup(token));
                    }


                    else if (token.Contains("+") || token.Contains("-"))
                    {
                        if (valueStack.Count >= 2 && (opStack.IsOnTop("+") || opStack.IsOnTop("-")))
                        {
                            valueStack.Push(popValueStackTwice(valueStack, opStack));
                        }
                        opStack.Push(token);
                    }

                    else if (token.Contains("*") || token.Contains("/") || token.Contains("("))
                    {
                        opStack.Push(token);
                    }

                    else if (token.Contains(")"))
                    {
                        if (valueStack.Count >= 2 && (opStack.IsOnTop("+") || opStack.IsOnTop("-")))
                        {
                            valueStack.Push(popValueStackTwice(valueStack, opStack));
                        }

                        if (opStack.IsOnTop("("))
                        {
                            opStack.Pop();
                        }

                        if (valueStack.Count() >= 2 && (opStack.IsOnTop("*") || opStack.IsOnTop("/")))
                        {
                            valueStack.Push(popValueStackTwice(valueStack, opStack));
                        }
                    }
                }

                // last token has been processed

                if (opStack.Count == 0 && valueStack.Count == 1)
                {
                    return valueStack.Pop();
                }
                else
                {
                    return popValueStackTwice(valueStack, opStack);
                }
            }
            catch (DivideByZeroException)
            {
                return new FormulaError("Division by zero is not allowed");
            }
            catch (ArgumentException)
            {
                return new FormulaError("Lookup variable does not exist");
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return normalizedVars;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return stringForm;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Formula f && f.ToString() == ToString();
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null))
            {
                return ReferenceEquals(f2, null);
            }
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return stringForm.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
        /// <summary>
        /// Helper method that checks if token is an operator
        /// </summary>
        /// <param name="op">token to check</param>
        /// <returns>true if the token is an operator</returns>
        private static bool IsOperator(string op)
        {
            switch (op)
            {
                case "+": return true;
                case "-": return true;
                case "*": return true;
                case "/": return true;
                default: return false;
            }
        }

        /// <summary>
        /// Pops the value stack twice, pops the operation stack once and performes the respective operation
        /// </summary>
        /// <param name="valueStack">the value stack to pop twice</param>
        /// <param name="opStack">the operator stack to pop</param>
        /// <returns>the result of the operation</returns>
        private static double popValueStackTwice(Stack<double> valueStack, Stack<string> opStack)
        {
            double value1 = valueStack.Pop();
            double value2 = valueStack.Pop();
            string operation = opStack.Pop();
            double performedOperation = PerformOperation(operation, value2, value1);
            return performedOperation;
        }

        /// <summary>
        /// Performs the right algorithm when token is an integer
        /// </summary>
        /// <param name="value">value stack to pop</param>
        /// <param name="operatorSign">operator stack to pop</param>
        /// <param name="token">token that is part of the operation</param>
        private static void DoubleOperation(Stack<double> value, Stack<string> operatorSign, double token)
        {
            if (operatorSign.IsOnTop("*") || operatorSign.IsOnTop("/"))
            {
                double stackValue = value.Pop();
                string operation = operatorSign.Pop();
                value.Push(PerformOperation(operation, stackValue, token));
            }
            else
            {
                value.Push(token);
            }
        }

        /// <summary>
        /// Recieves two values and an operator an performs the right operation
        /// </summary>
        /// <param name="operation">operator sign</param>
        /// <param name="value1">first value to be used in operation</param>
        /// <param name="value2">second value to be used in operation</param>
        /// <returns></returns>
        private static double PerformOperation(string operation, double value1, double value2)
        {
            if (operation == "+") return value1 + value2;
            if (operation == "-") return value1 - value2;
            if (operation == "*") return value1 * value2;
            if (value2 == 0) throw new DivideByZeroException();
            return value1 / value2;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    public static class PS3Extensions
    {
        /// <summary>
        /// Checks if value on top of a Stack is present
        /// </summary>
        /// <param name="s">The Stack to check</param>
        /// <param name="c">The value to be checked</param>
        /// <returns></returns>
        public static bool IsOnTop(this Stack<string> s, string c)
        {
            return s.Count > 0 && s.Peek() == c;
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}