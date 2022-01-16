using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    public class KeyResultHelper
    {
        public static List<KeyResult> SplitKeyResults(Fields field)
        {
            List<KeyResult> keyResults = new();
            if (field.KeyResult1 != null)
            {
                string[] keyResultSplit = field.KeyResult1.Split("*");
                keyResults.AddRange(CreateKeyResults(keyResultSplit));
            }
            if (field.KeyResult2 != null)
            {
                string[] keyResultSplit = field.KeyResult2.Split("*");
                keyResults.AddRange(CreateKeyResults(keyResultSplit));
            }
            if (field.KeyResult3 != null)
            {
                string[] keyResultSplit = field.KeyResult3.Split("*");
                keyResults.AddRange(CreateKeyResults(keyResultSplit));
            }
            if (field.KeyResult4 != null)
            {
                string[] keyResultSplit = field.KeyResult4.Split("*");
                keyResults.AddRange(CreateKeyResults(keyResultSplit));
            }
            return keyResults;
        }

        private static List<KeyResult> CreateKeyResults(string[] keyResultSplit)
        {
            List<KeyResult> kRList = new List<KeyResult>();
            foreach (string keyResultAsText in keyResultSplit)
            {
                KeyResult kR = new ();
                string[] stringArray = keyResultAsText.Trim().Split("[");
                string KeysResultText = stringArray.First().Trim();
                kR.Text = KeysResultText;
                if (stringArray.Length > 1)
                {
                    string syntaxString = stringArray.Last()[0..^1];
                    string[] parameters = syntaxString.Split(";");
                    Boolean valuesSet = false;
                    foreach (string parameter in parameters)
                    {
                       valuesSet = SetValue(kR, parameter);
                    }
                    if (!valuesSet && parameters.Length == 3)
                    {
                        if(kR.RAG == null) { 
                        kR.RAG = new RAG(parameters[0]);
                        }
                        if(kR.Change == null)
                        {
                            kR.Change = parameters[1];
                        }
                        if (kR.Value == null)
                        {
                            kR.Value = parameters[2];
                        }
                    }
                }                
                if (kR.RAG == null)
                {
                    kR.RAG = new RAG();
                }
                kRList.Add(kR);
            }
            return kRList;
        }

        private static bool SetValue(KeyResult kR, string parameter)
        {
            if (parameter.ToLower().Contains("rag:"))
            {
                kR.RAG = new RAG(parameter["rag:".Length..].Trim());
                return true;
            }
            else if (parameter.ToLower().Contains("change:"))
            {
                kR.Change = parameter["change:".Length..].Trim();
                return true;
            }
            else if (parameter.ToLower().Contains("value:"))
            {
                kR.Value = parameter["value:".Length..].Trim();
                return true;
            }
            return false;
        }
    }
}
