#include <iostream>
using namespace std;
class PointerC {
public:
	int c;
	PointerC(int cc) {
		this->c = cc;
	}
};

void ChangeC(void* cc)
{
	cout << &cc << endl;
	cout << cc << endl;
	cc = new PointerC(2);
	cout << static_cast<PointerC*>(cc)->c << endl;
}

int main()
{
	PointerC* cc = new PointerC(1);
	cout << &cc << endl;
	ChangeC(&cc);
	cout << &cc << endl;
	cout << cc->c << endl;
	system("pause");
	return 0;
}