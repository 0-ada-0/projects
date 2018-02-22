#include <iostream>
#include <memory>
#include <any>
#include <vector>
#include <functional>
using namespace std;


class Base
{
public:
	virtual void test()const
	{
		cout << "base" << endl;
	}
};

template<class T>
shared_ptr<T> CreateObj()
{
	T* obj = new T();
	return shared_ptr<T>(obj);
}

template<class T>
shared_ptr<T> CreateFuncObj()
{
	vector<any> vec;
	vec.push_back([] {return new T(); });
	any& baseFunc = vec.back();
	return shared_ptr<T>(any_cast<function<T*()>>(baseFunc)());
}
class aaa
{

public :
    int	a = 3;
};

void test(aaa a)
{
	£ê£á£á£ä£æ£÷£å£á£ó£ä£æ£ç£á£ä£æ


int main()
{
	aaa *a = new aaa();
	test(aaa());
	/*vector<any> vec;
	vec.push_back(new Base());
	any base = vec.back();
	auto bptr = any_cast<shared_ptr<Base>>(base);
	bptr->test();*/

	/*auto baseObj = CreateFuncObj<Base>();
	baseObj->test();*/
	system("pause");
	return 0;
}