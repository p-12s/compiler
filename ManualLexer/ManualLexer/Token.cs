namespace ManualLexer
{
    public enum TokenType
    {
        TT_END = 0,
        TT_ERROR,
        TT_NUMBER,
        TT_PLUS,
        TT_MINUS,
        TT_MULTIPLY,
        TT_DIVIDE,
        TT_ASSIGN,              // '='
        TT_ID,
        TT_SEMICOLON,           // ';'
        TT_OPENING_PARENTHESIS, // '('
        TT_CLOSING_PARENTHESIS  // ')'
    };

    public struct Token
    {
        public TokenType type;
        public string value;

        public Token(TokenType data, string str = null)
        {
            type = data;
            value = str;
        }
    };
}
