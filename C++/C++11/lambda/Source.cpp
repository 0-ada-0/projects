#include <iostream>
#include <vector>
#include <string>
using namespace::std;

class AddressBook
{
public:
	AddressBook(const vector<string>& address) :_address(address) {}
	template <typename Func>
	vector<string> FindMatchingAddress(Func func)
	{
		vector<string> results;
		for (auto itr = _address.begin(), end = _address.end();itr != end;++itr)
		{
			if (func(*itr))
				results.push_back(*itr);
		}
		return results;
	}
private:
	vector<string> _address;
};

int main()
{
	vector<string> address = { "1","2","3" };
	AddressBook ab(address);
	vector<string> results = ab.FindMatchingAddress([](const string& addr)->bool {
		return addr == "2";
	});
	for (auto itr = results.begin();itr != results.end();++itr)
	{
		cout << *itr << endl;
	}
	system("pause");
	return 0;
}