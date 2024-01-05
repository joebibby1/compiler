using Xunit;
using Parse;
using Lex;

namespace Compiler.Tests;

public class ResolverTests
{

    [Fact]
    public void ResolveVariableDeclrWithInitializer_DefinesVar()
    {
        var tokens = new Lexer("var x = 1;").ScanTokens();
        var statements = new Parser(tokens).Parse();
        var resolver = new Resolver();

        foreach (var statement in statements)
        {
            statement.Resolve(resolver);
        }

        var global = resolver.Scopes.Peek();
        // check that the variable is declared and defined in the global scope
        Assert.True(global["x"]);
    }

    [Fact]
    public void ResolveVariableExpr_AssignsScopeDistanceToVar()
    {
        var input = @"
            var x = 1;
            {{x;}}
            ";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();
        var resolver = new Resolver();

        foreach (var statement in statements)
        {
            statement.Resolve(resolver);
        }

        var block = statements[1];
        ExprStmt exprStatement = (ExprStmt)((BlockStmt)((BlockStmt)block).Statements[0]).Statements[0];
        VarExpr varExpr = (VarExpr)exprStatement.expr;
        // check that the scope distance between definition and evaluation is correct
        Assert.Equal(2, varExpr.ScopeDistance);
    }

    // resolves variable assignment expr
    [Fact]
    public void ResolveVariableAssignmentExpr_AssignsScopeDistanceToVar()
    {
        var input = @"
            var x = 1;
            {{x = 2;}}
            ";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();
        var resolver = new Resolver();

        foreach (var statement in statements)
        {
            statement.Resolve(resolver);
        }

        var block = statements[1];
        ExprStmt exprStatement = (ExprStmt)((BlockStmt)((BlockStmt)block).Statements[0]).Statements[0];
        VarAssignExpr assignExpr = (VarAssignExpr)exprStatement.expr;
        // check that the scope distance between definition and evaluation is correct
        Assert.Equal(2, assignExpr.ScopeDistance);
    }

    // resolves function declr
    [Fact]
    public void ResolveFunctionDeclr_DefinesFunc()
    {
        var input = @"
            func foo() {
                print 1;
            }
            ";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();
        var resolver = new Resolver();

        foreach (var statement in statements)
        {
            statement.Resolve(resolver);
        }

        var global = resolver.Scopes.Peek();
        // check that the function is declared and defined in the global scope
        Assert.True(global["foo"]);
    }

    // resolves function expr
    [Fact]
    public void ResolveFunctionExpr_AssignsScopeDistanceToCallee()
    {
        var input = @"
            func foo() {
                print 1;
            }
            {{foo();}}
            ";
        var tokens = new Lexer(input).ScanTokens();
        var statements = new Parser(tokens).Parse();
        var resolver = new Resolver();

        foreach (var statement in statements)
        {
            statement.Resolve(resolver);
        }

        var block = statements[1];
        ExprStmt exprStatement = (ExprStmt)((BlockStmt)((BlockStmt)block).Statements[0]).Statements[0];
        CallExpr callExpr = (CallExpr)exprStatement.expr;
        VarExpr callee = (VarExpr)callExpr.callee;
        // check that the scope distance between definition and evaluation is correct
        Assert.Equal(2, callee.ScopeDistance);
    }

    // arguments to a func are resolved
    // "this" is resolved
    // "super" is resolved
}