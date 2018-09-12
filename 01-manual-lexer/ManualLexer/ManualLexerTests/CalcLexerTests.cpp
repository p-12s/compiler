#include "catch2/catch.hpp"
#include "../ManualLexer/CalcLexer.h"
#include <vector>

using namespace std;
using namespace calc;

namespace calc
{
	bool operator ==(const Token& a, const Token& b)
	{
		return a.type == b.type && a.value == b.value;
	}

	string_view GetTokenName(TokenType type)
	{
		switch (type)
		{
		case calc::TT_END:
			return "end";
		case calc::TT_ERROR:
			return "error";
		case calc::TT_NUMBER:
			return "number";
		case calc::TT_PLUS:
			return "+";
		case calc::TT_MINUS:
			return "-";
		case calc::TT_MULTIPLY:
			return "*";
		case calc::TT_DIVIDE:
			return "/";
		case calc::TT_ASSIGN:
			return "=";
		case calc::TT_ID:
			return "identifier";
		case calc::TT_SEMICOLON:
			return ";";
		case calc::TT_OPENING_PARENTHESIS:
			return "(";
		case calc::TT_CLOSING_PARENTHESIS:
			return ")";
		}
		return "<UNEXPECTED!!!>";
	}

	std::ostream& operator<<(std::ostream& stream, const Token& token)
	{
		stream << "Token(" << GetTokenName(token.type);
		if (token.value)
		{
			stream << ", " << *token.value;
		}
		stream << ")";
		return stream;
	}
}

namespace
{
	using TokenList = vector<Token>;

	TokenList Tokenize(string_view text)
	{
		TokenList results;
		CalcLexer lexer{ text };
		for (Token token = lexer.Read(); token.type != TT_END; token = lexer.Read())
		{
			results.emplace_back(move(token));
		}
		return results;
	}
}

TEST_CASE("Can read one number", "[CalcLexer]") {
	REQUIRE(Tokenize("0"sv) == TokenList{
		Token{ TT_NUMBER, "0"s },
		});
	REQUIRE(Tokenize("1"sv) == TokenList{
		Token{ TT_NUMBER, "1"s },
		});
	REQUIRE(Tokenize("9876543210"sv) == TokenList{
		Token{ TT_NUMBER, "9876543210"s },
		});
}

TEST_CASE("Can read one operator", "[CalcLexer]") {
	REQUIRE(Tokenize("+"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("-"sv) == TokenList{
		Token{ TT_MINUS },
		});
	REQUIRE(Tokenize("*"sv) == TokenList{
		Token{ TT_MULTIPLY },
		});
	REQUIRE(Tokenize("/"sv) == TokenList{
		Token{ TT_DIVIDE },
		});
}

TEST_CASE("Can read expression tokens", "[CalcLexer]") {
	REQUIRE(Tokenize("45+9+28"sv) == TokenList{
		Token{ TT_NUMBER, "45"s },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "9"s },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "28"s },
		});
	REQUIRE(Tokenize("45-9-28"sv) == TokenList{
		Token{ TT_NUMBER, "45"s },
		Token{ TT_MINUS },
		Token{ TT_NUMBER, "9"s },
		Token{ TT_MINUS },
		Token{ TT_NUMBER, "28"s },
		});
	REQUIRE(Tokenize("45*9*28"sv) == TokenList{
		Token{ TT_NUMBER, "45"s },
		Token{ TT_MULTIPLY },
		Token{ TT_NUMBER, "9"s },
		Token{ TT_MULTIPLY },
		Token{ TT_NUMBER, "28"s },
		});
	REQUIRE(Tokenize("45/9/28"sv) == TokenList{
		Token{ TT_NUMBER, "45"s },
		Token{ TT_DIVIDE },
		Token{ TT_NUMBER, "9"s },
		Token{ TT_DIVIDE },
		Token{ TT_NUMBER, "28"s },
		});
	REQUIRE(Tokenize("45/9+28-54*4"sv) == TokenList{
		Token{ TT_NUMBER, "45"s },
		Token{ TT_DIVIDE },
		Token{ TT_NUMBER, "9"s },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "28"s },
		Token{ TT_MINUS },
		Token{ TT_NUMBER, "54"s },
		Token{ TT_MULTIPLY },
		Token{ TT_NUMBER, "4"s },
		});
#if 1 // fractional number support
	REQUIRE(Tokenize("5+7.005"sv) == TokenList{
		Token{ TT_NUMBER, "5" },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "7.005" },
		});
	REQUIRE(Tokenize("1.005+43.54+1"sv) == TokenList{
		Token{ TT_NUMBER, "1.005" },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "43.54" },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "1" },
		});
	REQUIRE(Tokenize("1940.1851/4.000149*1"sv) == TokenList{
		Token{ TT_NUMBER, "1940.1851" },
		Token{ TT_DIVIDE },
		Token{ TT_NUMBER, "4.000149" },
		Token{ TT_MULTIPLY },
		Token{ TT_NUMBER, "1" },
		});
	REQUIRE(Tokenize("05-00.2/0.1*0"sv) == TokenList{
		Token{ TT_ERROR },
		Token{ TT_MINUS },
		Token{ TT_ERROR },
		Token{ TT_DIVIDE },
		Token{ TT_NUMBER, "0.1" },
		Token{ TT_MULTIPLY },
		Token{ TT_NUMBER, "0" },
		});
