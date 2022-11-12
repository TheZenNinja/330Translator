﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    internal static class PatternDictionary
    {
        public static readonly Dictionary<string, string[]> pushDict = new Dictionary<string, string[]>{
        {"default"  , new string[] {"//--push {memSeg} {loc}--",
                                    "//get {memSeg} {loc}",
                                    "@{loc}",
                                    "D=A",
                                    "@{addrLoc}",
                                    "A=D+M",
                                    "D=M",
                                    "//push to stack",
                                    "@0",
                                    "A=M",
                                    "M=D",
                                    ""}},
        {"constant" , new string[] {"//--push constant {loc}--",
                                    "//get constant {loc}",
                                    "@{addr}",
                                    "D=A",
                                    "//push to stack",
                                    "@0",
                                    "A=M",
                                    "M=D",
                                    ""}},
        {"pointer"  , new string[] {"//--push pointer {loc}",
                                    "@{addr}",
                                    "D=M",
                                    "//push to stack",
                                    "@0",
                                    "A=M",
                                    "M=D",
                                    ""}},
        { "temp"    , new string[] {"//--push temp {loc}",
                                    "@{addr}",
                                    "D=M",
                                    "//push to stack",
                                    "@0",
                                    "A=M",
                                    "M=D",
                                    ""}}
        };
        public static readonly Dictionary<string, string[]> popDict = new Dictionary<string, string[]>{
        {"default"   , new string[] {"//--pop {memSeg} {loc}--",
                                     "//get addr {memSeg} {loc}",
                                     "@{loc}",
                                     "D=A",
                                     "@{addrLoc}",
                                     "D=D+M",
                                     "//save location to gen reg ",
                                     "@13",
                                     "M=D",
                                     "//get value from stack, accounting f|| the moved pointer",
                                     "@0",
                                     "A=M",
                                     "D=M",
                                     "//move back to st||ed value",
                                     "@13",
                                     "A=M",
                                     "M=D",
                                     "//clear stack value",
                                     "@0",
                                     "A=M",
                                     "M=0",
                                     "//clear gen reg value",
                                     "@13",
                                     "M=0",
                                     "",
                                     ""}},
        {"pointer"   , new string[] {"//--pop pointer {loc}--",
                                     "//get addr pointer {loc}",
                                     "@{addrLoc}",
                                     "D=A",
                                     "//save location to gen reg",
                                     "@13",
                                     "M=D",
                                     "//get value from stack, accounting f|| the moved pointer",
                                     "@0",
                                     "A=M",
                                     "D=M",
                                     "//move back to st||ed value",
                                     "@13",
                                     "A=M",
                                     "M=D",
                                     "//clear stack value",
                                     "@0",
                                     "A=M",
                                     "M=0",
                                     "//clear gen reg value",
                                     "@13",
                                     "M=0",
                                     "" }},
        {"temp"      , new string[] {"//--pop temp {loc}--",
                                     "//get addr temp {loc}",
                                     "@{addr}",
                                     "D=A",
                                     "//save location to gen reg",
                                     "@13",
                                     "M=D",
                                     "//get value from stack, accounting f|| the moved pointer",
                                     "@0",
                                     "A=M",
                                     "D=M",
                                     "//move back to st||ed value",
                                     "@13",
                                     "A=M",
                                     "M=D",
                                     "//clear stack value",
                                     "@0",
                                     "A=M",
                                     "M=0",
                                     "//clear gen reg value",
                                     "@13",
                                     "M=0",
                                     ""}},
        };
        public static readonly Dictionary<string, string[]> arithDic = new Dictionary<string, string[]>{
        {"add"   , new string[] {"//--add--",
                                 "//decr sp",
                                 "@0",
                                 "M=M-1",
                                 "//go to stack and save to reg and clear",
                                 "A=M",
                                 "D=M",
                                 "M=0",
                                 "//go to lower stack location and add to it",
                                 "@0",
                                 "A=M-1",
                                 "M=M+D",
                                 "" }},
        {"sub"   , new string[] {"//--sub--",
                                 "//dec sp",
                                 "@0",
                                 "M=M-1",
                                 "//go to stack and save to reg and clear",
                                 "A=M",
                                 "D=M",
                                 "M=0",
                                 "//go to lower stack addr and sub to it",
                                 "@0",
                                 "A=M-1",
                                 "M=M-D",
                                 ""}},
        {"neg"   , new string[] {"//--neg--",
                                 "@0",
                                 "//load 0 into reg",
                                 "D=A",
                                 "//go to stack addr",
                                 "A=M-1",
                                 "//sub 0 from the value in mem",
                                 "M=D-M",
                                 ""}},
        {"eq"    , new string[] {"//--eq--",
                                 "//dec sp and go to stack",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "//get top stack val and clear it",
                                 "D=M",
                                 "M=0",
                                 "//go to next stack val",
                                 "@0",
                                 "A=M-1",
                                 "//sub from eachother to check if ==",
                                 "D=D-M",
                                 "//jump to true if they are the same",
                                 "@TRUE_X",
                                 "D;JEQ",
                                 "//false",
                                 "@0",
                                 "A=M-1",
                                 "M=0",
                                 "@END_X",
                                 "0;JMP",
                                 "//true",
                                 "(TRUE_X)",
                                 "@0",
                                 "A=M-1",
                                 "M=1",
                                 "//the end of branch",
                                 "(END_X)",
                                 ""}},
        {"gt"    , new string[] {"//greater than",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "@13",
                                 "M=D",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "@13",
                                 "D=D-M",
                                 "@TRUE_X",
                                 "D;JGT",
                                 "@13",
                                 "M=0",
                                 "D=0",
                                 "@PUSH_X",
                                 "0;JMP",
                                 "(TRUE_X)",
                                 "@13",
                                 "M=0",
                                 "D=1",
                                 "(PUSH_X)",
                                 "@0",
                                 "A=M",
                                 "M=D",
                                 "@0",
                                 "M=M+1",
                                 ""}},
        {"lt"    , new string[] {"//less than",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "@13",
                                 "M=D",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "@13",
                                 "D=D-M",
                                 "@TRUE_X",
                                 "D;JLT",
                                 "@13",
                                 "M=0",
                                 "D=0",
                                 "@PUSH_X",
                                 "0;JMP",
                                 "(TRUE_X)",
                                 "@13",
                                 "M=0",
                                 "D=1",
                                 "(PUSH_X)",
                                 "@0",
                                 "A=M",
                                 "M=D",
                                 "@0",
                                 "M=M+1",
                                 ""}},
        {"and"   , new string[] {"//--and--",
                                 "//decrement stack pointer and save to D then clear",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "//st||e val in r13",
                                 "@13",
                                 "M=D",
                                 "//retrieve 2nd value and st||e in D then clear",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "//compare values using and, save in D",
                                 "@13",
                                 "D=D&M",
                                 "//clear r13",
                                 "M=0",
                                 "//increment pointer and push D to stack",
                                 "@0",
                                 "M=M+1",
                                 "A=M-1",
                                 "M=D",
                                 "" }},
        {"or"    , new string[] {"//--||--",
                                 "//decrement stack pointer and save to D then clear",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "//st||e val in r13",
                                 "@13",
                                 "M=D",
                                 "//retrieve 2nd value and st||e in D then clear",
                                 "@0",
                                 "M=M-1",
                                 "A=M",
                                 "D=M",
                                 "//compare values using ||, save in D",
                                 "@13",
                                 "D=D|M",
                                 "//clear r13",
                                 "M=0",
                                 "//increment pointer and push D to stack",
                                 "@0",
                                 "M=M+1",
                                 "A=M-1",
                                 "M=D",
                                 ""}},
        {"not"   , new string[] {"//--not--",
                                 "//go to top value in the stack and save in D",
                                 "@0",
                                 "A=M-1",
                                 "D=M",
                                 "//negate D and push to stack",
                                 "//If 0, make 1. If 1, make 0",
                                 "@ISZERO_X",
                                 "D;JEQ",
                                 "//its one, make it zero",
                                 "D=0",
                                 "@PUSH_X",
                                 "0; JMP",
                                 "(ISZERO_X)",
                                 "//its zero, so make it one",
                                 "D=1",
                                 "(PUSH_X)",
                                 "//now push to stack",
                                 "@0",
                                 "A=M-1",
                                 "M=D",
                                 "" }}
        };
    }
}