(FUNC.TEST)

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


//--push constant 1--
//get constant 1
@1
D=A
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--push arguement 0--
//get arguement 0
@0
D=A
@0
A=D+M
D=M
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//--push arguement 1--
//get arguement 1
@1
D=A
@0
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
//--push constant 10--
//get constant 10
@10
D=A
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


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


//--push constant 5--
//get constant 5
@5
D=A
//push to stack
@0
A=M
M=D

//incr stack pointer
@0
M=M+1


//decr stack pointer
@0
M=M-1

//--pop arguement 1--
//get addr arguement 1
@1
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


@RETURN0
D=A
@SP
A=M
M=D
@SP
M=M+1
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1
@SP
D=M
@7
D=D-A
@ARG
M=D
@SP
D=M
@LCL
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


@FUNC.TEST
0;JMP
(RETURN0)