#endif
}

#if 1 // whitespace support
TEST_CASE("Can read one operator with whitespaces", "[CalcLexer]") {
	REQUIRE(Tokenize("  +"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("\t+"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("   \t\t+"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("\n+"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("   \n  +"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("\t   \n  +"sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("+    "sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("+  \t\t   "sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("+  \n\t   "sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("   +   "sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("  \t +  \t "sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("  \t\t +  \t "sv) == TokenList{
		Token{ TT_PLUS },
		});
	REQUIRE(Tokenize("  \t \t +  \n\t "sv) == TokenList{
		Token{ TT_PLUS },
		});
}

TEST_CASE("Can read one number with whitespaces", "[CalcLexer]") {
	REQUIRE(Tokenize("  1"sv) == TokenList{
		Token{ TT_NUMBER, "1" },
		});
	REQUIRE(Tokenize("\t4"sv) == TokenList{
		Token{ TT_NUMBER, "4" },
		});
	REQUIRE(Tokenize("   \t\t3.2"sv) == TokenList{
		Token{ TT_NUMBER, "3.2" },
		});
	REQUIRE(Tokenize("\n9"sv) == TokenList{
		Token{ TT_NUMBER, "9" },
		});
	REQUIRE(Tokenize("   \n  15"sv) == TokenList{
		Token{ TT_NUMBER, "15" },
		});
	REQUIRE(Tokenize("\t   \n  21.03"sv) == TokenList{
		Token{ TT_NUMBER, "21.03" },
		});
	REQUIRE(Tokenize("0    "sv) == TokenList{
		Token{ TT_NUMBER, "0" },
		});
	REQUIRE(Tokenize("81  \t\t   "sv) == TokenList{
		Token{ TT_NUMBER, "81" },
		});
	REQUIRE(Tokenize("4.2  \n\t   "sv) == TokenList{
		Token{ TT_NUMBER, "4.2" },
		});
	REQUIRE(Tokenize("   7.9   "sv) == TokenList{
		Token{ TT_NUMBER, "7.9" },
		});
	REQUIRE(Tokenize("  \t 3  \t "sv) == TokenList{
		Token{ TT_NUMBER, "3" },
		});
	REQUIRE(Tokenize("  \t\t 9.001  \t "sv) == TokenList{
		Token{ TT_NUMBER, "9.001" },
		});
	REQUIRE(Tokenize("  \t \t 7  \n\t "sv) == TokenList{
		Token{ TT_NUMBER, "7" },
		});
}

TEST_CASE("Can read expression tokens with whitespaces") {
	REQUIRE(Tokenize("2 + 3"sv) == TokenList{
		Token{ TT_NUMBER, "2" },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "3" },
		});
	REQUIRE(Tokenize("\t0.52 + \n4"sv) == TokenList{
		Token{ TT_NUMBER, "0.52" },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "4" },
		});
	REQUIRE(Tokenize("\n+ \t7.1"sv) == TokenList{
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "7.1" },
		});
}
#endif

#if 1 
TEST_CASE("Cannot read number which starts with zero") {
	REQUIRE(Tokenize("0123456789"sv) == TokenList{
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("0.123.456789"sv) == TokenList{
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("0..456789"sv) == TokenList{
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("0.. 456789"sv) == TokenList{
		Token{ TT_ERROR },
		Token { TT_NUMBER, "456789"},
		});
	REQUIRE(Tokenize("01.25"sv) == TokenList{
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("+01"sv) == TokenList{
		Token{ TT_PLUS },
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("+00.32"sv) == TokenList{
		Token{ TT_PLUS },
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("4+0521"sv) == TokenList{
		Token{ TT_NUMBER, "4" },
		Token{ TT_PLUS },
		Token{ TT_ERROR },
		});
	REQUIRE(Tokenize("02+21"sv) == TokenList{
		Token{ TT_ERROR },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "21" },
		});
	REQUIRE(Tokenize("02.4+5.3"sv) == TokenList{
		Token{ TT_ERROR },
		Token{ TT_PLUS },
		Token{ TT_NUMBER, "5.3" },
		});
}
#endif

// extended version of the grammar

#if 1 
TEST_CASE("Can read assigne, semicolon, opening and closing parenthesis", "[CalcLexer]") {
	REQUIRE(Tokenize("5=5"sv) == TokenList{
		Token{ TT_NUMBER, "5" },
		Token{ TT_ASSIGN },
		Token{ TT_NUMBER, "5" },
		});
	REQUIRE(Tokenize("; 5;5;"sv) == TokenList{
		Token{ TT_SEMICOLON },
		Token{ TT_NUMBER, "5" },
		Token{ TT_SEMICOLON },
		Token{ TT_NUMBER, "5" },
		Token{ TT_SEMICOLON },
		});
	REQUIRE(Tokenize("(5)"sv) == TokenList{
		Token{ TT_OPENING_PARENTHESIS },
		Token{ TT_NUMBER, "5" },
		Token{ TT_CLOSING_PARENTHESIS },
		});
}
#endif