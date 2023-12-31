﻿using Lex;
using Parse;
using Interpret;


class Program
{
    static void Main(string[] args)
    {
        // var input = @"
        //         class Doughnut {
        //             cook() {
        //                 print 5;
        //             }
        //         }

        //         class BostonCream < Doughnut {
        //             cook() {
        //                 super.cook();
        //                 print 6;
        //             }
        //         }

        //         BostonCream().cook();
        //             ";
        var input = @"
            class Cake {
                taste() {
                    print this.flavor;
                }
            }

            var cake = Cake();
            cake.flavor = 6;
            cake.taste();
        ";
        var lexer = new Lexer(input);

        var tokens = lexer.ScanTokens();
        var statements = new Parser(tokens).Parse();
        var resolver = new Resolver();
        var env = new Env();
        foreach (var statement in statements)
        {
            statement.Resolve(resolver);
        }
        foreach (var statement in statements)
        {
            statement.Execute(env);
        }

    }
}
