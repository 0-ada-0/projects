#include <iostream>

using namespace std;

class base {
public:
   	void SayHi() {
		cout << "base hi" << endl;
	}
};

class child1 : public base{
public:
	virtual void SayHi()
	{
		cout << "child1 hi" << endl;
	}
	void SayHello() {
		cout << "child1 hello" << endl;
	}
};

class grandChild1 :public child1 {
public:
	void SayHi() override {
		cout << "grandchild1 hi" << endl;
	}

};

void main() {
	base* obj1 = new child1();
	child1 * obj2 = new child1();
	base* obj3 = new grandChild1();
	obj1->SayHi();
	obj1->
	obj2->SayHi();
	obj3->SayHi();
	system("pause");
}