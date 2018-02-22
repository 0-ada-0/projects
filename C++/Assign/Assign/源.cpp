#include<iostream>
using namespace std;

class Ada
{
public :
	Ada() {}
	Ada(const Ada& ada)
	{
	}
	Ada& operator=(const Ada& ada)
	{
		return *this;
	}
};

Ada f()
{
	Ada ada;
	cout << &ada << endl;
	return ada;
}

void main()
{
	int a;
	int b = 1;
	Ada ada;
	int c = 1;
	ada = f();
	Ada ada2 = f();
	cout << &ada << endl;
	system("pause");
}