namespace Lang.CodeGenerator
{
    using Lang.Lexer;
    using Lang.Parser;

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
            //foreach (Parser.ClassDef classDef in classDefs)
            //{
            //    code += GenerateClassDef(classDef);
            //    code += "\n";
            //}

            foreach (Parser.Stmt stmt in stmts)
            {
                code += $"{GenerateStmt(stmt)}\n";
            }

            return code;
        }

        private string GenerateExp(Parser.Exp expression)
        {
            if (expression is Parser.Identifier)
            {
                return GenerateIdentifierExp((Parser.Identifier)expression);
            }

            if (expression is Parser.IntegerExp)
            {
                return GenerateIntegerExp((Parser.IntegerExp)expression);
            }

            // No Data in TrueExp, FalseExp, or ThisExp
            // No need to make functions for them
            if (expression is Parser.TrueExp)
            {
                return "true";
            }

            if (expression is Parser.FalseExp)
            {
                return "false";
            }

            if (expression is Parser.ThisExp)
            {
                return "this";
            }

            if (expression is Parser.Println)
            {
                return GeneratePrintLnExp((Parser.Println)expression);
            }

            if (expression is Parser.NewExp)
            {
                return GenerateNewExp((Parser.NewExp)expression);
            }

            if (expression is Parser.MethodCall)
            {
                return GenerateMethodCall((Parser.MethodCall)expression);
            }

            if (expression is Parser.BinopExp)
            {
                return GenerateBinOpExp((Parser.BinopExp)expression);
            }

            return ""; // Will need to check for each kind of Exp
        }

        private string GenerateIdentifierExp(Parser.Identifier idExp)
        {
            return idExp.value;
        }

        private string GenerateIntegerExp(Parser.IntegerExp intExp)
        {
            return $"{intExp.value}";
        }

        private string GeneratePrintLnExp(Parser.Println printExp)
        {
            // console.log is the closest thing to a printLn();
            return $"console.log({GenerateExp(printExp.exp)})";
        }

        private string GenerateNewExp(Parser.NewExp newExp)
        {
            return $"new {GenerateExp(newExp.className)}()";
        }

        private string GenerateMethodCall(Parser.MethodCall call)
        {
            string callCode = $"{GenerateExp(call.Identifier)}(";

            if (call.Parameters.Length > 0)
            {
                callCode += $"{GenerateExp(call.Parameters[0])}";

                for (int i = 1; i < call.Parameters.Length; i++)
                {
                    callCode += $", {GenerateExp(call.Parameters[i])}";
                }
            }

            return callCode + ")";
        }

        private string GenerateBinOpExp(Parser.BinopExp binop)
        {
            string opString = "";

            if (binop.op is Parser.PlusOp)
            {
                opString = "+";
            }

            if (binop.op is Parser.MinusOp)
            {
                opString = "-";
            }

            if (binop.op is Parser.MultOp)
            {
                opString = "*";
            }

            if (binop.op is Parser.DivOp)
            {
                opString = "/";
            }
            
            // No Spaces or Parentheses if op is a period
            if (binop.op is Parser.PeriodOp)
            {
                return $"{GenerateExp(binop.left)}.{GenerateExp(binop.right)}";
            }

            return $"({GenerateExp(binop.left)}) {opString} ({GenerateExp(binop.right)})";
        }

        private string GenerateStmt(Parser.Stmt stmt)
        {
            if (stmt is Parser.VarDecStmt)
            {
                return GenerateVarDecStmt((Parser.VarDecStmt)stmt);
            }

            if (stmt is Parser.AssignmentStmt)
            {
                return GenerateAssignmentStmt((Parser.AssignmentStmt)stmt);
            }

            if (stmt is Parser.ExpStmt)
            {
                return GenerateExpStmt((Parser.ExpStmt)stmt);
            }

            if (stmt is Parser.WhileStmt)
            {
                return GenerateWhileStmt((Parser.WhileStmt)stmt);
            }

            if (stmt is Parser.BreakStmt)
            {
                // No data within a BreakStmt, no need to make a function for it
                return "break;";
            }

            if (stmt is Parser.ReturnStmt)
            {
                return GenerateReturnStmt((Parser.ReturnStmt)stmt);
            }

            if (stmt is Parser.IfStmt)
            {
                return GenerateIfStmt((Parser.IfStmt)stmt);
            }

            if (stmt is Parser.BlockStmt)
            {
                return GenerateBlockStmt((Parser.BlockStmt)stmt);
            }

            // Maybe throw an Exception instead of an Empty String?
            return "";
        }

        private string GenerateVarDecStmt(Parser.VarDecStmt varDec)
        {
            // JavaScript doesn't use types in variable declarations
            // Only Identitifier is needed
            return $"let {varDec.varIdentifier.value};";
        }

        private string GenerateAssignmentStmt(Parser.AssignmentStmt assignment)
        {
            return $"{assignment.variable.value} = {GenerateExp(assignment.assignedExp)};";
        }

        private string GenerateExpStmt(Parser.ExpStmt stmt)
        {
            return $"{GenerateExp(stmt.expression)};";
        }

        private string GenerateWhileStmt(Parser.WhileStmt stmt)
        {
            return $"while ({GenerateExp(stmt.Condition)})\n{GenerateStmt(stmt.body)}";
        }

        private string GenerateReturnStmt(Parser.ReturnStmt stmt)
        {
            if (stmt.left == null)
            {
                return "return;";
            }
            else
            {
                return $"return {GenerateExp(stmt.left)};";
            }
        }

        private string GenerateIfStmt(Parser.IfStmt stmt)
        {
            string ifCode = $"if ({GenerateExp(stmt.Condition)})\n{GenerateStmt(stmt.ifBody)}\n";

            if (stmt.elseBody != null)
            {
                ifCode += $"else\n{GenerateStmt(stmt.elseBody)}";
            }

            return ifCode;
        }

        private string GenerateBlockStmt(Parser.BlockStmt block)
        {
            string blockCode = "{\n";

            foreach (Parser.Stmt stmt in block.Block)
            {
                blockCode += $"\t{GenerateStmt(stmt)}\n";
            }

            return blockCode + "}\n";
        }

        private string GenerateClassDef(Parser.ClassDef classDef)
        {
            string classDefCode = "";

            return classDefCode;
        }
    }
}
