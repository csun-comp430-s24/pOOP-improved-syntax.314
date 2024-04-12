using Lang.Lexer;

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

        public class IntType : Type { }
        public class BooleanType : Type { }
        
        public class VoidType : Type { }
        public class CLassNameType : Type { }

        public class VarType : Vardec { }

        public class VarDecStmt : Stmt
        {
            public TokenType varType;
            public string varIdentifier;

            public VarDecStmt(TokenType varType, string varIdentifier)
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
            public Stmt elseBody;
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
                    return e.Condition.Equals(Condition) && e.ifBody.Equals(ifBody) && e.elseBody.Equals(elseBody);
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
            public MethodDef left;
            public MethodDef mleft;
            public MethodDef mid;
            public MethodDef mright;
            public MethodDef[] right;
            public Method(MethodDef left, MethodDef mleft, MethodDef mid, MethodDef mright, MethodDef[] right)
            {
                this.left = left;
                this.mleft = mleft;
                this.mid = mid;
                this.mright = mright;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is Method)
                {
                    Method e = (Method)other;
                    return e.left.Equals(left) && e.mleft.Equals(mleft) && e.mid.Equals(mid) && e.mright.Equals(mright) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }

        public class Con : Constructor
        {
            public Constructor left;
            public Constructor mleft;
            public Constructor mid;
            public Constructor mright;
            public Constructor[] right;
            public Con(Constructor left, Constructor mleft, Constructor mid, Constructor mright, Constructor[] right)
            {
                this.left = left;
                this.mleft = mleft;
                this.mid = mid;
                this.mright = mright;
                this.right = right;
            }
            public Boolean Equals(object other)
            {
                if (other is Con)
                {
                    Con e = (Con)other;
                    return e.left.Equals(left) && e.mleft.Equals(mleft) && e.mid.Equals(mid) && e.mright.Equals(mright) && e.right.Equals(right);
                }
                else
                {
                    return false;
                }
            }
        }

        public class Class : ClassDef
        {
            public ClassDef left;
            public ClassDef Identifier1;
            public ClassDef Extender;
            public ClassDef Identifier2;
            public ClassDef Vardec;
            public ClassDef Constructor;
            public ClassDef[] MethodDef;
            public Class(ClassDef left, ClassDef identifier1, ClassDef extender, ClassDef identifier2, ClassDef vardec, ClassDef constructor, ClassDef[] methodDef)
            {
                this.left = left;
                this.Identifier1 = identifier1;
                this.Extender = extender;
                this.Identifier2 = identifier2;
                this.Vardec = vardec;
                this.Constructor = constructor;
                this.MethodDef = methodDef;
            }
            public Boolean Equals(object other)
            {
                if (other is Class)
                {
                    Class e = (Class)other;
                    return e.left.Equals(left) && e.Identifier1.Equals(Identifier1) && e.Extender.Equals(Extender) && e.Identifier2.Equals(Identifier2) && e.Constructor.Equals(Constructor) && e.MethodDef.Equals(MethodDef);
                }
                else
                {
                    return false;
                }
            }
        }

        public class Code : Program
        {
            public Program[] classdef;
            public Program[] stmts;
            public Code(Program[] classdef, Program[] stmts)
            {
                this.classdef = classdef;
                this.stmts = stmts;
            }
            public Boolean Equals(object other)
            {
                if (other is Code)
                {
                    Code e = (Code)other;
                    return e.classdef.Equals(classdef) && e.stmts.Equals(stmts);
                }
                else
                {
                    return false;
                }
            }
        }

        public void ParseTokens()
        {
            int position = 0;

            while (position < tokens.Count)
            {
                break;
            }
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
            switch (token.Type)
            {
                case TokenType.IntToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 2].Type == TokenType.SemicolonToken)
                            return new ParseResult<Stmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    else
                        throw new ParseException("Identifier expected");

                case TokenType.BooleanToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 2].Type == TokenType.SemicolonToken)
                            return new ParseResult<Stmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    throw new ParseException("Identifier expected");

                case TokenType.VoidToken:
                    if (tokens[startPosition + 1].Type == TokenType.Identifier)
                    {
                        if (tokens[startPosition + 2].Type == TokenType.SemicolonToken)
                            return new ParseResult<Stmt>(new VarDecStmt(token.Type, tokens[startPosition + 1].Lexeme), startPosition + 3);
                        else
                            throw new ParseException("Missing Semicolon");
                    }
                    throw new ParseException("Identifier expected");
                default:
                    throw new ParseException("Defaulted in ParseVarDecStmt");
                    
            }
        }

         public ParseResult<Stmt[]> ParseVarDecComma(int startPosition) //FIXME
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
                    return ParseVarDecStmt(startPosition);

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
                    ParseResult<Stmt[]> block = ParseBlock(startPosition + 1);
                    if (tokens[block.nextPosition].Type == TokenType.ClosedBracketToken)
                        return new ParseResult<Stmt>(new BlockStmt(block.parseResult), block.nextPosition + 1);
                    else
                        throw new ParseException("Missing Closed Bracket");
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


        public ParseResult<Stmt[]> ParseBlock(int startPosition)
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
            return new ParseResult<Stmt[]>(stmts.ToArray(), nextPosition);
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