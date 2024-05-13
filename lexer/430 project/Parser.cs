using Lang.Lexer;
using System.Reflection;
using static Lang.Parser.Parser;

namespace Lang.Parser
{
    public sealed class Parser
    {
        List<Token> tokens;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public class ParseResult<A>
        {
            public A parseResult;
            public int nextPosition;

            public ParseResult(A parseResult, int nextPosition)
            {
                this.parseResult = parseResult;
                this.nextPosition = nextPosition;
            }
        }

        public interface Op { }

        public interface Exp { }

        public interface Type { }

        public interface Vardec { }
        public interface Stmt { }
        public interface MethodDef { }
        public interface Constructor { }
        public interface ClassDef { }
        public interface Program { }

        public class VarExp : Exp
        {
            public string name;
            public VarExp(string name)
            {
                this.name = name;
            }
        }

        public class IntegerExp : Exp
        {
            public int value;
            public IntegerExp(int value)
            {
                this.value = value;
            }
            public override bool Equals(object? other)
            {
                if (other == null || !(other is IntegerExp))
                {
                    return false;
                }

                IntegerExp e = (IntegerExp)other;
                return e.value == value;
            }
            public override string ToString()
            {
                return $"IntegerExp({value})";
            }
        }

        public class TrueExp : Exp
        {
            public override bool Equals(object? other)
            {
                return other is TrueExp;
            }
            public override string ToString()
            {
                return "TrueExp()";
            }
        }

        public class FalseExp : Exp
        {
            public override bool Equals(object? other)
            {
                return other is FalseExp;
            }
            public override string ToString()
            {
                return "FalseExp()";
            }
        }

        public class ThisExp : Exp
        {
            public override bool Equals(object? other)
            {
                return other is ThisExp;
            }
            public override string ToString()
            {
                return "ThisExp()";
            }
        }

        public class Println : Exp
        {
            public Exp exp;
            public Println(Exp exp)
            {
                this.exp = exp;
            }
            public override bool Equals(object? other)
            {
                if (other is Println)
                {
                    Println e = (Println)other;
                    return e.exp.Equals(exp);
                }
                else
                {
                    return false;
                }
            }
            public override string ToString()
            {
                return $"PrintLn({exp.ToString()})";
            }
        }

        public class NewExp : Exp
        {
            public Identifier className;
            public NewExp(Identifier className)
            {
                this.className = className;
            }
            public override bool Equals(object? other)
            {
                if (other is NewExp)
                {
                    NewExp e = (NewExp)other;
                    return e.className.Equals(className);
                }
                else
                {
                    return false;
                }
            }
            public override string ToString()
            {
                return $"NewExp({className.ToString()})";
            }
        }

        public class MethodCall : Exp
        {
            public Exp Identifier;
            public Exp[] Parameters;

            public MethodCall(Exp Identifier, Exp[] Parameters)
            {
                this.Identifier = Identifier;
                this.Parameters = Parameters;
            }
            
            public override bool Equals(object? other)
            {
                if (other is MethodCall)
                {
                    MethodCall e = (MethodCall)other;

                    if (e.Parameters.Length != Parameters.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < Parameters.Length; i++)
                    {
                        if (!e.Parameters[i].Equals(Parameters[i]))
                        {
                            return false;
                        }
                    }

                    return e.Identifier.Equals(Identifier);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string paramString = "";

                if (Parameters.Length > 0)
                {
                    paramString += $"{Parameters[0].ToString()}";
                }
                
                for (int i = 1; i < Parameters.Length; i++)
                {
                    paramString += $", {Parameters[i].ToString()}";
                }

                return $"MethodCall({Identifier.ToString()}, [{paramString}])";
            }
        }

        public class Identifier : Exp
        {
            public string value;
            public Identifier(string value)
            {
                this.value = value;
            }
            public override bool Equals(object? other)
            {
                if (other is Identifier)
                {
                    Identifier e = (Identifier)other;
                    return value.Equals(e.value);
                }
                else
                {
                    return false;
                }
            }
            public override string ToString()
            {
                return $"IdentifierExp({value})";
            }
        }

        public class BinopExp : Exp
        {
            public Exp left;
            public Op op;
            public Exp right;

            public BinopExp(Exp left, Op op, Exp right)
            {
                this.left = left;
                this.op = op;
                this.right = right;
            }
            public override bool Equals(object? other)
            {
                if (other is BinopExp)
                {
                    BinopExp e = (BinopExp)other;
                    return e.left.Equals(left) && e.right.Equals(right) && e.op.Equals(op);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return $"BinOpExp({left.ToString()} {op.ToString()} {right.ToString()})";
            }
        }

        public class PeriodOp : Op {   
           public override bool Equals(object? other)
            {
                return other is PeriodOp;
            }
            public override string ToString()
            {
                return "PeriodOp()";
            } 
        }


        public class PlusOp : Op
        {
            public override bool Equals(object? other)
            {
                return other is PlusOp;
            }
            public override string ToString()
            {
                return "PlusOp()";
            }
        }
        public class MinusOp : Op
        {
            public override bool Equals(object? other)
            {
                return other is MinusOp;
            }
            public override string ToString()
            {
                return "PlusOp()";
            }
        }
        public class MultOp : Op
        {
            public override bool Equals(object? other)
            {
                return other is MultOp;
            }
            public override string ToString()
            {
                return "MultOp()";
            }
        }
        public class DivOp : Op
        {
            public override bool Equals(object? other)
            {
                return other is DivOp;
            }
            public override string ToString()
            {
                return "DivOp()";
            }
        }

