#pragma once

enum TokenType
{
	//TT_ERROR = 1,
	TT_NUMBER = 1,
	TT_PLUS = 2,
	TT_MINUS = 3,
	TT_MULTIPLY = 4,
	TT_DIVIDE = 5,
	TT_ASSIGN = 6,                  // '='
	TT_ID = 7,
	TT_SEMICOLON = 8,               // ';' 
	TT_OPENING_PARENTHESIS = 9,    // '(' LP
	TT_CLOSING_PARENTHESIS = 10     // ')' RP
};