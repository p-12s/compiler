#include "pch.h"
#include "../lexertl/generator.hpp"
#include "../lexertl/lookup.hpp"
#include <iostream>
#include "CalcParser.h"

using namespace std;

int main()
{
	CalcParser lexer;
	string input;
	while (true)
	{
		getline(cin, input);
		cout << lexer.Calculate(input) << endl;
	}
	
	return 0;
}