//using System;
//using System.Collections.Generic;
//using System.Net.NetworkInformation;
//using System.Reflection.Metadata;
//using System.Xml.Linq;
//using static Lang.Parser.Parser;

//public sealed class ReturnAnalysis
//{
//    private ReturnAnalysis()
//    {
//    }

//    public static readonly ReturnAnalysis NoReturn = new ReturnAnalysis();
//    public static readonly ReturnAnalysis MaybeReturns = new ReturnAnalysis();
//    public static readonly ReturnAnalysis DefinitelyReturns = new ReturnAnalysis();
//}

//public static class Typechecker
//{
//    public static Dictionary<Variable, (Type, int)> MakeEnv(IEnumerable<FormalArg> args)
//    {
//        var accum = new Dictionary<Variable, (Type, int)>();
//        foreach (var arg in args)
//        {
//            if (accum.ContainsKey(arg.theVar))
//            {
//                throw new TypeErrorException($"Duplicate variable name: {arg.theVar}");
//            }
//            accum[arg.theVar] = (arg.typ, 0);
//        }
//        return accum;
//    }

//    public static Dictionary<Variable, (Type, int)> MakeEnvWithScopeLevel(IEnumerable<FormalArg> args, int scopeLevel)
//    {
//        var env = MakeEnv(args);
//        return env.ToDictionary(kvp => kvp.Key, kvp => (kvp.Value.Item1, scopeLevel));
//    }

//    public static Dictionary<StructName, Dictionary<Variable, Type>> MakeStructMap(Program prog)
//    {
//        var accum = new Dictionary<StructName, Dictionary<Variable, Type>>();
//        foreach (var structDef in prog.structs)
//        {
//            var (name, args) = structDef;
//            if (accum.ContainsKey(name))
//            {
//                throw new TypeErrorException($"Struct redefined: {name}");
//            }
//            accum[name] = MakeEnv(args);
//        }
//        return accum;
//    }

//    public static Dictionary<FunctionName, (IEnumerable<Type>, Type)> MakeFunctionMap(Program prog)
//    {
//        var accum = new Dictionary<FunctionName, (IEnumerable<Type>, Type)>();
//        foreach (var func in prog.funcs)
//        {
//            var (returnType, name, args, _) = func;
//            if (accum.ContainsKey(name))
//            {
//                throw new TypeErrorException($"Duplicate function name: {name}");
//            }
//            accum[name] = (args.Select(arg => arg.typ), returnType);
//        }
//        return accum;
//    }

//    public static void TypecheckProgram(Program prog)
//    {
//        var functionMapping = MakeFunctionMap(prog);
//        var structMapping = MakeStructMap(prog);
//        foreach (var func in prog.funcs)
//        {
//            TypecheckFunc(func, functionMapping, structMapping);
//        }
//    }

//    private static void TypecheckFunc(
//        Func func,
//        Dictionary<FunctionName, (IEnumerable<Type>, Type)> funcs,
//        Dictionary<StructName, Dictionary<Variable, Type>> structs)
//    {
//        var (_, returnAnalysis) = Typecheck(
//            func.body,
//            0,
//            MakeEnvWithScopeLevel(func.args, 0),
//            funcs,
//            structs,
//            func.returnType);
//        if (returnAnalysis != ReturnAnalysis.DefinitelyReturns)
//        {
//            throw new TypeErrorException($"Function might not return: {func}");
//        }
//    }

//    private static (Dictionary<Variable, (Type, int)>, ReturnAnalysis) TypecheckStmts(
//        IEnumerable<Stmt> stmts,
//        int scopeLevel,
//        Dictionary<Variable, (Type, int)> env,
//        Dictionary<FunctionName, (IEnumerable<Type>, Type)> funcs,
//        Dictionary<StructName, Dictionary<Variable, Type>> structs,
//        Type returnType)
//    {
//        (Dictionary<Variable, (Type, int)> curEnv, ReturnAnalysis curReturn) = (env, ReturnAnalysis.NoReturn);
//        foreach (var stmt in stmts)
//        {
//            (curEnv, var stmtReturn) = Typecheck(stmt, scopeLevel, curEnv, funcs, structs, returnType);
//            curReturn = CombineReturnAnalysis(curReturn, stmtReturn);
//        }
//        return (curEnv, curReturn);
//    }

//    private static ReturnAnalysis CombineReturnAnalysis(ReturnAnalysis a, ReturnAnalysis b)
//    {
//        if (a == ReturnAnalysis.DefinitelyReturns || b == ReturnAnalysis.DefinitelyReturns)
//        {
//            return ReturnAnalysis.DefinitelyReturns;
//        }
//        if (a == ReturnAnalysis.MaybeReturns || b == ReturnAnalysis.MaybeReturns)
//        {
//            return ReturnAnalysis.MaybeReturns;
//        }
//        return ReturnAnalysis.NoReturn;
//    }

//    private static void AssertTypesSame(Type expected, Type received)
//    {
//        if (!expected.Equals(received))
//        {
//            throw new TypeErrorException($"Expected type: {expected}; received type: {received}");
//        }
//    }

//    private static (Dictionary<Variable, (Type, int)>, ReturnAnalysis) Typecheck(
//        Stmt stmt,
//        int scopeLevel,
//        Dictionary<Variable, (Type, int)> env,
//        Dictionary<FunctionName, (IEnumerable<Type>, Type)> funcs,
//        Dictionary<StructName, Dictionary<Variable, Type>> structs,
//        Type returnType)
//    {
//        switch (stmt)
//        {
//            case ReturnStmt returnStmt:
//                AssertTypesSame(returnType, TypeOf(returnStmt.exp, env, funcs, structs));
//                return (env, ReturnAnalysis.DefinitelyReturns);
//            // Add other statement cases here
//            default:
//                throw new NotImplementedException($"Typecheck for statement type {stmt.GetType().Name} not implemented yet.");
//        }
//    }

