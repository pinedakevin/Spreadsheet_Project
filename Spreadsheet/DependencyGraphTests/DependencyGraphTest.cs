/*
 * Kevin Pineda
 * u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS2 Test
 */
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        /// In an ordered pair of only (s,t), s should have dependees 
        /// and t should have none;
        /// </summary>
        [TestMethod]
        public void HasDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "d");
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependees("d"));
        }

        /// <summary>
        /// In an ordered pair of only (s,t), t should have dependents 
        /// and s should have none;
        /// </summary>
        [TestMethod]
        public void HasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "d");
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependents("d"));
        }

        /// <summary>
        /// Checks for any empty dependent/dependee that exists in the dependency graph
        /// </summary>
        [TestMethod]
        public void EmptyGetTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "b");
            t.AddDependency("c", "a");
            t.AddDependency("b", "x");
            t.AddDependency("k", "a");
            t.AddDependency("k", "f");

            IEnumerator<string> e = t.GetDependents("f").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependents("x").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("k").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Checks the indexer to give the amount of dependees for any
        /// ordered pair (s,t). Should return the correct amount of dependees.
        /// In an ordered pair of only (s,t)
        /// <para> dg[s] = 1 </para>
        /// dg[t] = 0
        /// </summary>
        [TestMethod]
        public void IndexerTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "a");
            t.AddDependency("c", "b");
            t.AddDependency("d", "b");
            t.AddDependency("e", "b");
            t.AddDependency("b", "c");
            t.AddDependency("b", "d");
            t.AddDependency("b", "e");
            t.AddDependency("b", "f");
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.AddDependency("a", "e");
            t.AddDependency("a", "f");
            t.AddDependency("a", "g");
            Assert.AreEqual(14, t.Size);
            Assert.AreEqual(4, t["b"]);
            t.RemoveDependency("a", "b");
            Assert.AreEqual(13, t.Size);
            Assert.AreEqual(3, t["b"]);
            Assert.AreEqual(1, t["a"]);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }

        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            Console.WriteLine("test");
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        /// <summary>
        /// If it Does Not Exists than it should return false for 
        /// HasDependents.
        /// </summary>
        [TestMethod]
        public void HasDependentsDNETest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("z"));
        }

        /// <summary>
        /// If it Does Not Exists than it should return false for 
        /// HasDependets.
        /// </summary>
        [TestMethod]
        public void HasDependeesDNETest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("y"));
        }

        /// <summary>
        /// If a ordered piar Does Not Exists than it should 
        /// return false for RemoveDependency. 
        /// </summary>
        [TestMethod]
        public void DependentDNETest()
        {
                DependencyGraph t = new DependencyGraph();
                t.AddDependency("a", "b");
                t.AddDependency("b", "d");
                t.RemoveDependency("d", "c");
        }

        /// <summary>
        /// If it Does Not Exists than it should give
        /// an empty enumerator.
        /// </summary>
        [TestMethod]
        public void EnumeratorDNETest()
        {
            DependencyGraph t = new DependencyGraph();
            IEnumerator<string> e = t.GetDependents("f").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependents("azt").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// If it Does Not Exists than it should give
        /// not dependees when using indexer
        /// </summary>
        [TestMethod]
        public void IndexerDNETest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            Assert.AreEqual(0, t["c"]);
        }

        /// <summary>
        /// If a ordered piar Does Not Exists than it should 
        /// do nothing in ReplaceDependets/Dependees and return the 
        /// correct size.
        /// </summary>
        [TestMethod]
        public void ReplaceDNEMethods()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("b", "a");
            t.ReplaceDependents("b", new HashSet<string>());
            t.ReplaceDependees("b", new HashSet<string>());
            t.ReplaceDependents("a", new HashSet<string>());
            t.ReplaceDependees("a", new HashSet<string>());
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("x", new HashSet<string>());
            t.ReplaceDependees("x", new HashSet<string>() { "c"});
            t.ReplaceDependees("v", new HashSet<string>() { "c" });
            t.AddDependency("o", "b");

            Assert.AreEqual(3, t.Size);
        }
    }
}