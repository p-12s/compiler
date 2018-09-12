#include "CalcLexer.h"

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
}

CalcLexer::CalcLexer(std::string_view sources)
	: m_sources(sources)
{
}

Token CalcLexer::Read()
{
	/*
     * Reads next token from input string with following steps:
	 * 1) skips whitespace characters
	 * 2) checks for the end of input
	 * 3) checks first character to select token type
	 * 4) if token may have several characters, read them all
	 */

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
	default:
		break;
	}

	if (IsDigit(next))
	{
		return ReadNumber(next);
	}

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

	while (m_position < m_sources.size() && 
		(IsDigit(m_sources[m_position]) || m_sources[m_position] == '.'))
		//( && !isFractional))
	{
		// ��� ��������������� ��� ���������� ����� �� �����������
		if (m_sources[m_position] == '.') //TODO ����� �����
		{
			isFractional = true;
		}
		
		value += m_sources[m_position];
		++m_position;
	}

	return Token{ TT_NUMBER, value };
}

}
