#pragma once
#include "pch.h"
#include "../lexertl/generator.hpp"
#include "../lexertl/lookup.hpp"
#include <string>
#include <iostream>
#include <memory>
#include "Lexer.h"

class CalcParser
{
private:
	std::map<std::string, double> m_variables;
	double m_numberValue = 0.0f;
	int m_pointerToToken = 0;
	Token m_currentToken{ TT_SEMICOLON, ";" };
	Lexer m_lexer;
	std::vector<Token> m_tokens;

public:
	CalcParser();

	double Calculate(const std::string& source);

private:
	double Expr(bool get);

	void NextToken();

	double Error(const std::string& message);

	double Prim(bool get);

	double Term(bool get);
};
