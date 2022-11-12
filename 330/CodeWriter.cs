using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Translator
{
    internal class CodeWriter
    {
        private static readonly int segStackP = 0;
        private static readonly int segLcl = 1;
        private static readonly int segArg = 2;
        private static readonly int segPoint = 3;
        private static readonly int segTemp = 5;
        private static readonly int segStatic = 16;
        private static readonly int segStack = 256; //-2047

        public static readonly Dictionary<string, string> pushDict = new Dictionary<string, string>{
        {"default"  , "//--push {memSeg} {loc}--\n//get {memSeg} {loc}\n@{loc}\nD=A\n@{addrLoc}\nA=D+M\nD=M\n//push to stack\n@0\nA=M\nM=D\n\n" },
        {"constant" , "//--push constant {loc}--\n//get constant {loc}\n@{addr}\nD=A\n//push to stack\n@0\nA=M\nM=D\n\n"},
        {"pointer"  , "//--push pointer {loc}\n@{addr}\nD=M\n//push to stack\n@0\nA=M\nM=D\n"},
        { "temp"    , "//--push temp {loc}\n@{addr}\nD=M\n//push to stack\n@0\nA=M\nM=D\n\n"}
        };
        public static readonly Dictionary<string, string> popDict = new Dictionary<string, string>{
        {"default"   , "//--pop {memSeg} {loc}--\n//get addr {memSeg} {loc}\n@{loc}\nD=A\n@{addrLoc}\nD=D+M\n//save location to gen reg \n@13\nM=D\n//get value from stack, accounting f|| the moved pointer\n@0\nA=M\nD=M\n//move back to st||ed value\n@13\nA=M\nM=D\n//clear stack value\n@0\nA=M\nM=0\n//clear gen reg value\n@13\nM=0\n\n"},
        {"pointer"   , "//--pop pointer {loc}--\n//get addr pointer {loc}\n@{addrLoc}\nD=A\n//save location to gen reg\n@13\nM=D\n//get value from stack, accounting f|| the moved pointer\n@0\nA=M\nD=M\n//move back to st||ed value\n@13\nA=M\nM=D\n//clear stack value\n@0\nA=M\nM=0\n//clear gen reg value\n@13\nM=0\n" },
        {"temp"      , "//--pop temp {loc}--\n//get addr temp {loc}\n@{addr}\nD=A\n//save location to gen reg\n@13\nM=D\n//get value from stack, accounting f|| the moved pointer\n@0\nA=M\nD=M\n//move back to st||ed value\n@13\nA=M\nM=D\n//clear stack value\n@0\nA=M\nM=0\n//clear gen reg value\n@13\nM=0\n"},
        };
        public static readonly Dictionary<string, string> arithDic = new Dictionary<string, string>{
        {"add"   , "//--add--\n//decr sp\n@0\nM=M-1\n//go to stack and save to reg and clear\nA=M\nD=M\nM=0\n//go to lower stack location and add to it\n@0\nA=M-1\nM=M+D\n" },
        {"sub"   , "//--sub--\n//dec sp\n@0\nM=M-1\n//go to stack and save to reg and clear\nA=M\nD=M\nM=0\n//go to lower stack addr and sub to it\n@0\nA=M-1\nM=M-D\n"},
        {"neg"   , "//--neg--\n@0\n//load 0 into reg\nD=A\n//go to stack addr\nA=M-1\n//sub 0 from the value in mem\nM=D-M\n"},
        {"eq"    , "//--eq--\n//dec sp and go to stack\n@0\nM=M-1\nA=M\n//get top stack val and clear it\nD=M\nM=0\n//go to next stack val\n@0\nA=M-1\n//sub from eachother to check if ==\nD=D-M\n//jump to true if they are the same\n@TRUE_X\nD;JEQ\n//false\n@0\nA=M-1\nM=0\n@END_X\n0;JMP\n//true\n(TRUE_X)\n@0\nA=M-1\nM=1\n//the end of branch\n(END_X)\n"},
        {"gt"    , "//greater than\n@0\nM=M-1\nA=M\nD=M\n@13\nM=D\n@0\nM=M-1\nA=M\nD=M\n@13\nD=D-M\n@TRUE_X\nD;JGT\n@13\nM=0\nD=0\n@PUSH_X\n0;JMP\n(TRUE_X)\n@13\nM=0\nD=1\n(PUSH_X)\n@0\nA=M\nM=D\n@0\nM=M+1\n"},
        {"lt"    , "//less than\n@0\nM=M-1\nA=M\nD=M\n@13\nM=D\n@0\nM=M-1\nA=M\nD=M\n@13\nD=D-M\n@TRUE_X\nD;JLT\n@13\nM=0\nD=0\n@PUSH_X\n0;JMP\n(TRUE_X)\n@13\nM=0\nD=1\n(PUSH_X)\n@0\nA=M\nM=D\n@0\nM=M+1\n"},
        {"and"   , "//--and--\n//decrement stack pointer and save to D then clear\n@0\nM=M-1\nA=M\nD=M\n//st||e val in r13\n@13\nM=D\n//retrieve 2nd value and st||e in D then clear\n@0\nM=M-1\nA=M\nD=M\n//compare values using and, save in D\n@13\nD=D&M\n//clear r13\nM=0\n//increment pointer and push D to stack\n@0\nM=M+1\nA=M-1\nM=D\n" },
        {"or"    , "//--or--\n//decrement stack pointer and save to D then clear\n@0\nM=M-1\nA=M\nD=M\n//st||e val in r13\n@13\nM=D\n//retrieve 2nd value and st||e in D then clear\n@0\nM=M-1\nA=M\nD=M\n//compare values using ||, save in D\n@13\nD=D|M\n//clear r13\nM=0\n//increment pointer and push D to stack\n@0\nM=M+1\nA=M-1\nM=D\n"},
        {"not"   , "//--not--\n//go to top value in the stack and save in D\n@0\nA=M-1\nD=M\n//negate D and push to stack\n//If 0, make 1. If 1, make 0\n@ISZERO_X\nD;JEQ\n//its one, make it zero\nD=0\n@PUSH_X\n0; JMP\n(ISZERO_X)\n//its zero, so make it one\nD=1\n(PUSH_X)\n//now push to stack\n@0\nA=M-1\nM=D\n"}
        };

        private string outputFilePath;
        private int labelIndex;
        private int returnIndex = 0;


        //private StreamWriter outputFile;

        private List<string> commands;

        public CodeWriter(string outputFilePath)
        {
            this.outputFilePath = outputFilePath;
            labelIndex = 0;
            commands = new List<string>();
        }

        public void setFileName(string fileName) => outputFilePath = Regex.Replace(fileName, "\\..{1,4}$", ".asm");
        //outputFilePath = fileName[:-2] + ".asm"

        public void writeArithmetic(string cmd)
        {
            if (!PatternDictionary.arithDic.ContainsKey(cmd))
                throw new InvalidOperationException($"Doesnt contain '{cmd}'");

            string pattern = arithDic[cmd];
            pattern = pattern.Replace("_X", $"_{labelIndex}");

            if (cmd == "eq" || cmd == "gt" || cmd == "lt" || cmd == "not")
                labelIndex = labelIndex + 1;

            //outputFile.Write(pattern);
            commands.Add(pattern);
        }

        public void WritePushPop(bool isPop, string memSeg, string locStr)
        {
            int loc = int.Parse(locStr);
            string pattern = "";
            int baseAddr = 0;

            switch (memSeg)
            {
                case "local":
                    baseAddr = segLcl;
                    break;
                case "argument":
                    baseAddr = segArg;
                    break;
                case "pointer":
                    baseAddr = segPoint;
                    break;
                case "temp":
                    baseAddr = segTemp;
                    break;
                case "static":
                    baseAddr = segStatic;
                    break;
            }
            //Pop
            if (isPop)
            {
                if (memSeg == "constant")
                    throw new InvalidOperationException("Cant pop to constant");

                //decrement stack pointer
                pattern = "//decr stack pointer\n@0\nM=M-1\n\n";

                if (popDict.ContainsKey(memSeg))
                    pattern = pattern + popDict[memSeg];
                else
                    pattern = pattern + popDict["default"];

            }
            //Push
            else
            {
                if (pushDict.ContainsKey(memSeg))
                    pattern = pushDict[memSeg];
                else
                    pattern = pushDict["default"];

                // increment stack pointer
                pattern = pattern + "//incr stack pointer\n@0\nM=M+1\n\n";
            }

            // replacing the values in the string
            pattern = pattern.Replace("{loc}", loc.ToString());
            pattern = pattern.Replace("{memSeg}", memSeg);

            pattern = pattern.Replace("{addr}", (baseAddr + loc).ToString());
            pattern = pattern.Replace("{addrLoc}", baseAddr.ToString());

            //outputFile.Write(pattern);
            commands.Add(pattern);
        }

        // incr and decr stack pointer
        /*public void OpenFile() => outputFile = new StreamWriter(File.OpenWrite(outputFilePath));

        public void CloseFile()
        {

            outputFile.Write("\n\n(END_OF_PROGRAM)\n@END_OF_PROGRAM\n0;JMP");
            outputFile.Close();
        }*/
        public void WriteToFile()
        {
            using (var output = new StreamWriter(File.OpenWrite(outputFilePath)))
            {
                foreach (var cmd in commands)
                    output.WriteLine(cmd);

                commands.Add("\n\n(END_OF_PROGRAM)\n@END_OF_PROGRAM\n0;JMP");
            }
        }


        // test
        public void WriteLabel(string label)
        {
            //outputFile.Write($"({label.ToUpper()})\n");
            commands.Add($"({label.ToUpper()})\n");
        }

        //test
        public void WriteGoto(string label)
        {
            commands.Add($"@{label.ToUpper()}");
            commands.Add("0;JMP");
        }

        //test
        public void WriteIf(string label)
        {
            //pop top stack to register
            //if stack != 0 jump
            commands.Add("@0");
            commands.Add("A=M-1");
            commands.Add("D=M");
            commands.Add("M=0");
            //decr stack pointer
            commands.Add("@" + label.ToUpper());
            commands.Add("D;JNE");
        }

        public void WriteFunction(string funcName, int nVars)
        {
            WriteLabel(funcName);
            for (int i = 0; i < nVars; i++)
                WritePushPop(false, "constant", i.ToString());
        }


        public void WriteCall(string funcName, int nArgs) {
            //save the future label pos after calling the function
            commands.Add($"@RETURN{returnIndex}");
            commands.Add("D=A");
            PushToStack("D");
            //change to number if it throws an error for @LCL
            SavePointer("LCL");
            SavePointer("ARG");
            SavePointer("THIS");
            SavePointer("THAT");
            //go to stack pos
            commands.Add("@SP");
            commands.Add("D=M");
            //sub 5 to acct for the 5 values of the saved frame
            commands.Add($"@{nArgs+5}");
            commands.Add("D=D-A");
            //save location of saved frame
            commands.Add("@ARG");
            commands.Add("M=D");
            //load stack pointer
            commands.Add("@SP");
            commands.Add("D=M");
            //set local to SP
            commands.Add("@LCL");
            commands.Add("M=D");


            for (int i = 0; i < nArgs; i++)
                WritePushPop(false, "argument", i.ToString());
            WriteGoto(funcName);
            //the pos that was saved to jump to after the func
            WriteLabel($"RETURN{returnIndex}");

            returnIndex++;
        }
        private void SavePointer(string pointer)
        {
            commands.Add($"@{pointer}");
            commands.Add($"D=M");
            PushToStack("D");
        }
        private void PushToStack(string arg)
        {
            commands.Add($"@SP");
            commands.Add($"A=M");
            commands.Add($"M={arg}");
            commands.Add($"@SP");
            commands.Add($"M=M+1");
        }

        public void WriteReturn() {
            //pop arg 0
            WritePushPop(true, "arguement", 0.ToString());
            //restore frame
            RestorePointer("THAT");
            RestorePointer("THIS");
            RestorePointer("ARG");
            RestorePointer("LCL");

            //load jump addr loc
            DecrSP();
            commands.Add($"@SP");
            commands.Add($"A=M");
            commands.Add($"D=M");
            //jump
            commands.Add($"A=D");
            commands.Add($"0;JMP");
        }
        private void RestorePointer(string arg)
        {
            //decr stack pointer
            DecrSP();
            commands.Add($"@SP");
            commands.Add($"A=M");
            //get the stack value
            commands.Add($"D=M");
            commands.Add($"M=0");
            //set it to @arg
            commands.Add($"@{arg}");
            commands.Add($"M=D");
        }

        private void DecrSP()
        {
            commands.Add($"@SP");
            commands.Add($"M=M-1");
        }
        private void IncrSP()
        {
            commands.Add($"@SP");
            commands.Add($"M=M+1");
        }
    }
}
