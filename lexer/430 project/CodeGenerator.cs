namespace Lang.CodeGenerator
{
    using Lang.Parser;
    using System;

    public sealed class CodeGenerator
    {
        Parser.ClassDef[] classDefs;
        Parser.Stmt[] stmts;

        public CodeGenerator(Parser.Code program) 
        {
            this.classDefs = program.classDefs;
            this.stmts = program.stmts;
        }

        public string GenerateCode()
        {
            string code = "";

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

            return code;
        }

        private string GenerateStmt(Parser.Stmt stmt)
        {
            throw new NotImplementedException();
        }

        private string GenerateClassDef(Parser.ClassDef classDef)
        {
            throw new NotImplementedException();
        }
    }
}