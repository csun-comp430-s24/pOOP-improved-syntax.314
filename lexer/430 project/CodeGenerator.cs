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
        public string GenerateCode()
        {
            foreach (Parser.ClassDef classDef in classDefs)
            {
                code += GenerateClassDef((Parser.Class)classDef);
                code += "\n";
            }

            foreach (Parser.Stmt stmt in stmts)
            {
                code += $"{GenerateStmt(stmt)}\n";
            }

            return code;
        }
        public string GenerateClassDef(Parser.Class classdef)
        {
            string? parentClassName = (classdef.extendingClassName != null) ? classdef.extendingClassName.value : null;
            string className = classdef.className.value;
            Parser.Stmt[] localVarDecs = classdef.localVarDecs;

            string ClassDefCode = $"{GenerateConstructor((Parser.Con)(classdef.constructor), localVarDecs, className, parentClassName)}";
            
            foreach(Parser.MethodDef methodDef in classdef.classMethods)
            {
                ClassDefCode += GenerateMethodDef((Parser.Method)methodDef, className);
            }

            return ClassDefCode;
        }
        private string GenerateCommaStmt(Parser.Stmt[] stmts)
        {
            string commaStmtString = "";

            if (stmts.Length > 0)
            {
                commaStmtString += $"{GenerateStmt(stmts[0])}";

                for (int i = 1; i < stmts.Length; i++)
                {
                    commaStmtString += $", {GenerateStmt(stmts[i])}";
                }
            }

            return commaStmtString;
        }

        private string GenerateCommaExp(Parser.Exp[] exps)
        {
            string commaExpString = "";

            if (exps.Length > 0)
            {
                commaExpString += $"{GenerateExp(exps[0])}";

                for (int i = 1; i < exps.Length; i++)
                {
                    commaExpString += $", {GenerateExp(exps[i])}";
                }
            }

            return commaExpString;
        }

        private string GenerateConstructor(Parser.Con constructor, Parser.Stmt[] localVarDecs, string className, string? parentClassName = null)
        {
            string constructorCode = $"function {className}";

            constructorCode += $"({GenerateCommaStmt(constructor.parameters)}) {{";

            if (constructor.callsSuper)
            {
                constructorCode += $"{parentClassName}.call(this);\n";
            }

            constructorCode += $"{GenerateStmt(new Parser.BlockStmt(localVarDecs))}\n\n{GenerateStmt(constructor.constructorBody)}}}\n";

            if (constructor.callsSuper)
            {
                constructorCode += $"Object.setPrototypeOf({className}.prototype, {parentClassName}.prototype);\n";
            }

            return constructorCode;
        }

        private string GenerateMethodDef(Parser.Method methodDef, string className)
        {
            string methodDefCode = $"{className}.prototype.{methodDef.methodName.value} = function(";

            methodDefCode += GenerateCommaStmt(methodDef.parameters);

            return methodDefCode + $") {GenerateStmt(methodDef.methodBody)};";
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

            return "";
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

            callCode += GenerateCommaExp(call.Parameters);

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
