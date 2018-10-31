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
	/*lexertl::state_machine m_stateMachine;
	Token m_currentToken;
	double m_numberValue;
	std::map<std::string, double> m_variables;
	int m_noOfErrors = 0;
	std::vector<Token> m_tokens;
	int m_pointerToToken;//указатель?
	*/

	int m_pointerToToken = 0;
	std::vector<Token> m_tokens;

	//std::make_unique<lexertl::state_machine> m_lexer;
	std::unique_ptr<lexertl::state_machine> _stateMachine;

	Token m_currentToken{ TT_SEMICOLON, ";" };

public:
	CalcParser();

	double Calculate(const std::string& source);

private:
	double Expr(bool get);

	void GetToken();

	/*double Error(std::string message);

	double Prim(bool get);

	double Term(bool get);

	*/
};
