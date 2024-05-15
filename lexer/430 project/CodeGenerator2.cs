using System;
using System.Text;
using Lang.Parser;
using static Lang.Parser.Parser;

namespace Lang.CodeGenerator
{
    public class CodeGenerator2
    {
        public string GenerateCode(Code program)
        {
            var sb = new StringBuilder();
            if (program is Code code)
            {
                foreach (var classDef in code.classDefs)
                {
                    sb.AppendLine(GenerateClass(classDef));
                }
                foreach (var stmt in code.stmts)
                {
                    sb.AppendLine(GenerateStatement(stmt));
                }
            }
            return sb.ToString();
        }

        private string GenerateClass(ClassDef classDef)
        {
            if (classDef is Class cls)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"class {cls.className.value} {(cls.extendingClassName != null ? $"extends {cls.extendingClassName.value} " : "")}{{");

                // Generate constructor
                sb.AppendLine(GenerateConstructor(cls.constructor));

                // Generate methods
                foreach (var method in cls.classMethods)
                {
                    sb.AppendLine(GenerateMethod(method));
                }

                sb.AppendLine("}");
                return sb.ToString();
            }
            throw new ArgumentException("Invalid ClassDef node");
        }

        private string GenerateConstructor(Constructor constructor)
        {
            if (constructor is Con con)
            {
                var sb = new StringBuilder();
                sb.Append("constructor(");
                for (int i = 0; i < con.parameters.Length; i++)
                {
                    if (con.parameters[i] is VarDecStmt varDecStmt)
                    {
                        sb.Append(varDecStmt.varIdentifier.value);
                        if (i < con.parameters.Length - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                }
                sb.AppendLine(") {");

                if (con.callsSuper)
                {
                    sb.Append("super(");
                    for (int i = 0; i < con.superParameters.Length; i++)
                    {
                        sb.Append(GenerateExpression(con.superParameters[i]));
                        if (i < con.superParameters.Length - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    sb.AppendLine(");");
                }

                foreach (var stmt in ((BlockStmt)con.constructorBody).Block)
                {
                    sb.AppendLine(GenerateStatement(stmt));
                }

                sb.AppendLine("}");
                return sb.ToString();
            }
            throw new ArgumentException("Invalid Constructor node");
        }

        private string GenerateMethod(MethodDef methodDef)
        {
            if (methodDef is Method method)
            {
                var sb = new StringBuilder();
                sb.Append($"function {method.methodName.value}(");
                for (int i = 0; i < method.parameters.Length; i++)
                {
                    if (method.parameters[i] is VarDecStmt varDecStmt)
                    {
                        sb.Append(varDecStmt.varIdentifier.value);
                        if (i < method.parameters.Length - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                }
                sb.AppendLine(") {");

                foreach (var stmt in ((BlockStmt)method.methodBody).Block)
                {
                    sb.AppendLine(GenerateStatement(stmt));
                }

                sb.AppendLine("}");
                return sb.ToString();
            }
            throw new ArgumentException("Invalid MethodDef node");
        }

        private string GenerateStatement(Stmt stmt)
        {
            switch (stmt)
            {
                case VarDecStmt varDecStmt:
                    return $"let {varDecStmt.varIdentifier.value};";
                case AssignmentStmt assignmentStmt:
                    return $"{assignmentStmt.variable.value} = {GenerateExpression(assignmentStmt.assignedExp)};";
                case ExpStmt expStmt:
                    return $"{GenerateExpression(expStmt.expression)};";
                case ReturnStmt returnStmt:
                    return returnStmt.left == null ? "return;" : $"return {GenerateExpression(returnStmt.left)};";
                case BreakStmt breakStmt:
                    return "break";
                case IfStmt ifStmt:
                    var elsePart = ifStmt.elseBody != null ? $" else {GenerateStatement(ifStmt.elseBody)}" : "";
                    return $"if ({GenerateExpression(ifStmt.Condition)}) {GenerateStatement(ifStmt.ifBody)}{elsePart}";
                case WhileStmt whileStmt:
                    return $"while ({GenerateExpression(whileStmt.Condition)}) {GenerateStatement(whileStmt.body)}";
                case BlockStmt blockStmt:
                    var sb = new StringBuilder();
                    sb.AppendLine("{");
                    foreach (var s in blockStmt.Block)
                    {
                        sb.AppendLine(GenerateStatement(s));
                    }
                    sb.AppendLine("}");
                    return sb.ToString();
                default:
                    throw new ArgumentException("Unknown statement type");
            }
        }
        private string GenerateExpression(Exp exp)
        {
            switch (exp)
            {
                case VarExp varExp:
                    return varExp.name;
                case IntegerExp integerExp:
                    return integerExp.value.ToString();
                case TrueExp:
                    return "true";
                case FalseExp:
                    return "false";
                case ThisExp:
                    return "this";
                case Println println:
                    return $"console.log({GenerateExpression(println.exp)})";
                case NewExp newExp:
                    return $"new {newExp.className.value}()";
                case MethodCall methodCall:
                    var parameters = string.Join(", ", Array.ConvertAll(methodCall.Parameters, GenerateExpression));
                    return $"{GenerateExpression(methodCall.Identifier)}({parameters})";
                case Identifier identifier:
                    return identifier.value;
                case BinopExp binopExp:
                    return $"{GenerateExpression(binopExp.left)} {GenerateOperator(binopExp.op)} {GenerateExpression(binopExp.right)}";
                default:
                    throw new ArgumentException("Unknown expression type");
            }
        }

        private string GenerateOperator(Op op)
        {
            switch (op)
            {
                case PlusOp:
                    return "+";
                case MinusOp:
                    return "-";
                case MultOp:
                    return "*";
                case DivOp:
                    return "/";
                case PeriodOp:
                    return ".";
                default:
                    throw new ArgumentException("Unknown operator type");
            }
        }
    }
}