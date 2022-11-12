using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Translator
{
    internal class Parser
    {
        public List<string> lines;
        public int lineNum;
        public string currentLine => lines[lineNum];

        public Parser(string filePath)
        {
            lines = new List<string>(File.ReadLines(filePath));

            //self.lines = f.read().split('\n')
            //f.close()

            lineNum = -1;
            advance();
        }


        public bool hasMoreCommands() => lineNum < lines.Count;

        public void advance()
        {
            if (hasMoreCommands())
                lineNum++;
            //if self.hasMoreCommands():
            //self.currentLine = self.lines[self.lineNum]
        }


        public string commandType()
        {
            //return an id string based on the split current line [0] item
            string cmd = currentLine.Split()[0];
            if (Regex.IsMatch(cmd, "^//.*"))
                return "comment";
            else
                return cmd;
        }

        public string arg1()
        {
            // split current line and return [1] item
            var a = currentLine.Split();
            if (a.Length > 1)
                return a[1];
            else
                return "error";
        }

        public string arg2()
        {
            // split current line and return [2] item
            var a = currentLine.Split();
            if (a.Length > 2)
                return a[2];
            else
                return "error";
        }


    }
}
