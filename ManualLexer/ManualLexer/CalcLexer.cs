namespace ManualLexer
{
    public class CalcLexer
    {
        private string m_sources;
        private int m_position = 0;

        public CalcLexer(string sources)
        {
            m_sources = sources;
        }

        public Token Read()
        {
            SkipSpaces();

            if (m_position >= m_sources.Length)
            {
                return new Token(TokenType.TT_END);
            }

            char next = m_sources[m_position];
            ++m_position;

            switch (next)
            {
                case '+':
                    return new Token(TokenType.TT_PLUS);
                case '-':
                    return new Token(TokenType.TT_MINUS);
                case '*':
                    return new Token(TokenType.TT_MULTIPLY);
                case '/':
                    return new Token(TokenType.TT_DIVIDE);
                case '=':
                    return new Token(TokenType.TT_ASSIGN);
                case ';':
                    return new Token(TokenType.TT_SEMICOLON);
                case '(':
                    return new Token(TokenType.TT_OPENING_PARENTHESIS);
                case ')':
                    return new Token(TokenType.TT_CLOSING_PARENTHESIS);
                default:
                    break;
            }

            if (IsDigit(next))
            {
                return ReadNumber(next);
            }

            if (CanBeStartOfIdentifier(next))
            {
                return ReadIdentifier(next);
            }

            return new Token(TokenType.TT_ERROR);
        }

        private void SkipSpaces()
        {
            while (m_position < m_sources.Length &&
                (m_sources[m_position] == ' ' ||
                m_sources[m_position] == '\t' ||
                m_sources[m_position] == '\n'))
            {
                ++m_position;
            }
        }

        private Token ReadNumber(char head)
        {
            bool wasIncorrectSequence = false;
            bool isFractional = false;
            string value = head.ToString();

            if (m_position < m_sources.Length &&
                    (
                    (head == '0' && IsDigit(m_sources[m_position])) || // 01 or some not 0.
                    CanBePartOfIdentifier(m_sources[m_position])
                    )
                )
            {
                wasIncorrectSequence = true;
            }

            while (m_position < m_sources.Length &&
                    (
                        IsDigit(m_sources[m_position]) ||
                        m_sources[m_position] == '.' ||
                        CanBePartOfIdentifier(m_sources[m_position])
                    )
                )
            {
                if ( (m_sources[m_position] == '.' && isFractional) ||
                    CanBePartOfIdentifier(m_sources[m_position])
                    )
                {
                    wasIncorrectSequence = true;
                }

                if (!wasIncorrectSequence)
                {
                    if (m_sources[m_position] == '.')
                    {
                        isFractional = true;
                    }

                    value += m_sources[m_position];
                }

                ++m_position;
            }

            return wasIncorrectSequence ? 
                new Token(TokenType.TT_ERROR) : 
                new Token(TokenType.TT_NUMBER, value);
        }

        private Token ReadIdentifier(char head)
        {
            string value = head.ToString();

            while (
                (m_position < m_sources.Length) &&
                    (
                        CanBePartOfIdentifier(m_sources[m_position]) || 
                        char.IsDigit(m_sources[m_position])
                    )
                )
            {
                value += m_sources[m_position];
                ++m_position;
            }

            return new Token(TokenType.TT_ID, value);
        }

        private bool IsDigit(char ch)
        {
            return char.IsDigit(ch);
        }

        private bool CanBeStartOfIdentifier(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }

        private bool CanBePartOfIdentifier(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }
    }
}