        public class IntType : Type
        {
            public override bool Equals(object? other)
            {
                return other is IntType;
            }
            public override string ToString()
            {
                return "IntType()";
            }
        }

        public class BooleanType : Type
        {
            public override bool Equals(object? other)
            {
                return other is BooleanType;
            }
            public override string ToString()
            {
                return "BooleanType()";
            }
        }
        
        public class VoidType : Type
        {
            public override bool Equals(object? other)
            {
                return other is VoidType;
            }
            public override string ToString()
            {
                return "VoidType()";
            }
        }

        public class CLassNameType : Type 
        {
            public string className;

            public CLassNameType(string className)
            {
                this.className = className;
            }

            public override string ToString()
            {
                return $"ClassType({className})";
            }
        }

        public Type tokenToDataType(Token token)
        {
            switch(token.Type)
            {
                case TokenType.IntToken:
                    return new IntType();
                case TokenType.BooleanToken:
                    return new BooleanType();
                case TokenType.VoidToken:
                    return new VoidType();
                case TokenType.Identifier:
                    return new CLassNameType(token.Lexeme);
                default:
                    throw new ParseException("Can't parse type " + token.Type);
            }
        }

        public bool canBeType(Token token)
        {
            return token.Type == TokenType.IntToken || token.Type == TokenType.BooleanToken || token.Type == TokenType.VoidToken || token.Type == TokenType.Identifier;
        }

        public class VarType : Vardec { }

        public class VarDecStmt : Stmt
        {
            public Type varType;
            public Identifier varIdentifier;

            public VarDecStmt(Type varType, Identifier varIdentifier)
            {
                this.varType = varType;
                this.varIdentifier = varIdentifier;
            }

            public override bool Equals(object? other)
            {
                if (other is VarDecStmt)
                {
                    VarDecStmt e = (VarDecStmt)other;
                    return e.varType.Equals(varType) && e.varIdentifier.Equals(varIdentifier);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return $"VarDec({varType.ToString()}, {varIdentifier.ToString()})";
            }
        }
        public class AssignmentStmt : Stmt
        {
            public Identifier variable;
            public Exp assignedExp;
            public AssignmentStmt(Identifier variable, Exp assignedExp)
            {
                this.variable = variable;
                this.assignedExp = assignedExp;
            }
            public override bool Equals(object? other)
            {
                if (other is AssignmentStmt)
                {
                    AssignmentStmt e = (AssignmentStmt)other;
                    return e.variable.Equals(variable) && e.assignedExp.Equals(assignedExp);
                }
                else
                {
                    return false;
                }
            }
            public override string ToString()
            {
                return $"AssignmentStmt({variable.ToString()}, {assignedExp})";
            }
        }
        public class ExpStmt : Stmt
        {
            public Exp expression;

            public ExpStmt(Exp expression)
            {
                this.expression = expression;
            }
            public override bool Equals(object? other)
            {
                if (other is ExpStmt)
                {
                    ExpStmt e = (ExpStmt)other;
                    return e.expression.Equals(expression);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return $"ExpStmt({expression.ToString()})";
            }
        }

        public class WhileStmt : Stmt
        {
            public Exp Condition;
            public Stmt body;
            public WhileStmt(Exp Condition, Stmt body)
            {
                this.Condition = Condition;
                this.body = body;
            }
            
            public override bool Equals(object? other)
            {
                if (other is WhileStmt)
                {
                    WhileStmt e = (WhileStmt)other;
                    return e.Condition.Equals(Condition) && e.body.Equals(body);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return $"WhileStmt({Condition.ToString()}, ({body.ToString()}))";
            }
        }
        public class BreakStmt : Stmt
        {
            public override bool Equals(object? other)
            {
                return other is BreakStmt;
            }
            public override string ToString()
            {
                return "BreakStmt()";
            }
        }

        public class ReturnStmt : Stmt
        {
            public Exp? left;
            

            public ReturnStmt(Exp? left)
            {
                this.left = left;
            }
            public override bool Equals(object? other)
            {
                if (other is ReturnStmt)
                {
                    ReturnStmt e = (ReturnStmt)other;
                    return (left == null && e.left == null) || (e.left != null && e.left.Equals(left));
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return (left == null) ? "ReturnStmt()" : $"ReturnStmt({left.ToString()})";
            }
        }
        public class IfStmt : Stmt
        {
            public Exp Condition;
            public Stmt ifBody;
            public Stmt? elseBody;
            public IfStmt(Exp Condition, Stmt ifBody, Stmt elseBody)
            {
                this.Condition = Condition;
                this.ifBody = ifBody;
                this.elseBody = elseBody;
            }

            public IfStmt(Exp Condition, Stmt ifBody)
            {
                this.Condition = Condition;
                this.ifBody = ifBody;
            }
           
