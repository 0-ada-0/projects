#include <iostream>
#include <vector>
#include <string>
using namespace::std;


#pragma region β�÷�������1-2
int(*func(int arr[][3], int n))[3]{
	int(*innerArr)[3] = arr;
for (int i = 0;i < n;i++)
	innerArr[i][1] = 2;
return innerArr;
};//����һ����ά����

auto funcTail(int arr[][3], int n)->int(*)[3]{
	int(*innerArr)[3] = arr;
for (int i = 0;i < n;i++)
	innerArr[i][1] = 2;
return innerArr;
}
#pragma endregion

#pragma region ���ӵ㣬β�÷��������﷨��Ч������1-2
int(*(*comfunc())(int arr[][3],int n))[3] {
	return func;
}
auto comfuncTail()->int(*(*)(int arr[][3], int n))[3]{
	return funcTail;
}
#pragma endregion



int main()
{
#pragma region ָ������������ϵ
	/*int a[3] = { 1,2,3 };
	for (auto i = 0;i < 3;i++)
	cout << (*a)++ << endl;*/ //==>(*a)��Ч��a[n]
#pragma endregion

#pragma region β�÷�������2-2
	//int a[5][3];
	//for (int i = 0;i < 5;i++)
	//	a[i][1] = 1;
	//int(*b)[3] = func(a, 5);
	//for (int i = 0;i < 5;i++)
	//	cout << b[i][1] << " ";
	//cout << endl;
	//int(*c)[3] = funcTail(a, 5);
	//for (int i = 0;i < 5;i++)
	//	cout << c[i][1] << " ";
#pragma endregion

#pragma region ���ӵ㣬β�÷��������﷨��Ч������2-2
	int a[5][3];
	for (int i = 0;i < 5;i++)
		a[i][1] = 1;
	int(*b)[3] = comfunc()(a, 5);
	for (int i = 0;i < 5;i++)
		cout << b[i][1] << " ";
	cout << endl;
	int(*c)[3] = comfuncTail()(a, 5);
	for (int i = 0;i < 5;i++)
		cout << c[i][1] << " ";
#pragma endregion

	system("pause");
	return 0;
}