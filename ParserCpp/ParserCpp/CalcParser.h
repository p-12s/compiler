#pragma once
#include "pch.h"
#include "../lexertl/generator.hpp"
#include "../lexertl/lookup.hpp"
#include <string>
#include <iostream>
#include <memory>
#include "TokenType.h"

struct Token
{
	int id;
	std::string value;

	Token(int id, const std::string& value)
		:id(id), value(value)
	{
	}
};

class CalcParser
{
private:
	std::map<std::string, double> m_variables;
	double m_numberValue = 0.0f;
	int m_noOfErrors = 0;
	int m_pointerToToken = 0;
	std::vector<Token> m_tokens;
	std::unique_ptr<lexertl::state_machine> m_stateMachine;
	Token m_currentToken{ TT_SEMICOLON, ";" };

public:
	CalcParser();

	double Calculate(const std::string& source);

private:
	double Expr(bool get);

	void GetToken();

	double Error(const std::string& message);

	double Prim(bool get);

	double Term(bool get);
};
