(SIMPLEFUNCTION.TEST)

//--push constant 0--
//get constant 0
@0
D=A
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--push constant 0--
//get constant 0
@0
D=A
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--push local 0--
//get local 0
@0
D=A
@1
A=D+M
D=M
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--push local 1--
//get local 1
@1
D=A
@1
A=D+M
D=M
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--add--
//decr sp
@0
M=M-1
//go to stack and save to reg and clear
A=M
D=M
M=0
//go to lower stack location and add to it
@0
A=M-1
M=M+D

//--not--
//go to top value in the stack and save in D
@0
A=M-1
D=M
//negate D and push to stack
//If 0, make 1. If 1, make 0
@ISZERO_0
D;JEQ
//its one, make it zero
D=0
@PUSH_0
0; JMP
(ISZERO_0)
//its zero, so make it one
D=1
(PUSH_0)
//now push to stack
@0
A=M-1
M=D

//--push argument 0--
//get argument 0
@0
D=A
@2
A=D+M
D=M
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--add--
//decr sp
@0
M=M-1
//go to stack and save to reg and clear
A=M
D=M
M=0
//go to lower stack location and add to it
@0
A=M-1
M=M+D

//--push argument 1--
//get argument 1
@1
D=A
@2
A=D+M
D=M
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--sub--
//dec sp
@0
M=M-1
//go to stack and save to reg and clear
A=M
D=M
M=0
//go to lower stack addr and sub to it
@0
A=M-1
M=M-D

//decr stack pointer
@0
M=M-1

//--pop arguement 0--
//get addr arguement 0
@0
D=A
@0
D=D+M
//save location to gen reg 
@13
M=D
//get value from stack, accounting f|| the moved pointer
@0
A=M
D=M
//move back to st||ed value
@13
A=M
M=D
//clear stack value
@0
A=M
M=0
//clear gen reg value
@13
M=0


@SP
M=M-1
@SP
A=M
D=M
M=0
@THAT
M=D
@SP
M=M-1
@SP
A=M
D=M
M=0
@THIS
M=D
@SP
M=M-1
@SP
A=M
D=M
M=0
@ARG
M=D
@SP
M=M-1
@SP
A=M
D=M
M=0
@LCL
M=D
@SP
M=M-1
@SP
A=M
D=M
A=D
0;JMP


(END_OF_PROGRAM)
@END_OF_PROGRAM
0;JMP
