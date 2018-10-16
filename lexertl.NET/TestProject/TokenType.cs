namespace TestProject
{
    public enum TokenType //Token_value
    {
        TT_ERROR = -1,
        TT_NUMBER = 2,
        TT_PLUS = 3,
        TT_MINUS = 4,
        TT_MULTIPLY = 5,
        TT_DIVIDE = 6,
        TT_ASSIGN = 7,                  // '='
        TT_ID = 8,
        TT_SEMICOLON = 9,               // ';' 
        TT_OPENING_PARENTHESIS = 10,    // '(' LP
        TT_CLOSING_PARENTHESIS = 11     // ')' RP
    };
}
