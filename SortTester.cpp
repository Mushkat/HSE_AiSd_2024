#include <iostream>
#include <vector>
#include <chrono>
#include "HybridMergeSort.cpp"
#include "ArrayGenerator.cpp"

class SortTester {
public:
    static int SortTesting() {
        int maxSize = 10000;
        int valueRange = 6000;
        int step = 100;
        int trials = 10;

        ArrayGenerator generator(maxSize, valueRange);
        //std::ofstream outFile("hybrid_merge_sort_results5.csv");
        //outFile << "Size,Type,Time\n";

        std::vector<std::string> types = {"random", "reversed", "nearly_sorted"};

        for (const auto &type: types) {
            std::cout << "Processing type: " << type << "\n";
            for (int size = 500; size <= maxSize; size += step) {
                long long totalTime = 0;

                for (int t = 0; t < trials; ++t) {
                    auto array = generator.getArray(type, size);
                    totalTime += hybridMeasureTime(array);
                }

                long long avgTime = totalTime / trials;
                //outFile << size << "," << type << "," << avgTime << "\n";

                std::cout << "Size: " << size << " Avg Time: " << avgTime << "sec\n";
            }
        }

        //outFile.close();
        //std::cout << "Results saved to merge_sort_results.csv\n";
        return 0;
    }
};
