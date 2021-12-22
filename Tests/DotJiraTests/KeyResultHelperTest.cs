
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    class KeyResultHelperTest
    {        
        [Test]
        public void KeyResultsReturnAList()
        {
            Fields fields = new Fields();
            fields.KeyResult1 = "";
            List<KeyResult> keyResults = fields.SplitKeyResults();
            Assert.NotNull(keyResults);
        }

        [Test]
        public void KeyResultsParametersAreSet()
        {
            /*
             * SYNTAX
             ** [RAG;change;value]
             *** RAG - options:
             **** Red
             **** Amber
             **** Green
             *
             *** change - options:
             **** + :: improvement 
             **** 0 :: no change
             **** - :: deterioration
             *
             *** value:
             **** Write the actual value
             */
            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[{0};{1};{2}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax ;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);
            Assert.AreEqual(rAG, firstKeyResults.RAG.Value.Substring(0,1));
            Assert.AreEqual(change, firstKeyResults.Change);
            Assert.AreEqual(value, firstKeyResults.Value);
        }

        [Test]
        public void KeyResultsParametersOnlyPartiallySetRagNotSet()
        {
            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[change:{1};value:{2}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);            
            Assert.AreEqual(change, firstKeyResults.Change);
            Assert.AreEqual(value, firstKeyResults.Value);
        }
        [Test]
        public void KeyResultsParametersOnlyPartiallySetChangeNotSet()
        {
            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[rag: {0};value:{2}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);
            Assert.AreEqual(rAG, firstKeyResults.RAG.Value.Substring(0,1));
            Assert.AreEqual(value, firstKeyResults.Value);
        }
        [Test]
        public void KeyResultsParametersOnlyPartiallySetValueNotSet()
        {

            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[RAG:{0};change:{1}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);
            Assert.AreEqual(rAG, firstKeyResults.RAG.Value.Substring(0,1));
            Assert.AreEqual(change, firstKeyResults.Change);            
        }

        [Test]
        public void KeyResultsParametersOnlyPartiallySetOnlyRAGSet()
        {
            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[rag:{0}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);
            Assert.AreEqual(rAG, firstKeyResults.RAG.Value.Substring(0,1));            
        }
        [Test]
        public void KeyResultsParametersOnlyPartiallySetOnlyChangeSet()
        {

            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[change:{1}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);            
            Assert.AreEqual(change, firstKeyResults.Change);            
        }
        [Test]
        public void KeyResultsParametersOnlyPartiallySetOnlyValueSet()
        {
            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[value:{2}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);            
            Assert.AreEqual(value, firstKeyResults.Value);
        }

        [Test]
        public void AllParametersAreSetWithNameInWrongOrder()
        {
            string rAG = "r";
            string change = "+";
            string value = "35%";
            String keyResultSyntax = String.Format("[rag:{0};value:{2};change:{1}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);
            Assert.AreEqual(rAG, firstKeyResults.RAG.Value.Substring(0, 1));
            Assert.AreEqual(value, firstKeyResults.Value);
            Assert.AreEqual(change, firstKeyResults.Change);
        }

        [Test]
        public void AllParametersInvalidParameterUsed()
        {
            string rAG = "r";
            string change = "+";
            string value = "lars:35%";
            String keyResultSyntax = String.Format("[rag:{0};change:{1};{2}]", rAG, change, value);
            Fields fields = new();
            String krText = "I am a KeyResult   ";
            fields.KeyResult1 = krText + keyResultSyntax;
            List<KeyResult> keyResults = fields.SplitKeyResults();
            KeyResult firstKeyResults = keyResults.First();

            Assert.AreEqual(krText.Trim(), firstKeyResults.Text);
            Assert.AreEqual(rAG, firstKeyResults.RAG.Value.Substring(0, 1));            
            Assert.AreEqual(change, firstKeyResults.Change);
            Assert.AreEqual(value, firstKeyResults.Value);
        }
    }
}
