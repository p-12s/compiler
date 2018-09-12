#include "CalcLexer.h"
#include <stdio.h>
#include <ctype.h>

namespace calc
{
namespace
{
	bool IsDigit(char ch)
	{
		/*
		 * Returns true if given character is digit.
		 */
		switch (ch)
		{
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			return true;
		default:
			return false;
		}
	}

	bool CanBeStartOfIdentifier(char ch)
	{
		return isalpha(ch) || ch == '_';
	}
}

CalcLexer::CalcLexer(std::string_view sources)
	: m_sources(sources)
{
}

Token CalcLexer::Read()
{
	SkipSpaces();

	if (m_position >= m_sources.size())
	{
		return Token{ TT_END };
	}

	char next = m_sources[m_position];
	++m_position;

	switch (next)
	{
	case '+':
		return Token{ TT_PLUS };
	case '-':
		return Token{ TT_MINUS };
	case '*':
		return Token{ TT_MULTIPLY };
	case '/':
		return Token{ TT_DIVIDE };
	case '=':
		return Token{ TT_ASSIGN };
	case ';':
		return Token{ TT_SEMICOLON };
	case '(':
		return Token{ TT_OPENING_PARENTHESIS };
	case ')':
		return Token{ TT_CLOSING_PARENTHESIS };
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
	// ������������� ���. �� � �����

	return Token{ TT_ERROR };
}

void CalcLexer::SkipSpaces()
{
	while (m_position < m_sources.size() &&
			(m_sources[m_position] == ' ' ||
			m_sources[m_position] == '\t' ||
			m_sources[m_position] == '\n'))
	{
		++m_position;
	}
}

Token CalcLexer::ReadIdentifier(char head)
{
	std::string value;
	value += head;

	while (m_position < m_sources.size())
	{
		value += m_sources[m_position];

		++m_position;
	}

	return Token{ TT_ID, value };
}

Token CalcLexer::ReadNumber(char head)
{
	/*
	 * Reads the tail of number token and returns this token.
	 * PRECONDITION: first character already read.
	 * POSTCONDITION: all number characters have been read.
	 */
	bool isSkeep = false; 
	bool isFractional = false;
	std::string value;
	value += head;

	if (m_position < m_sources.size() && 
		head == '0' && IsDigit(m_sources[m_position])) // 01 or some not 0.
	{
		isSkeep = true;
	}

	while (m_position < m_sources.size() && 
		(IsDigit(m_sources[m_position]) || m_sources[m_position] == '.'))
	{
		// already fractional! skeep
		if (m_sources[m_position] == '.' && isFractional)
		{
			isSkeep = true;
		}

		if (!isSkeep)
		{
			if (m_sources[m_position] == '.')
			{
				isFractional = true;
			}

			value += m_sources[m_position];
		}
		
		++m_position;
	}

	return isSkeep ? Token{ TT_ERROR } : Token{ TT_NUMBER, value };
}

}