            public override bool Equals(object? other)
            {
                if (other is IfStmt)
                {
                    IfStmt e = (IfStmt)other;
                    return e.Condition.Equals(Condition) && e.ifBody.Equals(ifBody) && ((elseBody == null && e.elseBody == null) || (e.elseBody != null && e.elseBody.Equals(elseBody)));
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string ifString = $"IfStmt({Condition.ToString()}, ({ifBody.ToString()})";

                if (elseBody != null)
                {
                    ifString += $", ElseStmt({elseBody.ToString()})";
                }

                return ifString + ")";
            }
        }
        public class BlockStmt : Stmt
        {
            public Stmt[] Block;
            public BlockStmt(Stmt[] Block)
            {
                this.Block = Block;
            }
            public override bool Equals(object? other)
            {
                if (other is BlockStmt)
                {
                    BlockStmt e = (BlockStmt)other;

                    if (e.Block.Length != Block.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < Block.Length; i++)
                    {
                        if (!e.Block[i].Equals(Block[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string result = "";
                foreach (Stmt stmt in Block)
                {
                    result += $"\t{stmt.ToString()}\n";
                }
                return $"BlockStmt(\n{result})";
            }
        }

        public class Method : MethodDef
        {
            public Identifier methodName;
            public Type returnType;
            public Stmt[] parameters;
            public Stmt methodBody;

            public Method(Identifier methodName, Type returnType, Stmt[] parameters, Stmt methodBody)
            {
                this.methodName = methodName;
                this.returnType = returnType;
                this.parameters = parameters;
                this.methodBody = methodBody;
            }
            public override bool Equals(object? other)
            {
                if (other is Method)
                {
                    Method e = (Method)other;

                    if (e.parameters.Length != parameters.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (!e.parameters[i].Equals(parameters[i]))
                        {
                            return false;
                        }
                    }

                    return e.methodName.Equals(methodName) && e.returnType.Equals(returnType) && e.methodBody.Equals(methodBody);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string paramsString = "";

                if (parameters?.Length > 0)
                {
                    paramsString += $"{parameters[0].ToString()}";
                }

                for (int i = 1; i < parameters?.Length; i++)
                {
                    paramsString += $", {parameters[i].ToString()}";
                }

                return $"MethodDef(\n{methodName.ToString()}, {returnType.ToString()}, [{paramsString}]\n{methodBody.ToString()})";
            }
        }

        public class Con : Constructor
        {
            public Stmt[] parameters;
            public bool callsSuper;
            public Exp[]? superParameters;
            public Stmt constructorBody;
            
            public Con(Stmt[] parameters, Exp[]? superParameters, Stmt constructorBody)
            {
                this.parameters = parameters;
                this.callsSuper = true;
                this.superParameters = superParameters;
                this.constructorBody = constructorBody;
            }

            public Con(Stmt[] parameters, Stmt constructorBody)
            {
                this.parameters = parameters;
                this.callsSuper = false;
                this.superParameters = null;
                this.constructorBody = constructorBody;
            }

            public override bool Equals(object? other)
            {
                if (other is Con)
                {
                    Con e = (Con)other;
                    if (e.parameters.Length != parameters.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (!e.parameters[i].Equals(parameters[i]))
                        {
                            return false;
                        }
                    }

                    if (callsSuper)
                    {
                        if (!e.callsSuper || e.parameters.Length != parameters.Length)
                        {
                            return false;
                        }

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (!e.parameters[i].Equals(parameters[i]))
                            {
                                return false;
                            }
                        }
                    }

                    return e.constructorBody.Equals(constructorBody);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string paramsString = "";
                string superParamsString = "";

                if (parameters?.Length > 0)
                {
                    paramsString += $"{parameters[0].ToString()}";
                }

                for (int i = 1; i < parameters?.Length; i++)
                {
                    paramsString += $", {parameters[i].ToString()}";
                }

                if (callsSuper)
                {
                    if (superParameters?.Length > 0)
                    {
                        superParamsString += $"{superParameters[0].ToString()}";
                    }

                    for (int i = 1; i < superParameters?.Length; i++)
                    {
                        superParamsString += $", {superParameters[i].ToString()}";
                    }
                }

                return $"Constructor(\n[{paramsString}]\n{((callsSuper) ? $"Calls Super, [{superParamsString}]" : "No Super, ")}\n{constructorBody.ToString()})";
            }
        }

        public class Class : ClassDef
        {
            public Identifier className;
            public Identifier? extendingClassName;
            public Stmt[] localVarDecs;
            public Constructor constructor;
            public MethodDef[] classMethods;

            public Class(Identifier className, Identifier? extendingClassName, Stmt[] localVarDecs, Constructor constructor, MethodDef[] classMethods)
            {
                this.className = className;
                this.extendingClassName = extendingClassName;
                this.localVarDecs = localVarDecs;
                this.constructor = constructor;
                this.classMethods = classMethods;
            }

            public override bool Equals(object? other)
            {
                if (other is Class)
                {
                    Class e = (Class)other;

                    if (e.localVarDecs.Length != localVarDecs.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < localVarDecs.Length; i++)
                    {
                        if (!e.localVarDecs[i].Equals(localVarDecs[i]))
                        {
                            return false;
                        }
                    }

                    if (e.classMethods.Length != classMethods.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < classMethods.Length; i++)
                    {
                        if (!e.classMethods[i].Equals(classMethods[i]))
                        {
                            return false;
                        }
                    }

                    return e.className.Equals(className) && ((extendingClassName == null && e.extendingClassName == null) || (e.extendingClassName != null && e.extendingClassName.Equals(extendingClassName))) && e.constructor.Equals(constructor);
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string varDecString = "";
                foreach (Stmt varDec in localVarDecs)
                {
                    varDecString += $"{varDec.ToString()}\n";
                }

                string methodsString = "";
                foreach (MethodDef methodDef in classMethods)
                {
                    methodsString += $"{methodDef.ToString()}\n";
                }

                return $"ClassDef(\n{className.ToString()} : {extendingClassName?.ToString()}\n{varDecString}{constructor.ToString()}\n{methodsString})";
            }
        }

        public class Code : Program
        {
            public ClassDef[] classDefs;
            public Stmt[] stmts;
            public Code(ClassDef[] classDefs, Stmt[] stmts)
            {
                this.classDefs = classDefs;
                this.stmts = stmts;
            }
            public override bool Equals(object? other)
            {
                if (other is Code)
                {
                    Code e = (Code)other;

                    if (e.classDefs.Length != classDefs.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < classDefs.Length; i++)
                    {
                        if (!e.classDefs[i].Equals(classDefs[i]))
                        {
                            return false;
                        }
                    }

                    if (e.stmts.Length != stmts.Length)
                    {
                        return false;
                    }

                    for (int i = 0; i < stmts.Length; i++)
                    {
                        if (!e.stmts[i].Equals(stmts[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                string programString = "";

                foreach (ClassDef def in classDefs)
                {
                    programString += $"{def.ToString()}\n";
                }

                foreach (Stmt stmt in stmts)
                {
                    programString += $"{stmt.ToString()}\n";
                }

                return programString;
            }
        }

        public ParseResult<Program> ParseProgram(int startPosition)
        {
            List<ClassDef> classDefs = new List<ClassDef>();
            List<Stmt> stmts = new List<Stmt>();

            int currentPosition = startPosition;
            Token currentToken = tokens[currentPosition];

            while (currentPosition < tokens.Count && currentToken.Type == TokenType.ClassToken)
            {
                ParseResult<ClassDef> classDef = ParseClassDef(currentPosition);
                classDefs.Add(classDef.parseResult);
                currentPosition = classDef.nextPosition;
                currentToken = tokens[currentPosition];
            }

            if (currentPosition >= tokens.Count)
            {
                throw new ParseException("No Entry Point in file");
            }

            ParseResult<Stmt> programStmt = ParseStmt(currentPosition);
            stmts.Add(programStmt.parseResult);
            currentPosition = programStmt.nextPosition;
            while (currentPosition < tokens.Count)
            {               
                programStmt = ParseStmt(currentPosition);
                stmts.Add(programStmt.parseResult);
                currentPosition = programStmt.nextPosition;
            }

            return new ParseResult<Program>(new Code(classDefs.ToArray(), stmts.ToArray()), currentPosition + 1);
        }

        public ParseResult<Op> ParseOp(int startPosition)
        {
            Token token = tokens[startPosition];

            switch (token.Type)
            {
                case TokenType.AddToken:
                    return new ParseResult<Op>(new PlusOp(), startPosition + 1);
                case TokenType.SubToken:
                    return new ParseResult<Op>(new MinusOp(), startPosition + 1);
                case TokenType.MultToken:
                    return new ParseResult<Op>(new MultOp(), startPosition + 1);
                case TokenType.DivToken:
                    return new ParseResult<Op>(new DivOp(), startPosition + 1);
                default:
                    throw new ParseException("Failed to Parse Operator.");
            }
        }

        public ParseResult<Stmt> ParseVarDecStmt(int startPosition)
        {
            Token token = tokens[startPosition];

            if (tokens[startPosition + 1].Type != TokenType.Identifier)
            {
                throw new ParseException("Identifier expected");
            }

            if (canBeType(token))
            {
                return new ParseResult<Stmt>(new VarDecStmt(tokenToDataType(token), new Identifier(tokens[startPosition + 1].Lexeme)), startPosition + 2);
            }

            throw new ParseException("Variable type expected");
        }

        public ParseResult<Stmt[]> ParseCommaVarDec(int startPosition)
        {
            List<Stmt> varDecs = new List<Stmt>();
            int nextPosition = startPosition;

            if (tokens[startPosition].Type != TokenType.CloseParenthesisToken)
            {
                ParseResult<Stmt> startVarDec = ParseVarDecStmt(startPosition);

                varDecs.Add(startVarDec.parseResult);

                nextPosition = startVarDec.nextPosition;
                Token currentToken = tokens[nextPosition];

                while (currentToken.Type == TokenType.CommaToken)
                {
                    ParseResult<Stmt> nextVarDec = ParseVarDecStmt(nextPosition + 1);
                    varDecs.Add(nextVarDec.parseResult);
                    nextPosition = nextVarDec.nextPosition;
                    currentToken = tokens[nextPosition];
                }
            }
            
            return new ParseResult<Stmt[]>(varDecs.ToArray(), nextPosition);
        }

        public ParseResult<Stmt> ParseBreakStmt(int startPosition)
        {
            Token token = tokens[startPosition];
                switch (token.Type)
            {
                case TokenType.BreakToken:
                    if (startPosition + 1 < tokens.Count && tokens[startPosition + 1].Type == TokenType.SemicolonToken)
                        return new ParseResult<Stmt>(new BreakStmt(), startPosition + 2);
                    else
                        throw new ParseException("Missing Semicolon");
                default:
                    throw new ParseException("defaulted in ParseBreakStmt");
            }
        }

        public ParseResult<Stmt> ParseReturnStmt(int startPosition)
        {
            Token token = tokens[startPosition];
            if (token.Type == TokenType.ReturnToken)
            {
                if (startPosition + 1 >= tokens.Count)
                {
                    throw new ParseException("Unexpected End of File after return statement");
                }
                if (tokens[startPosition + 1].Type != TokenType.SemicolonToken)
                {
                    ParseResult<Exp> ReturnExp = ParseExp(startPosition + 1);
                    return new ParseResult<Stmt>(new ReturnStmt(ReturnExp.parseResult), ReturnExp.nextPosition);
                }
                else
                {
                    return new ParseResult<Stmt>(new ReturnStmt(null), startPosition + 1);
                }
            }

            throw new ParseException("defaulted in ParseReturnStmt");
        }


        public ParseResult<Stmt> ParseStmt(int startPosition)
        {
            Token token = tokens[startPosition];
            switch (token.Type)
            {
                case TokenType.IntToken:
                case TokenType.BooleanToken:
                case TokenType.VoidToken:
                    ParseResult<Stmt> varDec = ParseVarDecStmt(startPosition);
                    if (varDec.nextPosition < tokens.Count && tokens[varDec.nextPosition].Type == TokenType.SemicolonToken)
                    {
                        return new ParseResult<Stmt>(varDec.parseResult, varDec.nextPosition + 1);
                    }
                    else
                        throw new ParseException("Missing Semicolon");
                case TokenType.Identifier:
                    if (tokens[startPosition + 1].Type == TokenType.EqualsToken)
                    {
                        ParseResult<Exp> exp = ParseExp(startPosition + 2);

                        if (exp.nextPosition < tokens.Count && tokens[exp.nextPosition].Type == TokenType.SemicolonToken)
                            return new ParseResult<Stmt>(new AssignmentStmt(new Identifier(token.Lexeme), exp.parseResult), exp.nextPosition + 1);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    else if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        // Current token is a custom class, parse as a vardec
                        ParseResult<Stmt> customClassVarDec = ParseVarDecStmt(startPosition);
                        if (customClassVarDec.nextPosition < tokens.Count && tokens[customClassVarDec.nextPosition].Type == TokenType.SemicolonToken)
                        {
                            return new ParseResult<Stmt>(customClassVarDec.parseResult, customClassVarDec.nextPosition + 1);
                        }
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    else
                    {
                        // Try to parse as an expression
                        ParseResult<Exp> exp = ParseExp(startPosition);
                        if (exp.nextPosition < tokens.Count && tokens[exp.nextPosition].Type == TokenType.SemicolonToken)
                            return new ParseResult<Stmt>(new ExpStmt(exp.parseResult), exp.nextPosition + 1);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                case TokenType.BreakToken:
                    return ParseBreakStmt(startPosition);
                case TokenType.ReturnToken:
                    ParseResult<Stmt> returnStmt = ParseReturnStmt(startPosition);
                    return new ParseResult<Stmt>(returnStmt.parseResult, returnStmt.nextPosition + 1);
                case TokenType.OpenBracketToken:
                    ParseResult<Stmt> block = ParseBlock(startPosition + 1);
                    return new ParseResult<Stmt>(block.parseResult, block.nextPosition + 1);
                case TokenType.WhileToken:
                    if (tokens[startPosition + 1].Type == TokenType.OpenParenthesisToken)
                    {
                        ParseResult<Exp> exp = ParseExp(startPosition + 2);
                        if (tokens[exp.nextPosition].Type == TokenType.CloseParenthesisToken)
                        {
                            ParseResult<Stmt> stmt = ParseStmt(exp.nextPosition + 1);
                            return new ParseResult<Stmt>(new WhileStmt(exp.parseResult, stmt.parseResult), stmt.nextPosition);
                        }
                        else
                            throw new ParseException("Missing Close Parenthesis");
                    }
                    else
                        throw new ParseException("Missing Open Parenthesis");
                case TokenType.IfToken:
                    if (tokens[startPosition + 1].Type == TokenType.OpenParenthesisToken)
                    {
                        ParseResult<Exp> exp = ParseExp(startPosition + 2);
                        if (tokens[exp.nextPosition].Type == TokenType.CloseParenthesisToken)
                        {
                            ParseResult<Stmt> stmt = ParseStmt(exp.nextPosition + 1);
                            if (stmt.nextPosition < tokens.Count && tokens[stmt.nextPosition].Type == TokenType.ElseToken)
                            {
                                ParseResult<Stmt> elseStmt = ParseStmt(stmt.nextPosition + 1);
                                return new ParseResult<Stmt>(new IfStmt(exp.parseResult, stmt.parseResult, elseStmt.parseResult), elseStmt.nextPosition);
                            }
                            return new ParseResult<Stmt>(new IfStmt(exp.parseResult, stmt.parseResult), stmt.nextPosition);
                        }
                        else
                            throw new ParseException("Missing Close Parenthesis");
                    }
                    else
                        throw new ParseException("Missing Open Parenthesis");
                case TokenType.IntegerLiteral:
                case TokenType.OpenParenthesisToken:
                case TokenType.ThisToken:
                case TokenType.TrueToken:
                case TokenType.FalseToken:
                case TokenType.PrintlnToken:
                case TokenType.NewToken:
                    ParseResult<Exp> stmtExp = ParseExp(startPosition);
                    if (stmtExp.nextPosition < tokens.Count && tokens[stmtExp.nextPosition].Type == TokenType.SemicolonToken)
                        return new ParseResult<Stmt>(new ExpStmt(stmtExp.parseResult), stmtExp.nextPosition + 1);
                    else
                        throw new ParseException("Missing Semicolon");

                default:
                    throw new ParseException("defaulted in ParseStmt");

            }
        }

        public ParseResult<Stmt> ParseBlock(int startPosition)
        {
            List<Stmt> stmts = new List<Stmt>();
            int nextPosition = startPosition;
            Token token = tokens[nextPosition];
            
            while (token.Type != TokenType.ClosedBracketToken)
            {
                ParseResult<Stmt> nextStmt = ParseStmt(nextPosition);
                stmts.Add(nextStmt.parseResult);
                nextPosition = nextStmt.nextPosition;
                token = tokens[nextPosition];
            }

            return new ParseResult<Stmt>(new BlockStmt(stmts.ToArray()), nextPosition);
        }

        public ParseResult<MethodDef> ParseMethodDef(int startposition)
        {
            Identifier methodName;
            Type returnType;
            Stmt[] parameters;
            ParseResult<Stmt> methodBody;

            int currentPosition = startposition;
            Token currentToken = tokens[currentPosition];

            // Must start with `method`
            if (currentToken.Type != TokenType.MethodToken)
            {
                throw new ParseException("Invalid start of Method Definition");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            // Must be followed with an Identifier
            if (currentToken.Type != TokenType.Identifier)
            {
                throw new ParseException("Method Name is not an Identifier");
            }

            methodName = new Identifier(currentToken.Lexeme);

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.OpenParenthesisToken)
            {
                throw new ParseException("Missing Open Parenthesis for Method Definition");
            }

            ParseResult<Stmt[]> parameterVarDecs = ParseCommaVarDec(currentPosition + 1);
            parameters = parameterVarDecs.parseResult;
            currentPosition = parameterVarDecs.nextPosition;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.CloseParenthesisToken)
            {
                throw new ParseException("Missing Closing Parenthesis not found for Method Definition");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (!canBeType(currentToken))
            {
                throw new ParseException("Invalid Method Type");
            }

            returnType = tokenToDataType(currentToken);

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.OpenBracketToken)
            {
                throw new ParseException("Missing Curly Brackets for Method Definition");
            }

            methodBody = ParseBlock(currentPosition + 1);

            return new ParseResult<MethodDef>(new Method(methodName, returnType, parameters, methodBody.parseResult), methodBody.nextPosition);
        }

        public ParseResult<Constructor> ParseConstructor(int startPosition)
        {
            Stmt[] parameters;
            bool callsSuper = false;
            Exp[]? superParameters = null;
            ParseResult<Stmt> constructorBody;

            int currentPosition = startPosition;
            Token currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.InitToken)
            {
                throw new ParseException("Invalid Start of Constructor");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.OpenParenthesisToken)
            {
                throw new ParseException("Missing Open Parenthesis for Constructor");
            }

            ParseResult<Stmt[]> parameterVarDecs = ParseCommaVarDec(currentPosition + 1);
            parameters = parameterVarDecs.parseResult;
            currentPosition = parameterVarDecs.nextPosition;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.CloseParenthesisToken)
            {
                throw new ParseException("Missing Closing Parenthesis not found for Constructor");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.OpenBracketToken)
            {
                throw new ParseException("Missing Curly Brackets for Constructor");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type == TokenType.SuperToken)
            {
                callsSuper = true;
                currentPosition++;
                currentToken = tokens[currentPosition];

                if (currentToken.Type != TokenType.OpenParenthesisToken)
                {
                    throw new ParseException("Missing Open Parenthesis for Super");
                }

                ParseResult<Exp[]> superParameterExps = ParseCommaExp(currentPosition + 1);
                superParameters = superParameterExps.parseResult;
                currentPosition = superParameterExps.nextPosition;
                currentToken = tokens[currentPosition];

                if (currentToken.Type != TokenType.CloseParenthesisToken)
                {
                    throw new ParseException("Missing Closing Parenthesis not found for Super");
                }
                if (currentPosition + 1 >= tokens.Count || tokens[currentPosition + 1].Type != TokenType.SemicolonToken)
                {
                    throw new ParseException("Missing Semicolon for Super");
                }

                currentPosition += 2;
                currentToken = tokens[currentPosition];
            }

            constructorBody = ParseBlock(currentPosition);

            if (callsSuper)
            {
                return new ParseResult<Constructor>(new Con(parameters, superParameters, constructorBody.parseResult), constructorBody.nextPosition);
            }

            return new ParseResult<Constructor>(new Con(parameters, constructorBody.parseResult), constructorBody.nextPosition);
        }

        public ParseResult<ClassDef> ParseClassDef(int startPosition)
        {
            Identifier className;
            Identifier? extendedClassName = null;
            List<Stmt> localVarDecs = new List<Stmt>();
            ParseResult<Constructor> constructor;
            List<MethodDef> methodDefs = new List<MethodDef>();

            int currentPosition = startPosition;
            Token currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.ClassToken)
            {
                throw new ParseException("Invalid Start of Class Definition");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.Identifier)
            {
                throw new ParseException("Invalid Class Definition Identifier");
            }

            className = new Identifier(currentToken.Lexeme);
            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type == TokenType.ExtendsToken)
            {
                currentPosition++;
                currentToken = tokens[currentPosition];

                if (currentToken.Type != TokenType.Identifier)
                {
                    throw new ParseException("Invalid Extended Class Identifier");
                }

                extendedClassName = new Identifier(currentToken.Lexeme);

                currentPosition++;
                currentToken = tokens[currentPosition];
            }

            if (currentToken.Type != TokenType.OpenBracketToken)
            {
                throw new ParseException("Missing Opening Curly Bracket for Class Definition");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            while (canBeType(currentToken))
            {
                if (currentPosition + 2 >= tokens.Count || tokens[currentPosition + 2].Type != TokenType.SemicolonToken)
                {
                    throw new ParseException("Missing Semicolon in Class Definition");
                }

                ParseResult<Stmt> varDec = ParseVarDecStmt(currentPosition);
                localVarDecs.Add(varDec.parseResult);
                currentPosition += 3;
                currentToken = tokens[currentPosition];
            }

            if (currentToken.Type != TokenType.InitToken)
            {
                throw new ParseException("Missing Constructor in Class Definition");
            }

            constructor = ParseConstructor(currentPosition);
            currentPosition = constructor.nextPosition;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.ClosedBracketToken)
            {
                throw new ParseException("Missing Closing Curly Bracket for Constructor");
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            while (currentToken.Type == TokenType.MethodToken)
            {
                ParseResult<MethodDef> methodDef = ParseMethodDef(currentPosition);
                methodDefs.Add(methodDef.parseResult);
                currentPosition = methodDef.nextPosition;
                currentToken = tokens[currentPosition];
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.ClosedBracketToken)
            {
                throw new ParseException($"Missing Closing Curly Bracket for Class Definition");
            }

            return new ParseResult<ClassDef>(new Class(className, extendedClassName, localVarDecs.ToArray(), constructor.parseResult, methodDefs.ToArray()), currentPosition + 1);
        }

        public ParseResult<Exp> ParseExp(int startPosition)
        {
            ParseResult<Exp> left = ParseMultExp(startPosition);
            List<Exp> MultExps = new List<Exp>();
            List<Op> Ops = new List<Op>();
            int nextPosition = left.nextPosition;
            if (left.nextPosition < tokens.Count)
            {
                Token token = tokens[nextPosition];
                while (token.Type == TokenType.AddToken || token.Type == TokenType.SubToken)
                {
                    ParseResult<Op> op = ParseOp(nextPosition);
                    ParseResult<Exp> right = ParseMultExp(op.nextPosition);
                    nextPosition = right.nextPosition;
                    token = tokens[nextPosition];
                    MultExps.Add(right.parseResult);
                    Ops.Add(op.parseResult);
                }
                
               
            }
            if (Ops.Count > 0){
                BinopExp finalExp = new BinopExp(left.parseResult, Ops[0], MultExps[0]);
                for (int i = 1; i < Ops.Count; i++)
                    {
                        finalExp = new BinopExp(finalExp, Ops[i], MultExps[i]);
                    }
                return new ParseResult<Exp>(finalExp, nextPosition);
            }
                    
            return new ParseResult<Exp>(left.parseResult, left.nextPosition);

        }

        public ParseResult<Exp> ParseMultExp(int startPosition)
        {
           ParseResult<Exp> left = ParseCallExp(startPosition);
            List<Exp> CallExps = new List<Exp>();
            List<Op> Ops = new List<Op>();
            int nextPosition = left.nextPosition;
            if (left.nextPosition < tokens.Count)
            {
                Token token = tokens[nextPosition];
                while (token.Type == TokenType.MultToken || token.Type == TokenType.DivToken)
                {
                    ParseResult<Op> op = ParseOp(nextPosition);
                    ParseResult<Exp> right = ParseCallExp(op.nextPosition);
                    nextPosition = right.nextPosition;

                    if (nextPosition >= tokens.Count)
                    {
                        break;
                    }

                    token = tokens[nextPosition];
                    CallExps.Add(right.parseResult);
                    Ops.Add(op.parseResult);
                }
            }
            if (Ops.Count > 0)
            {
                BinopExp finalExp = new BinopExp(left.parseResult, Ops[0], CallExps[0]);
                for (int i = 1; i < Ops.Count; i++)
                {
                    finalExp = new BinopExp(finalExp, Ops[i], CallExps[i]);
                }
                return new ParseResult<Exp>(finalExp, nextPosition);
            }
                    
            return new ParseResult<Exp>(left.parseResult, left.nextPosition);
        }

        public ParseResult<Exp> ParseCallExp(int startPosition)
        {
           ParseResult<Exp> left = ParsePrimaryExp(startPosition);
            List<Exp> MethodCallsExps = new List<Exp>();
            int nextPosition = left.nextPosition;
            if (left.nextPosition < tokens.Count)
            {
                Token token = tokens[nextPosition];
                while (token.Type == TokenType.PeriodToken)
                {
                    ++nextPosition;
                    ParseResult<Exp> right = ParseMethodName(nextPosition);
                    nextPosition = right.nextPosition;
                    token = tokens[nextPosition];

                    if (token.Type == TokenType.OpenParenthesisToken)
                    {
                        ParseResult<Exp[]> ParameterExp = ParseCommaExp(nextPosition + 1);
                        if (tokens[ParameterExp.nextPosition].Type == TokenType.CloseParenthesisToken)
                        {
                            MethodCallsExps.Add(new MethodCall(right.parseResult, ParameterExp.parseResult.ToArray()));
                            nextPosition = ParameterExp.nextPosition + 1;
                            token = tokens[nextPosition];
                        }    
                    }
                    else
                    {
                        MethodCallsExps.Add(right.parseResult);
                        nextPosition = right.nextPosition;
                        token = tokens[nextPosition];
                    }
                }
            }
            if (MethodCallsExps.Count > 0){
                BinopExp finalExp = new BinopExp(left.parseResult, new PeriodOp(), MethodCallsExps[0]);
                for (int i = 1; i < MethodCallsExps.Count; i++)
                {
                    finalExp = new BinopExp(finalExp, new PeriodOp(), MethodCallsExps[i]);
                }
                return new ParseResult<Exp>(finalExp, nextPosition);
            }
                    
            return new ParseResult<Exp>(left.parseResult, left.nextPosition); 
        }

        public ParseResult<Exp> ParseMethodName(int startPosition)
        {
            Token targetToken = tokens[startPosition];

            if (targetToken.Type != TokenType.Identifier)
            {
                throw new ParseException("Method Name Expected");
            }

            return new ParseResult<Exp>(new Identifier(targetToken.Lexeme), startPosition + 1);
        }

        public ParseResult<Exp[]> ParseCommaExp(int startPosition)
        {
            List<Exp> exps = new List<Exp>();
            int nextPosition = startPosition;

            if (tokens[startPosition].Type != TokenType.CloseParenthesisToken)
            {
                ParseResult<Exp> left = ParseExp(startPosition);
                exps.Add(left.parseResult);
                nextPosition = left.nextPosition;
                Token token = tokens[nextPosition];
                while (token.Type == TokenType.CommaToken)
                {
                    ParseResult<Exp> right = ParseExp(nextPosition + 1);
                    exps.Add(right.parseResult);
                    nextPosition = right.nextPosition;
                    token = tokens[nextPosition];
                }
            }
            
            return new ParseResult<Exp[]>(exps.ToArray(), nextPosition);
        }
        public ParseResult<Exp> ParsePrimaryExp(int startPosition){
            Token token = tokens[startPosition];
            switch (token.Type)
            {
                case TokenType.IntegerLiteral:
                    return new ParseResult<Exp>(new IntegerExp(int.Parse(token.Lexeme)), startPosition + 1);
                case TokenType.TrueToken:
                    return new ParseResult<Exp>(new TrueExp(), startPosition + 1);
                case TokenType.FalseToken:
                    return new ParseResult<Exp>(new FalseExp(), startPosition + 1);
                case TokenType.ThisToken:
                    return new ParseResult<Exp>(new ThisExp(), startPosition + 1);
                case TokenType.Identifier:
                    return new ParseResult<Exp>(new Identifier(token.Lexeme), startPosition + 1);
                case TokenType.NewToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 2].Type == TokenType.OpenParenthesisToken)
                        {
                            if (tokens[startPosition + 3].Type == TokenType.CloseParenthesisToken)
                                return new ParseResult<Exp>(new NewExp(new Identifier(tokens[startPosition + 1].Lexeme)), startPosition + 4);
                            else
                                throw new ParseException("Missing Close Parenthesis");
                        }
                        else
                            throw new ParseException("Missing Open Parenthesis");
                    }
                    else
                        throw new ParseException("Identifier expected");
                case TokenType.OpenParenthesisToken:
                    ParseResult<Exp> OpenParenthesisExp = ParseExp(startPosition + 1);
                    if (tokens[OpenParenthesisExp.nextPosition].Type == TokenType.CloseParenthesisToken)
                        return new ParseResult<Exp>(OpenParenthesisExp.parseResult, OpenParenthesisExp.nextPosition + 1);
                    else
                        throw new ParseException("Missing Close Parenthesis");
                case TokenType.PrintlnToken:
                    if (tokens[startPosition + 1].Type == TokenType.OpenParenthesisToken)
                    {
                        ParseResult<Exp> PrintlnExp = ParseExp(startPosition + 2);
                        if (tokens[PrintlnExp.nextPosition].Type == TokenType.CloseParenthesisToken)
                            return new ParseResult<Exp>(new Println(PrintlnExp.parseResult), PrintlnExp.nextPosition + 1);
                        else
                            throw new ParseException("Missing Close Parenthesis");
                    }
                    else
                        throw new ParseException("Missing Open Parenthesis");
                default:
                    throw new ParseException("Failed to Parse Primary Expression.");
            }
        }    

    }

    
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
            Console.WriteLine("Failed to Parse.");
        }
    }
}