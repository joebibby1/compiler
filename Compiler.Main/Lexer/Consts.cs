namespace Lex;

public enum TokenType
{
    // primitives
    NUM,
    // operators
    PLUS, MINUS, MULT, DIV, EQUAL, EQUAL_EQUAL, BANG_EQUAL,
    // keywords
    PRINT, VAR,
    // identifiers
    IDENTIFIER,
    // punctuation
    SEMICOLON,
    EOF
}