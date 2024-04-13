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
        }

        public class StringExp : Exp
        {
            public string value;
            public StringExp(string value)
            {
                this.value = value;
            }
        }
        public class TrueExp : Exp { }
        public class FalseExp : Exp { }
        public class ThisExp : Exp { }
        public class Println : Exp
        {
            public Exp exp;
            public Println(Exp exp)
            {
                this.exp = exp;
            }
        }
        public class NewExp : Exp
        {
            public Exp left;
            public Exp center;
            public Exp right;
            public NewExp(Exp leftExp, Exp Identifier, Exp rightExp)
            {
                this.left = leftExp;
                this.center = Identifier;
                this.right = rightExp;
            }
            public Boolean Equals(object other)
            {
                if (other is NewExp)
                {
                    NewExp e = (NewExp)other;
                    return e.left.Equals(left) && e.center.Equals(center) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
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
            
            public Boolean Equals(object other)
            {
                if (other is MethodCall)
                {
                    MethodCall e = (MethodCall)other;
                    return e.Identifier.Equals(Identifier) && e.Parameters.Equals(Parameters);
                }
                else
                {
                    return false;
                }
            }
        }
        public class Identifier : Exp
        {
            public string value;
            public Identifier(string value)
            {
                this.value = value;
            }
            public Boolean Equals(object other)
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
            public String ToString()
            {
                return "IdentifierExp(" + value + ")";
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
            public Boolean Equals(object other)
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

        }
        public class CommaExp : Exp
        {
            public Exp left;
            public Exp right;
            public CommaExp(Exp left, Exp right)
            {
                this.left = left;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is CommaExp)
                {
                    CommaExp e = (CommaExp)other;
                    return e.left.Equals(left) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }

        public class PeriodOp : Op {   
           public Boolean Equals(object other)
            {
                return other is PeriodOp;
            }
            public String ToString()
            {
                return "PeriodOp()";
            } 
        }


        public class PlusOp : Op
        {
            public Boolean Equals(object other)
            {
                return other is PlusOp;
            }
            public String ToString()
            {
                return "PlusOp()";
            }
        }
        public class MinusOp : Op
        {
            public Boolean Equals(object other)
            {
                return other is MinusOp;
            }
            public String ToString()
            {
                return "MinusOp()";
            }
        }
        public class MultOp : Op
        {
            public Boolean Equals(object other)
            {
                return other is MultOp;
            }
            public String ToString()
            {
                return "MultOp()";
            }
        }
        public class DivOp : Op
        {
            public Boolean Equals(object other)
            {
                return other is DivOp;
            }
            public String ToString()
            {
                return "DivOp()";
            }
        }

        public class IntType : Type
        {
            public Boolean Equals(object other)
            {
                return other is IntType;
            }
            public String ToString()
            {
                return "IntType()";
            }
        }

        public class BooleanType : Type
        {
            public Boolean Equals(object other)
            {
                return other is BooleanType;
            }
            public String ToString()
            {
                return "BooleanType()";
            }
        }
        
        public class VoidType : Type
        {
            public Boolean Equals(object other)
            {
                return other is VoidType;
            }
            public String ToString()
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
            public string varIdentifier;

            public VarDecStmt(Type varType, string varIdentifier)
            {
                this.varType = varType;
                this.varIdentifier = varIdentifier;
            }
        }
        public class AssignmentStmt : Stmt
        {
            public Identifier left;
            public Exp right;
            public AssignmentStmt(Identifier left, Exp right)
            {
                this.left = left;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is AssignmentStmt)
                {
                    AssignmentStmt e = (AssignmentStmt)other;
                    return e.left.Equals(left) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
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
            
            public Boolean Equals(object other)
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
        }
        public class BreakStmt : Stmt { }
        public class ReturnStmt : Stmt
        {
            public Exp left;
            

            public ReturnStmt(Exp left)
            {
                this.left = left;
            }
            public Boolean Equals(object other)
            {
                if (other is ReturnStmt)
                {
                    ReturnStmt e = (ReturnStmt)other;
                    return e.left.Equals(left);
                }
                else
                {
                    return false;
                }
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
           
            public Boolean Equals(object other)
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
        }
        public class BlockStmt : Stmt
        {
            public Stmt[] Block;
            public BlockStmt(Stmt[] Block)
            {
                this.Block = Block;
            }
            public Boolean Equals(object other)
            {
                if (other is BlockStmt)
                {
                    BlockStmt e = (BlockStmt)other;
                    return e.Block.Equals(Block);
                }
                else
                {
                    return false;
                }
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
            public Boolean Equals(object other)
            {
                if (other is Method)
                {
                    Method e = (Method)other;
                    return e.methodName.Equals(methodName) && e.returnType.Equals(returnType) && e.parameters.Equals(parameters) && e.methodBody.Equals(methodBody);
                }
                else
                {
                    return false;
                }
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

            public Boolean Equals(object other)
            {
                if (other is Con)
                {
                    Con e = (Con)other;
                    return e.parameters.Equals(parameters) && e.constructorBody.Equals(constructorBody) && e.callsSuper.Equals(callsSuper) && (callsSuper && ((superParameters == null && e.superParameters == null) || e.superParameters != null && e.superParameters.Equals(superParameters)));
                }
                else
                {
                    return false;
                }
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

            public Boolean Equals(object other)
            {
                if (other is Class)
                {
                    Class e = (Class)other;
                    return e.className.Equals(className) && ((extendingClassName == null && e.extendingClassName == null) || (e.extendingClassName != null && e.extendingClassName.Equals(extendingClassName))) && e.localVarDecs.Equals(localVarDecs) && e.constructor.Equals(constructor) && e.classMethods.Equals(classMethods);
                }
                else
                {
                    return false;
                }
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
            public Boolean Equals(object other)
            {
                if (other is Code)
                {
                    Code e = (Code)other;
                    return e.classDefs.Equals(classDefs) && e.stmts.Equals(stmts);
                }
                else
                {
                    return false;
                }
            }
        }

        public ParseResult<Program> ParseProgram(int startPosition)//More Breakpoints!
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
                return new ParseResult<Stmt>(new VarDecStmt(tokenToDataType(token), tokens[startPosition + 1].Lexeme), startPosition + 2);
            }

            throw new ParseException("Variable type expected");
        }

        public ParseResult<Stmt[]> ParseCommaVarDec(int startPosition)
        {
            List<Stmt> varDecs = new List<Stmt>();
            ParseResult<Stmt> startVarDec = ParseVarDecStmt(startPosition);
            
            varDecs.Add(startVarDec.parseResult);

            int nextPosition = startVarDec.nextPosition;
            Token currentToken = tokens[nextPosition];

            while (currentToken.Type == TokenType.CommaToken)
            {
                ParseResult<Stmt> nextVarDec = ParseVarDecStmt(nextPosition + 1);
                varDecs.Add(nextVarDec.parseResult);
                nextPosition = nextVarDec.nextPosition;
                currentToken = tokens[nextPosition];
            }

            return new ParseResult<Stmt[]>(varDecs.ToArray(), nextPosition);
        }

        public ParseResult<Stmt> ParseBreakStmt(int startPosition)
        {
            Token token = tokens[startPosition];
                switch (token.Type)
            {
                case TokenType.BreakToken:
                    if (tokens[startPosition + 1].Type == TokenType.SemicolonToken)
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
            switch (token.Type)
            {
                case TokenType.ReturnToken:
                    if (tokens[startPosition + 1].Type != TokenType.SemicolonToken){
                        ParseResult<Exp> ReturnExp = ParseExp(startPosition + 1);
                        return new ParseResult<Stmt>(new ReturnStmt(ReturnExp.parseResult), startPosition + 2);
                    }
                    else
                        throw new ParseException("Missing Semicolon");
                default:
                    throw new ParseException("defaulted in ParseReturnStmt");
            }
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
                    if (tokens[varDec.nextPosition].Type == TokenType.SemicolonToken)
                    {
                        return new ParseResult<Stmt>(varDec.parseResult, varDec.nextPosition + 1);//now we pray
                    }
                    else
                        throw new ParseException("Missing Semicolon");
                case TokenType.Identifier:
                    if (tokens[startPosition + 1].Type == TokenType.EqualsToken)
                    {
                        ParseResult<Exp> exp = ParseExp(startPosition + 2);
                        if (tokens[exp.nextPosition].Type == TokenType.SemicolonToken)
                            return new ParseResult<Stmt>(new AssignmentStmt(new Identifier(token.Lexeme), exp.parseResult), exp.nextPosition + 1);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    else
                        throw new ParseException("Assignment Operator expected");
                case TokenType.BreakToken:
                    return ParseBreakStmt(startPosition);
                case TokenType.ReturnToken:
                    return ParseReturnStmt(startPosition);
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
                            if (tokens[stmt.nextPosition].Type == TokenType.ElseToken)
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
                
                default:
                    throw new ParseException("defaulted in ParseStmt");

            }
        }

        public ParseResult<Stmt> ParseBlock(int startPosition)
        {
            List<Stmt> stmts = new List<Stmt>();
            ParseResult<Stmt> stmt = ParseStmt(startPosition);
            stmts.Add(stmt.parseResult);
            int nextPosition = stmt.nextPosition;
            Token token = tokens[nextPosition];
            while (token.Type != TokenType.ClosedBracketToken)
            {
                ParseResult<Stmt> nextStmt = ParseStmt(nextPosition);
                stmts.Add(nextStmt.parseResult);
                nextPosition = nextStmt.nextPosition;
                token = tokens[nextPosition];
            }
            return new ParseResult<Stmt>(new BlockStmt(stmts.ToArray()), nextPosition + 1);
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

                if (currentToken.Type != TokenType.CloseParenthesisToken)
                {
                    throw new ParseException("Missing Closing Parenthesis not found for Super");
                }
                if (tokens[currentPosition + 1].Type != TokenType.SemicolonToken)
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
                if (tokens[currentPosition + 2].Type != TokenType.SemicolonToken)
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

            while (currentToken.Type == TokenType.MethodToken)
            {
                ParseResult<MethodDef> methodDef = ParseMethodDef(currentPosition);
                methodDefs.Add(methodDef.parseResult);
                currentPosition += methodDef.nextPosition;
                currentToken = tokens[currentPosition];
            }

            currentPosition++;
            currentToken = tokens[currentPosition];

            if (currentToken.Type != TokenType.ClosedBracketToken)
            {
                throw new ParseException("Missing Closing Curly Bracket for Class Definition");
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
                    ParseResult<Op> op = ParseOp(left.nextPosition);
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
                    ParseResult<Op> op = ParseOp(left.nextPosition);
                    ParseResult<Exp> right = ParseCallExp(op.nextPosition);
                    nextPosition = right.nextPosition;
                    token = tokens[nextPosition];
                    CallExps.Add(right.parseResult);
                    Ops.Add(op.parseResult);
                }
                
               
            }
            if (Ops.Count > 0){
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
                    ParseResult<Exp> right = ParsePrimaryExp(nextPosition);
                    nextPosition = right.nextPosition;
                    token = tokens[nextPosition];
                    if (token.Type == TokenType.OpenParenthesisToken)
                    {
                        ParseResult<Exp[]> ParameterExp = ParseCommaExp(nextPosition + 1); 
                        if (tokens[ParameterExp.nextPosition].Type == TokenType.CloseParenthesisToken)
                        {
                            MethodCallsExps.Add(new MethodCall(right.parseResult, ParameterExp.parseResult.ToArray()));
                            nextPosition = ParameterExp.nextPosition + 1;
                        }    
                    }
                    else
                    {
                        MethodCallsExps.Add(right.parseResult);
                        break;
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

        public ParseResult<Exp[]> ParseCommaExp(int startPosition)
        {
            List<Exp> exps = new List<Exp>();
            ParseResult<Exp> left = ParseExp(startPosition);
            exps.Add(left.parseResult);
            int nextPosition = left.nextPosition;
            Token token = tokens[nextPosition];
            while (token.Type == TokenType.CommaToken)
            {
                ParseResult<Exp> right = ParseExp(nextPosition + 1);
                exps.Add(right.parseResult);
                nextPosition = right.nextPosition;
                token = tokens[nextPosition];
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
                                return new ParseResult<Exp>(new NewExp(new Identifier(tokens[startPosition + 1].Lexeme), new Identifier(""), new Identifier("")), startPosition + 4);
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