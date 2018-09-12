#pragma once

#include <string>
#include <optional>

namespace calc
{

enum TokenType
{
	TT_END = 0,
	TT_ERROR,
	TT_NUMBER,
	TT_PLUS,
	TT_MINUS,
	TT_MULTIPLY,
	TT_DIVIDE,
	TT_ASSIGN,				// '='
	TT_ID,
	TT_SEMICOLON,			// ';'
	TT_OPENING_PARENTHESIS, // '('
	TT_CLOSING_PARENTHESIS	// ')'
};

struct Token
{
	TokenType type = TT_END;
	std::optional<std::string> value;
};

}
