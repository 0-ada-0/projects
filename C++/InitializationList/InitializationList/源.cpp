#include <iostream>
using namespace std;

/* 

	1.����������������ͳ�ʼ�����������Ǹ��߱������и����������һ�����ŵ����ã�������Ϊ��������ҵ���Ӧ���ڴ��ַ����ʼ�����Ǹ�����ڴ��ַ�״θ�ֵ��
	2.����ʱ������ָ������ȷ�������Ķ��壬����ʱ������ָ����ʱ����ȷ�������Ķ��塣
	3.���캯������ȷ���������ڰ��ճ�Ա������˳��ȥִ�г�ʼ���б������ʼ���б����У�����ÿ������캯�����������Ĭ�Ϲ��캯�������ִ�й��캯�����ڵĴ��롣
	4.�������ı��������ж��ַ�ʽ:Ĭ�Ϲ��캯��,�������캯��������ʼ������ֵ���������������캯���ĵڶ��׶Σ���ֵ�������
	5.������д���ָ�룬����Ҫ��д�������캯���͸�ֵ����������ϵ���������ǳ�������⡣
	6.��ȷĳ����������(����)��Ҫ������֪������������ڴ��ַ��
	7.��������
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
	8.C++�У�ȫ�ֱ���������ж���Ļ����ʼ��Ĭ��ֵ���ֲ���������ж���Ļ��������ʼ��Ĭ��ֵ��
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