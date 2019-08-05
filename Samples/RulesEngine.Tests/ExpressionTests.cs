using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RulesEngine.Tests
{
    class sample
    {
        public sample()
        {
            this.List = new List<string>();
        }
        public string Message { get; set; }
        public List<string> List { get; private set; }
        public bool Flag { get; set; }
        public int Metric { get; set; }
    }

    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void CollectionTest()
        {
            //Check that "Hello World!!" is one of the elements on List
            RulesEngine.Rule r = new Rule();
            r.Member = "List";
            r.Operation = "Contains";
            r.Target = "Hello World!!";

            sample s = new sample();
            s.List.Add("Hello World!!");

            Engine<sample> engine = new Engine<sample>();
            engine.Evaluate(r, s);

        }

        [TestMethod]
        public void UnaryOperatorTest()
        {
            //Check that Flag is False
            RulesEngine.Rule r = new Rule();
            r.Member = "Flag";
            r.Operation = "Not";

            sample s = new sample();
            s.Flag = false;

            Engine<sample> engine = new Engine<sample>();
            engine.Evaluate(r, s);

        }

        [TestMethod]
        public void BinaryOperatorTest()
        {
            //Check that Metric is greater than 5
            RulesEngine.Rule r = new Rule();
            r.Member = "Metric";
            r.Operation = "GreaterThan";
            r.Target = 5;

            sample s = new sample();
            s.Metric = 10;

            Engine<sample> engine = new Engine<sample>();
            engine.Evaluate(r, s);


        }

        [TestMethod]
        public void InvokeMethodTest()
        {
            //Check that Message starts with "Hello"
            RulesEngine.Rule r = new Rule();
            r.Member = "Message";
            r.Operation = "StartsWith";
            r.Target = "Hello";

            sample s = new sample();
            s.Message = "Hello World!!";

            Engine<sample> engine = new Engine<sample>();
            engine.Evaluate(r, s);


        }


        [TestMethod]
        public void ConstraintTest()
        {
            //If Flag is <False>, Message must start with "Hello1"
            RulesEngine.Rule r1 = new Rule();
            r1.Member = "Message";
            r1.Operation = "StartsWith";
            r1.Target = "Hello1";

            RulesEngine.Rule r0 = new Rule();
            r0.Member = "Flag";
            r0.Operation = "Equals";
            r0.Target = false;
            r0.Constraint = r1;

            sample s = new sample();
            s.Message = "Hello World!!";
            s.Flag = true;

            Engine<sample> engine = new Engine<sample>();
            engine.Evaluate(r0, s);


        }
    }
}
