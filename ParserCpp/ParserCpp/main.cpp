#include "pch.h"
#include "../lexertl/generator.hpp"
#include "../lexertl/lookup.hpp"
#include <iostream>
#include "CalcParser.h"

using namespace std;

/*class Lexer
{
	Lexer();

	std::vector<Token> GetTokens(const std::string& source
};

class Parser
{
	Parser(std::unique_ptr<Lexer>&& lexer);

	double Calculcate(string source)
	{
		auto tokens = m_lexer.GetTokens();

	}

private:
	slexer
};*/

int main()
{
	CalcParser lexer;

	auto a1 = lexer.Calculate("1+2+3");
	auto a = 9;
	/*while (std::cin)
	{
		std::string line;
		std::getline(std::cin, line);
		for (auto &token : TokenizeLine(*lexer, line))
		{
			std::cout << "Id: " << token.id << ", Token: '" << token.str() << "'\n";
		}
	}*/

	return 0;
}