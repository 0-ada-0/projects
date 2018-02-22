#include <iostream>
using namespace std;

/* 

	1.变量的声明，定义和初始化，声明就是告诉编译器有个这变量，起一个符号的作用，定义是为这个符号找到对应的内存地址，初始化就是给这个内存地址首次赋值。
	2.编译时类型是指编译能确定变量的定义，运行时类型是指运行时才能确定变量的定义。
	3.构造函数是先确定参数，在按照成员变量的顺序去执行初始化列表，如果初始化列表中有，则调用拷贝构造函数，否则调用默认构造函数，最后执行构造函数体内的代码。
	4.给声明的变量定义有多种方式:默认构造函数,拷贝构造函数，而初始化（赋值）变量：拷贝构造函数的第二阶段，赋值运算符。
	5.如果类中存在指针，则需要重写拷贝构造函数和赋值运算符，这关系到对象的深浅拷贝问题。
	6.明确某变量的类型(定义)充要条件：知道这个变量的内存地址。
	7.输出结果：
the default Ada Ctor!
===========
the copy Ada Ctor!
the copy Ada Ctor!
the default Ada Ctor!
===========
the copy Ada Ctor!
the default Ada Ctor!
the default Ada Ctor!
the assign Ada Ctor!
===========
the copy Ada Ctor!
the default Ada Ctor!
the default Ada Ctor!
the copy Ada Ctor!
the assign Ada Ctor!
2
	8.C++中，全局变量会如果有定义的话会初始化默认值，局部变量如果有定义的话并不会初始化默认值。
*/




class Ada
{
public:
	Ada()
	{
		cout << "the default Ada Ctor!" << endl;
	}

	Ada(const Ada& ada)
	{
		cout << "the copy Ada Ctor!" << endl;
	}

	const Ada& operator=(const Ada& ada) const
	{
		cout << "the assign Ada Ctor!" << endl;
		return *this;
	}
	int name{ 2 };
};

class Componment
{
public:
	Componment(Ada ada) : _ada(ada) {}
private:
	Ada _ada;
	Ada *_ada2{ new Ada() };
};

class Componment2
{
public:
	Componment2(Ada ada) { _ada = ada; }
private:
	Ada _ada;
	Ada *_ada2{ new Ada() };
};

class Componment3
{
public:
	Componment3(Ada ada) { _ada = ada; }
public:
	Ada _ada;
	Ada _ada2{ *(new Ada()) };
};

void main()
{
	Ada ada;
	cout << "===========" << endl;
	Componment com(ada);
	cout << "===========" << endl;
	Componment2 com2(ada);
	cout << "===========" << endl;
	Componment3 com3(ada);
	cout << com3._ada.name << endl;
	system("pause");
}