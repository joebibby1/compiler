namespace Lex;

public enum TokenType
{
    // primitives
    NUM,
    // operators
    PLUS, MINUS, MULT, DIV, EQUAL, EQUAL_EQUAL, BANG_EQUAL,
    // keywords
    PRINT, VAR, IF, ELSE, OR, AND,
    // identifiers
    IDENTIFIER,
    // punctuation
    SEMICOLON, LEFT_BRACE, RIGHT_BRACE, LEFT_PAREN, RIGHT_PAREN,
    EOF
}