#include <iostream>
using namespace std;

void test(int* &i)
{
	i++;
}

int main() 
{
	int j = 3;
	int* i = &j;
	test(&i);
	cout << &i << endl;
	cout << i << endl;
	cout << *i << endl;
	system("pause");
}