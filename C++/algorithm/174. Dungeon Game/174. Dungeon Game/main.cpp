#include <iostream>
#include <vector>
#include <math.h>

//class Solution {
//public:
//	int calculateMinimumHP(std::vector<std::vector<int>>& dungeon) {
//		std::vector<std::vector<int>> dp(dungeon.size() + 1);
//		for (auto i = 0; i < dp.size(); i++)
//		{
//			dp[i].resize(dungeon[0].size() + 1);
//		}
//		for (auto i = 0; i < dp.size(); i++)
//		{
//			for (auto j = 0; j < dp[i].size(); j++)
//			{
//				if (i == dp.size() - 1 || j == dp[i].size() - 1)
//				{
//					dp[i][j] = 0;
//				}
//				else
//					dp[i][j] = INT_MAX;
//			}
//		}
//
//		for (auto i = dungeon.size() - 1; i > 0; --i)
//		{
//			for (auto j = dungeon[i].size() - 1; j > 0; --j)
//			{
//				auto package = dp[i+1][j] > dp[i][j + 1] ? dp[i][j + 1] : dp[i + 1][j];
//				if (package - dungeon[i][j] + 1 < 0)
//				{
//					dp[i][j] = 0;
//					continue;
//				}
//				if (dp[i][j] > package - dungeon[i][j] + 1)
//					dp[i][j] = package - dungeon[i][j] + 1;
//			}
//		}
//		return dp[1][1];
//	}
//};


class Solution {
public:
	int calculateMinimumHP(std::vector<std::vector<int>>& dungeon) {
		auto m = dungeon.size();
		auto n = dungeon[0].size();
		std::vector<std::vector<int> > dp(m + 1, std::vector<int>(n + 1, INT_MAX));
		dp[m][n - 1] = 1;
		dp[m - 1][n] = 1;
		for (auto i = m - 1; i >= 0; i--)
		{
			for (auto j = n - 1; j >= 0; j--)
			{
				auto need = (dp[i + 1][j] > dp[i][j + 1] ? dp[i][j + 1] : dp[i + 1][j]) - dungeon[i][j];
				dp[i][j] = need <= 0 ? 1 : need;
			}
		}
			
		return dp[0][0];
	}
};

//class Solution {
//public:
//	int calculateMinimumHP(std::vector<std::vector<int> > &dungeon) {
//		int M = dungeon.size();
//		int N = dungeon[0].size();
//		// hp[i][j] represents the min hp needed at position (i, j)
//		// Add dummy row and column at bottom and right side
//		std::vector<std::vector<int> > hp(M + 1, std::vector<int>(N + 1, INT_MAX));
//		hp[M][N - 1] = 1;
//		hp[M - 1][N] = 1;
//		for (int i = M - 1; i >= 0; i--) {
//			for (int j = N - 1; j >= 0; j--) {
//				int need = (hp[i + 1][j] > hp[i][j + 1] ? hp[i][j + 1] : hp[i + 1][j]) - dungeon[i][j];
//				hp[i][j] = need <= 0 ? 1 : need;
//			}
//		}
//		return hp[0][0];
//	}
//};

int main()
{
	int n = 3;
	std::vector<std::vector<int>> dungeon(n);
	for each (auto dun in dungeon)
	{
		dun.resize(3);
	}

	dungeon[0].push_back(-2);
	dungeon[0].push_back(-3);
	dungeon[0].push_back(3);

	dungeon[1].push_back(-5);
	dungeon[1].push_back(-10);
	dungeon[1].push_back(1);

	dungeon[2].push_back(10);
	dungeon[2].push_back(30);
	dungeon[2].push_back(-5);
	std::cout << Solution().calculateMinimumHP(dungeon) << std::endl;
	system("pause");
	return 0;
}