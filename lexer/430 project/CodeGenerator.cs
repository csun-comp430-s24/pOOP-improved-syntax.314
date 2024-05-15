namespace Lang.CodeGenerator
{
    using Lang.Lexer;
    using Lang.Parser;
    using System;
    using System.Security.Cryptography.X509Certificates;
    using static Lang.Parser.Parser;
    public sealed class CodeGenerator
    {
        List<Token> tokens;
        Parser.ClassDef[] classDefs;
        Parser.Stmt[] stmts;
        string code = "";

        public CodeGenerator(Parser.Code program) 
        {
            this.classDefs = program.classDefs;
            this.stmts = program.stmts;
        }
        public string GenerateCode(int counter)
        {
            foreach (Parser.ClassDef classDef in classDefs)
            {
                code += GenerateClassDef(classDef);
                code += "\n";
            }
            foreach (Parser.Stmt stmt in stmts)
            {
                code += GenerateStmt(stmt);
                code += "\n";
            }
            //for (int i = 0; i < code.Length; i++)
            //{
            //    Token token = tokens[counter];
                
            //    switch (token.Type)
            //    {
            //        case TokenType.AddToken:
            //            code = code + "+";
            //            return code;
            //        case TokenType.SubToken:
            //            code = code + "-";
            //            return code;
            //        case TokenType.MultToken:
            //            code = code + "*";
            //            return code;
            //        case TokenType.DivToken:
            //            code = code + "/";
            //            return code;
            //        case TokenType.ThisToken:
            //            code = code + "this";
            //            return code;
            //        case TokenType.BooleanToken:
            //            code = code + "bool";
            //            return code;
            //        case TokenType.TrueToken:
            //            code = code + "true";
            //            return code;
            //        case TokenType.FalseToken:
            //            code = code + "false";
            //            return code;
            //        case TokenType.NewToken:
            //            code = code + "new";
            //            return code;
            //        case TokenType.WhileToken:
            //            code = code + "while";
            //            return code;
            //        case TokenType.BreakToken:
            //            code = code + "break";
            //            return code;
            //        case TokenType.ReturnToken:
            //            code = code + "return";
            //            return code;
            //        case TokenType.IfToken:
            //            code = code + "if";
            //            return code;
            //        case TokenType.ElseToken:
            //            code = code + "else";
            //            return code;
            //        case TokenType.SuperToken:
            //            code = code + "super";
            //            return code;
            //        case TokenType.PeriodToken:
            //            code = code + ".";
            //            return code;
            //        case TokenType.OpenBracketToken:
            //            code = code + "{";
            //            return code;
            //        case TokenType.ClosedBracketToken:
            //            code = code + "}";
            //            return code;
            //        case TokenType.AndToken:
            //            code = code + "&&";
            //            return code;
            //        case TokenType.CloseParenthesisToken:
            //            code = code + ")";
            //            return code;
            //        case TokenType.OpenParenthesisToken:
            //            code = code + "(";
            //            return code;
            //        case TokenType.CommaToken:
            //            code = code + ",";
            //            return code;
            //        case TokenType.DoubleEqualsToken:
            //            code = code + "==";
            //            return code;
            //        case TokenType.EqualsToken:
            //            code = code + "=";
            //            return code;
            //        case TokenType.NotEqualsToken:
            //            code = code + "!=";
            //            return code;
            //        case TokenType.OrToken:
            //            code = code + "||";
            //            return code;
            //        case TokenType.SemicolonToken:
            //            code = code + ";";
            //            return code;
            //        default:
            //            throw new ParseException("problem during generation.");
            //    }
            //}
            return code;
        }
        private string GenerateStmt(Parser.Stmt stmt)
        {
            if (stmts ==)
            throw new NotImplementedException();
        }

        private string GenerateClassDef(Parser.ClassDef classDef)
        {
            throw new NotImplementedException();
        }
    }
}
