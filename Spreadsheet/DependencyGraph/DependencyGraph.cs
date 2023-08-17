/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS2
 */

// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private readonly Dictionary<string, HashSet<string>> dependants;
        private readonly Dictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependants = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                int size = 0;
                foreach (KeyValuePair<string, HashSet<string>> entry in dependants)
                {
                    size += dependants[entry.Key].Count;
                }
                return size;
            }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// For anything not in the DependencyGraphy, no dependees will return.
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (!dependees.ContainsKey(s))
                {
                    return 0;
                }
                return dependees[s].Count;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return HasHelper(dependants, s);
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return HasHelper(dependees, s);
        }

        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            return GetHelper(dependants, s);
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            return GetHelper(dependees, s);
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            EmptyDependency(s, dependees);
            EmptyDependency(t, dependants);

            if (dependees.ContainsKey(t))
            {
                dependees[t].Add(s);
                if (dependants.ContainsKey(s))
                {
                    dependants[s].Add(t);
                }
                else
                {
                    AddDependencyHelper(t, s, dependants);
                }
            }
            else
            {
                AddDependencyHelper(s, t, dependees);
                if (dependants.ContainsKey(s))
                {
                    dependants[s].Add(t);
                }
                else
                {
                    AddDependencyHelper(t, s, dependants);
                }
            }
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependants.ContainsKey(t))
            {
                dependees[t].Remove(s);
                dependants[s].Remove(t);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            ReplaceHelper(s, newDependents, false);
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            ReplaceHelper(s, newDependees, true);
        }

        /// <summary>
        /// Removes ordered pairs in the form (r,s)/(s,r).Then, for each
        /// t in a DependencyGraphy, adds the ordered pair (s,t)/(t,s).
        /// <para>
        /// Helper method for ReplaceDependees(s)/Dependent(s)
        /// </para>
        /// </summary>
        /// <param name="s">value representing the dependees/dependent</param>
        /// <param name="newEnumerable">An enumerable that Inherits IEnumerable interface</param>
        /// <param name="dg1">A dependency graph</param>
        /// <param name="dg2">A dependency graph</param>
        private void ReplaceHelper(string s, IEnumerable<string> newEnumerable, bool depnOrDepne)
        {
            List<string> tempList;
            if (depnOrDepne)
            {
                tempList = GetDependees(s).ToList();
                foreach (string item in tempList)
                {
                    RemoveDependency(item, s);
                }
                foreach (string item in newEnumerable)
                {
                    AddDependency(item, s);
                }
            }
            else
            {
                tempList = GetDependents(s).ToList();
                foreach (string item in tempList)
                {
                    RemoveDependency(s, item);
                }
                foreach (string item in newEnumerable)
                {
                    AddDependency(s, item);
                }
            }
        }

        /// <summary>
        /// Reports whether dependees(s)/dependent(s) is non-empty.
        /// <para>
        /// Helper method for GetDependees(s)/Dependent(s)
        /// </para>
        /// </summary>
        /// <param name="dg">A dependency graph</param>
        /// <param name="s">value representing the dependees/dependent</param>
        /// <returns></returns>
        private IEnumerable<string> GetHelper(Dictionary<string, HashSet<string>> dg, string s)
        {
            HashSet<string> enumerator = new HashSet<string>();
            if (!dg.ContainsKey(s))
            {
                return enumerator;
            }
            if (dg[s].Count != 0)
            {
                foreach (string x in dg[s])
                {
                    enumerator.Add(x);
                }
            }
            return enumerator;
        }

        /// <summary>
        /// Checks if a value does not exists in a dependency graph 
        /// to balance dependent(s)/dependee(s).
        /// 
        /// <para>
        /// Helper method for AddDependency
        /// </para>
        /// </summary>
        /// <param name="s">value representing the dependees/dependent</param>
        /// <param name="dg">A dependency graph</param>
        private void EmptyDependency(string s, Dictionary<string, HashSet<string>> dg)
        {
            if (!dg.ContainsKey(s))
            {
                HashSet<string> tempHash = new HashSet<string>();
                dg.Add(s, tempHash);
            }
        }

        /// <summary>
        /// Checks if a depenency graph has a dependees/dependent and
        /// if not it will add a new ordered pair. 
        /// 
        /// <para>
        /// Helper method for AddDependency
        /// </para>
        /// </summary>
        /// <param name="s">value representing the dependees/dependent</param>
        /// <param name="t">value representing the dependees/dependent</param>
        /// <param name="dg">A dependency graph</param>
        private void AddDependencyHelper(string s, string t, Dictionary<string, HashSet<string>> dg)
        {
            HashSet<string> tempHash = new HashSet<string>
            {
                s
            };
            dg.Add(t, tempHash);
        }

        /// <summary>
        /// Checks a value if it has a dependent(s)/dependee(s)
        /// 
        /// <para>
        /// Helper method for HasDependent/Dependee
        /// </para>
        /// </summary>
        /// <param name="dg">A dependency graph</param>
        /// <param name="s">value representing the dependees/dependent</param>
        /// <returns></returns>
        private bool HasHelper(Dictionary<string, HashSet<string>> dg, string s)
        {
            if (!dg.ContainsKey(s))
            {
                return false;
            }
            return dg[s].Any(x => true);
        }
    }
}
