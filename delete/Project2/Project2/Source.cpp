#include <iostream>
using namespace std;

class base {
public:
	void SayHi()
	{
		cout << "base hi!" << endl;
	}
};

class child1:public base {
public:
	void SayHi()
	{
		cout << "child1 hi" << endl;
	}
	void SayHello() {
		cout << "child1 hello!" << endl;
	}
};

class child2 :public base {
public:
	void SayHi()
	{
		cout << "child1 hi" << endl;
	}
	void SayHello() {
		cout << "child1 hello!" << endl;
	}
};

int main()
{
	base * object1 = new child1();
	child1 * object2 = new child1();
	object1->SayHi();
	object2->SayHi();
	system("pause");
	return 0;
}