#include <iostream>

using namespace std;
#include <any>
#include <functional>
#include <vector>
#include <unordered_map>
#include <memory>

class IocContainer
{
public:
	template<class T, typename Depend>
	void RegisterType(const string& strKey)
	{
		any constructor = new Depend();
		RegisterType(strKey, constructor);
	}

	template<class T>
	T* Resolve(const string& strKey)
	{
		if (m_creatorMap.find(strKey) == m_creatorMap.end())
			return nullptr;
		any resolver = m_creatorMap[strKey];
		T* value = any_cast<*T>(resolver);
		return value;
	}

	template<class T>
	shared_ptr<T> ResolveShared(const string& strKey)
	{
		T* pointer = Resolve<T>(strKey);
		return shared_ptr<T>(pointer);
	}

private:

	void RegisterType(const string& strKey, any& constructor)
	{
		if (m_creatorMap.find(strKey) != m_creatorMap.end())
			cout << "strKey has existed!@" << endl;
		m_creatorMap.emplace(strKey, constructor);
	}

	unordered_map<string, any> m_creatorMap;
};

class Bus
{
public:
	void test() const
	{
		cout << "bus" << endl;
	}
};

class Car
{
public:
	void test() const
	{
		cout << "car" << endl;
	}
};

class Base
{
public:
    virtual	void test() const
	{
		cout << "base" << endl;
	}
};

class ChildA : public Base
{
public :
	void test()const
	{
		cout << "childA" << endl;
	}

};

class ChildB :public Base
{
public:
	void test()const
	{
		cout << "childB" << endl;
	}

};

int main()
{
	IocContainer ioc;
	ioc.RegisterType<Base,ChildA>("A");
	ioc.RegisterType<Base,ChildB>("B");
	auto childA = ioc.ResolveShared<Base>("A");
	childA->test();
	system("pause");
	return 0;
}