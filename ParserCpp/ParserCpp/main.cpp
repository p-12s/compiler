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
	/*
	auto a1 = lexer.Calculate("1+2+3");
	cout << a1 << endl;
	auto a = 9;
	*/
	// ОСТАНОВИЛСЯ ТУТ: надо сделать непрерывный ввод, а потом тесты

	/*while (std::cin)
	{
		std::string line;
		std::getline(std::cin, line);

		auto a1 = lexer.Calculate(line);
		cout << a1 << endl;
	}*/

	//table["pi"] = 3.1415926535897932385;
	//table["e"] = 2.7182818284590452354;

	//CalcParser parser = new CalcParser();

	string input;
	while (true)
	{
		getline(cin, input);
		cout << lexer.Calculate(input) << endl;
	}
	/*
	while (cin) {
		//lexer.
		//CalcParser::GetToken();
		if (curr_tok == END) break;
		if (curr_tok == PRINT) continue;
		cout << expr(false) << '\n';
	}
	*/
	
	return 0;
}