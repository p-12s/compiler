#include "pch.h"
#include "Lexer.h"

Lexer::Lexer()
{
	m_rules.push("[1-9]+[\\d]*[.]{1}[\\d]+|0{1}[.]{1}[\\d]+|[1-9]{1}[\\d]+|[\\d]{1}", TokenType::TT_NUMBER);
	m_rules.push("\\+", TokenType::TT_PLUS);
	m_rules.push("\\-", TokenType::TT_MINUS);
	m_rules.push("\\*", TokenType::TT_MULTIPLY);
	m_rules.push("\\/", TokenType::TT_DIVIDE);
	m_rules.push("\\=", TokenType::TT_ASSIGN);
	m_rules.push("[a-zA-Z_]{1}[\\w]*", TokenType::TT_ID);
	m_rules.push("\\;", TokenType::TT_SEMICOLON);
	m_rules.push("\\(", TokenType::TT_OPENING_PARENTHESIS);
	m_rules.push("\\)", TokenType::TT_CLOSING_PARENTHESIS);
	m_rules.push("[ \t\r\n]+", m_rules.skip());

	auto lexer = std::make_unique<lexertl::state_machine>();
	lexertl::generator::build(m_rules, *lexer);

	m_stateMachine = std::move(lexer);
}

Lexer::~Lexer()
{
}

std::vector<Token> Lexer::GetTokens(const std::string& source)
{
	std::vector<Token> tokens;

	lexertl::siterator begin(source.begin(), source.end(), *m_stateMachine);
	lexertl::siterator end;
	for (auto& it = begin; it != end; ++it)
	{
		Token token{ (int)it->id, it->str() };
		tokens.emplace_back(token);
	}

	return tokens;
}