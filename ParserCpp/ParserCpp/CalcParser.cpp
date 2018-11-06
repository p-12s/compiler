#include "pch.h"
#include "CalcParser.h"
#include "../lexertl/iterator.hpp"

CalcParser::CalcParser()
{
	lexertl::rules rules;

	rules.push("[1-9]+[\\d]*[.]{1}[\\d]+|0{1}[.]{1}[\\d]+|[1-9]{1}[\\d]+|[\\d]{1}", TokenType::TT_NUMBER);
	rules.push("\\+", TokenType::TT_PLUS);
	rules.push("\\-", TokenType::TT_MINUS);
	rules.push("\\*", TokenType::TT_MULTIPLY);
	rules.push("\\/", TokenType::TT_DIVIDE);
	rules.push("\\=", TokenType::TT_ASSIGN);
	rules.push("[a-zA-Z_]{1}[\\w]*", TokenType::TT_ID);
	rules.push("\\;", TokenType::TT_SEMICOLON);
	rules.push("\\(", TokenType::TT_OPENING_PARENTHESIS);
	rules.push("\\)", TokenType::TT_CLOSING_PARENTHESIS);
	rules.push("[ \t\r\n]+", rules.skip());

	auto lexer = std::make_unique<lexertl::state_machine>();
	lexertl::generator::build(rules, *lexer);

	m_stateMachine = std::move(lexer);
}


void CalcParser::GetToken()
{
	if (m_pointerToToken < m_tokens.size())
	{
		m_currentToken = m_tokens[m_pointerToToken];
		++m_pointerToToken;
	}
	else
	{
		m_currentToken = Token(0, "");
	}
}

double CalcParser::Calculate(const std::string& source)
{
	double result = 0;

	//_tokens = _stateMachine.GetTokens(source);
	lexertl::siterator begin(source.begin(), source.end(), *m_stateMachine);
	lexertl::siterator end;
	for (auto& it = begin; it != end; ++it)
	{
		Token token{ (int)it->id, it->str() };
		m_tokens.emplace_back(token);
	}

	GetToken();
	if (m_currentToken.id == 0) return result;
	if (m_currentToken.id == TT_SEMICOLON) return result;

	result = Expr(false);
	m_pointerToToken = 0;

	// Ќ”∆Ќќ пон€ть, что лишнее в этом классе и разнести их в разные

	// попробуем искус-но обнулить
	m_tokens.clear();

	return result;
	// ќ—“јЌќ¬»Ћя “”“: m_tokens не обнул€етс€, токены складируютс€ друг за другом но считываетс€ только 1 экспрешен
}

double CalcParser::Error(const std::string& message)
{
	m_noOfErrors++;
	std::cout << "error: " << message << std::endl;
	return 1;
}

double CalcParser::Prim(bool get)
{
	if (get) GetToken();

	std::string stringValue;
	switch (m_currentToken.id)
	{
	case TokenType::TT_NUMBER:
	{
		m_numberValue = atof(m_currentToken.value.c_str());
		GetToken();
		return m_numberValue;
	}
	case TokenType::TT_ID:
	{
		stringValue = m_currentToken.value;

		double v1;

		//auto it = m_variables.find(m_currentToken.value);
		if (m_variables.find(m_currentToken.value) != m_variables.end())
		{
			v1 = m_variables[m_currentToken.value];
			//v1 = Convert.ToDouble(_variables[_currentToken.Value]);
		}
		else
		{
			m_variables.emplace(m_currentToken.value, 0);
			v1 = m_variables[m_currentToken.value];
		}

		GetToken();
		if (m_currentToken.id == TokenType::TT_ASSIGN)
		{
			v1 = Expr(true);
			m_variables[stringValue] = v1;
		}
		return v1;
	}
	case TokenType::TT_MINUS:
	{
		return -Prim(true);
	}
	case TokenType::TT_OPENING_PARENTHESIS:
	{
		double e = Expr(true);
		if (m_currentToken.id != TokenType::TT_CLOSING_PARENTHESIS)
			return Error(") expected");
		GetToken();
		return e;
	}
	default:
		return Error("primary expected");		
	}
}

double CalcParser::Expr(bool get)
{
	double left = Term(get);

	for ( ; ; )
	{
		switch (m_currentToken.id)
		{
		case TokenType::TT_PLUS:
			left += Term(true);
			break;

		case TokenType::TT_MINUS:
			left -= Term(true);
			break;

		default:
			return left;
		}
	}
}

double CalcParser::Term(bool get)
{
	double left = Prim(get);

	for ( ; ; )
	{
		switch (m_currentToken.id)
		{
			case TokenType::TT_MULTIPLY:
			{
				left *= Prim(true);
				break;
			}
			case TokenType::TT_DIVIDE:
			{
				double d = Prim(true);
				if (d == 0)
					return Error("divide by 0");

				left /= d;
				break;
			}
			default:
				return left;
		}
	}
}