#pragma once
#include "../lexertl/iterator.hpp"
#include "../lexertl/generator.hpp"
#include "../lexertl/lookup.hpp"
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

class Lexer
{
public:
	Lexer();
	~Lexer();
	std::vector<Token> GetTokens(const std::string& source);

private:
	lexertl::rules m_rules;
	std::unique_ptr<lexertl::state_machine> m_stateMachine;
};