//    private static Type TypeOf(
//        Expr expr,
//        Dictionary<Variable, (Type, int)> env,
//        Dictionary<FunctionName, (IEnumerable<Type>, Type)> funcs,
//        Dictionary<StructName, Dictionary<Variable, Type>> structs)
//    {
//        // Implement TypeOf logic for expressions
//        throw new NotImplementedException($"TypeOf for expression type {expr.GetType().Name} not implemented yet.");
//    }
//}

//public class TypeErrorException : Exception
//{
//    public TypeErrorException(string message) : base(message)
//    {
//    }
//}

//// expectedType theVar = initializer;
//case VariableDeclarationStmt(expectedType, theVar, initializer):
//    {
//        var receivedType = typeof(initializer, env, funcs, structs);
//        assertTypesSame(expectedType, receivedType);
//        if (env.TryGetValue(theVar, out var existingEntry) && existingEntry.Item2 == scopeLevel)
//        {
//            throw new TypeErrorException($"Name in same scope: {theVar}");
//        }
//        else
//        {
//            env[theVar] = (receivedType, scopeLevel);
//            return (env, NoReturn);
//        }
//    }

//case BlockStmt(stmts):
//    {
//        var (_, returnAnalysis) = typecheckStmts(stmts, scopeLevel + 1, env, funcs, structs, returnType);
//        return (env, returnAnalysis);
//    }

//public Type typeof(
//    Exp exp,
//    Dictionary<Variable, (Type, int)> env,
//    Dictionary<FunctionName, (IEnumerable<Type>, Type)> funcs,
//    Dictionary<StructName, Dictionary<Variable, Type>> structs)
//{
//    switch (exp)
//    {
//        case IntegerLiteralExp(_):
//            return IntType;
//        case TrueExp:
//case FalseExp:
//    return BoolType;
//case VariableExp(theVar):
//    if (env.TryGetValue(theVar, out var typeAndLevel))
//    {
//        return typeAndLevel.Item1;
//    }
//    else
//    {
//        throw new TypeErrorException($"Variable not in scope: {theVar}");
//    }
//case BinopExp(left, op, right):
//    {
//        var leftType = typeof(left, env, funcs, structs);
//        var rightType = typeof(right, env, funcs, structs);
//        switch ((leftType, op, rightType))
//        {
//            case (IntType, LessThanOp, IntType):
//                return BoolType;
//            case (BoolType, AndOp, BoolType):
//                return BoolType;
//            case (IntType, PlusOp, IntType):
//                return IntType;
//            default:
//                throw new TypeErrorException($"Bad types: ({leftType}, {op}, {rightType})");
//        }
//    }
//case CallExp(name, parameters):
//    {
//        var actualParamTypes = parameters.Select(param => typeof(param, env, funcs, structs)).ToList();
//        if (funcs.TryGetValue(name, out var(expectedParamTypes, returnType)))
//        {
//            if (actualParamTypes.SequenceEqual(expectedParamTypes))
//            {
//                return returnType;
//            }
//            else
//            {
//                throw new TypeErrorException("Call had incorrect params");
//            }
//        }
//        else
//        {
//            throw new TypeErrorException($"No such function: {name}");
//        }
//    }
//case MakeStructExp(name, fields):
//    {
//        if (structs.TryGetValue(name, out var expectedFieldTypes))
//        {
//            var types = fields.Select(pair => (pair.Item1, typeof(pair.Item2, env, funcs, structs))).ToList();
//            var asMap = types.ToDictionary(t => t.Item1, t => t.Item2);
//            if (asMap.Count != types.Count)
//            {
//                throw new TypeErrorException("Duplicate names when creating struct");
//            }
//            if (!asMap.SequenceEqual(expectedFieldTypes))
//            {
//                throw new TypeErrorException("Something wrong with fields");
//            }
//            return StructType(name);
//        }
//        else
//        {
//            throw new TypeErrorException($"No such struct: {name}");
//        }
//    }
//case DotExp(exp, variable):
//    {
//        var structType = typeof(exp, env, funcs, structs);
//        if (structType is StructType(var name))
//        {
//            if (structs.TryGetValue(name, out var fields) && fields.TryGetValue(variable, out var fieldType))
//            {
//                return fieldType;
//            }
//            else
//            {
//                throw new TypeErrorException($"Field doesn't exist: {variable}");
//            }
//        }
//        else
//        {
//            throw new TypeErrorException($"Not a struct: {exp}");
//        }
//    }
//default:
//    throw new NotImplementedException($"Unhandled expression type: {exp.GetType().Name}");
//}
//}

//public class TypeErrorException : Exception
//{
//    public TypeErrorException(string msg) : base(msg)
//    {
//    }
//}

//public class DotExp
//{
//    public object typ { get; set; }

//    public object Eval(object exp)
//    {
//        object expType;
//        switch (exp)
//        {
//            case null:
//                throw new TypeErrorException("Struct doesn't exist: " + exp);
//            default:
//                throw new TypeErrorException("Struct doesn't exist: " + exp);
//        }
//        exp.typ = expType;
//        return expType;
//    }
//}

