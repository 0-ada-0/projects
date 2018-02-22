#include <iostream>
#include <vector>

class Solution {
public:
	int coinChange(std::vector<int>& coins, int amount) {
		auto max = amount + 1;
		std::vector<int> f(amount + 1, max);
		f[0] = 0;
		for (auto s = 1; s <= amount; s++)
		{
			for each (auto coin in coins)
			{
				if (s - coin >= 0)
					if (f[s] > f[s - coin] + 1)
						f[s] = f[s - coin] + 1;
			}
		}
		if (f[amount] == max)
			return -1;
		return f[amount];
	}
};

int main()
{
	int n[] = { 2 };
	std::vector<int> coins(n, n+1);
	auto amount = 3;
	auto coinNum = Solution().coinChange(coins,amount);
	std::cout << "coinnum:" << coinNum << std::endl;
	system("pause");
	return 0;
}
