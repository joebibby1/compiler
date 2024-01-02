namespace Lex;

public enum TokenType
{
    // primitives
    NUM,
    // operators
    PLUS, MINUS, MULT, DIV, EQUAL, EQUAL_EQUAL, BANG_EQUAL, GREATER, GREATER_EQUAL, LESS, LESS_EQUAL, BANG,
    // keywords
    PRINT, VAR, IF, ELSE, OR, AND, WHILE, FOR, TRUE, FALSE,
    // identifiers
    IDENTIFIER,
    // punctuation
    SEMICOLON, LEFT_BRACE, RIGHT_BRACE, LEFT_PAREN, RIGHT_PAREN, COMMA,
    EOF
}