using System;

namespace Translator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // inputFile = input("Input File Path) ")
            string inputFile = "test.vm";//input("Input File Path) ")
                                         //outputFile = input("Output File Path (Optional)) ")
            string outputFile = "";

            //create parser
            var parser = new Parser(inputFile);
            //create codewriter
            var cWriter = new CodeWriter(outputFile);

            //if there is no stated output file name
            if (outputFile.Length < 4)
                cWriter.setFileName(inputFile);

            // open file
            //cWriter.OpenFile();

            while (parser.hasMoreCommands())
            {
                // check command type
                string cmd = parser.commandType();
                if (CodeWriter.arithDic.ContainsKey(cmd))
                    cWriter.writeArithmetic(cmd);
                else
                    switch (cmd)
                    {
                        case "push":
                            cWriter.WritePushPop(false, parser.arg1(), parser.arg2());
                            break;
                        case "pop":
                            cWriter.WritePushPop(true, parser.arg1(), parser.arg2());
                            break;
                        case "function":
                            cWriter.WriteFunction(parser.arg1(), int.Parse(parser.arg2()));
                            break;
                        case "call":
                            cWriter.WriteCall(parser.arg1(), int.Parse(parser.arg2()));
                            break;
                        case "return":
                            cWriter.WriteReturn();
                            break;
                        //"comment"
                        //whitespace causing program to hang
                        case "":
                        case "//":
                            continue;
                        default:
                            throw new ArgumentException($"invalid command '{cmd}'");
                    }
                // advance
                parser.advance();
            }
            // close file
            //cWriter.CloseFile();

            cWriter.WriteToFile();

            Console.WriteLine(inputFile, "converted to asm");
        }
    }
}